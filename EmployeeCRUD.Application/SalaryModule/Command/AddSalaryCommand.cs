using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Application.SalaryModule.Document;
using EmployeeCRUD.Application.SalaryModule.DTO;
using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Domain.Enums;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.SalaryModule.Command
{
    public record AddSalaryCommand(Guid EmployeeId, decimal BasicSalary, decimal Conveyance, decimal Tax, decimal Pf, decimal ESI, string PaymentMethod, string Status, DateTime SalaryDate) : IRequest<byte[]>;

    public class AddSalaryCommandHandler : IRequestHandler<AddSalaryCommand, byte[]>
    {
        private readonly IAppDbContext appDbContext;
        private readonly ISalaryDbContext salaryDbContext;
        private readonly IPdfService pdfService;
        private readonly ISalaryEmailService salaryEmailService;
        private readonly UserManager<ApplicationUser> userManager;
        public AddSalaryCommandHandler(ISalaryDbContext _salaryDbContext, IPdfService _pdfService, IAppDbContext _appDbContext, ISalaryEmailService _salaryEmailService, UserManager<ApplicationUser> _userManager)
        {
            salaryDbContext = _salaryDbContext;
            pdfService = _pdfService;
            appDbContext= _appDbContext;
            salaryEmailService = _salaryEmailService;
            userManager = _userManager;
        }

        public async Task<byte[]> Handle(AddSalaryCommand request, CancellationToken cancellationToken)
        {
            if (!Enum.TryParse<PaymentMethod>(request.PaymentMethod, true, out var paymentEnum))
            {
                throw new ArgumentException($"Invalid PaymentMethod: {request.PaymentMethod}");
            }

            if (!Enum.TryParse<SalaryStatus>(request.Status, true, out var statusEnum))
            {
                throw new ArgumentException($"Invalid Status: {request.Status}");
            }

            var salary = new Salary
            {
                Id = Guid.NewGuid(),
                EmployeeId = request.EmployeeId,
                BasicSalary = request.BasicSalary,
                Conveyance = request.Conveyance,
                Tax = request.Tax,
                PF = request.Pf,
                ESI = request.ESI,
                PaymentMode = paymentEnum,
                Status = statusEnum,
                SalaryDate= request.SalaryDate,
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            };

            salaryDbContext.Salaries.Add(salary);
            await salaryDbContext.SaveChangesAsync(cancellationToken);

            var employee = await appDbContext.Employees
                .Where(e => e.Id == request.EmployeeId)
                .Include(e => e.Department) // Include department
               .FirstOrDefaultAsync(cancellationToken);
            var user = await userManager.FindByEmailAsync(employee.Email);
            var roles = await userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault();



            var salaryPdf = new SalaryPdfModel
            {
                EmployeeName = employee.EmpName ?? "-",
                Department= employee.Department.DeptName?? "-",
                Role= role,
                Joined = employee.CreatedAt.ToString("dd MMM yyyy"),
        
                BasicSalary = salary.BasicSalary,
                Conveyance = salary.Conveyance,
                Tax = salary.Tax,
                PF = salary.PF,
                ESI = salary.ESI,
                PaymentMode = salary.PaymentMode.ToString(),
                Status = salary.Status.ToString(),
                GrossSalary = salary.GrossSalary,
                NetSalary = salary.NetSalary,
                SalaryMonth = salary.SalaryDate.ToString("MMMM yyyy"),
                CreatedAt = salary.CreatedAt
            };

            var salaryDoc = new SalaryTablePdf(salaryPdf);
            var pdfBytes = pdfService.GeneratePdf(salaryDoc);
            var employeeEmail = await appDbContext.Employees
          .Where(e => e.Id == request.EmployeeId)
             .Select(e => e.Email)
             .FirstOrDefaultAsync(cancellationToken);
            BackgroundJob.Enqueue<ISalaryEmailService>(x =>
    x.SendSalarySlipAsync(employeeEmail, pdfBytes, "SalarySlip.pdf", salaryPdf)
);
            return pdfBytes;
        }
    }
    
}
