using EmployeeManagementSystem.Application.Interface;
using EmployeeManagementSystem.Application.SalaryModule.Document;
using EmployeeManagementSystem.Application.SalaryModule.DTO;
using EmployeeManagementSystem.Domain.Entities;
using Hangfire;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Application.SalaryModule.Command
{
    public record GenerateSalaryPdfCommand(Guid SalaryId) : IRequest<byte[]>;

    public class GenerateSalaryPdfHandler : IRequestHandler<GenerateSalaryPdfCommand, byte[]>
    {
        private readonly ISalaryDbContext salaryDbContext;
        private readonly IAppDbContext appDbContext;
        private readonly IPdfService pdfService;
        private readonly UserManager<ApplicationUser> userManager;

        public GenerateSalaryPdfHandler(ISalaryDbContext _salaryDbContext, IAppDbContext _appDbContext, IPdfService _pdfService, UserManager<ApplicationUser> _userManager)
        {
            salaryDbContext = _salaryDbContext;
            appDbContext = _appDbContext;
            pdfService = _pdfService;
            userManager = _userManager;
        }
        public async Task<byte[]> Handle(GenerateSalaryPdfCommand request, CancellationToken cancellationToken)
        {
            var salary = await salaryDbContext.Salaries
                  .FirstOrDefaultAsync(s => s.Id == request.SalaryId, cancellationToken);

            var employee = await appDbContext.Employees
                .Where(e => e.Id == salary.EmployeeId).Include(e => e.Department).FirstOrDefaultAsync();

            var user= await userManager.FindByEmailAsync(employee.Email);
            var roles= await userManager.GetRolesAsync(user);
            var role= roles.FirstOrDefault();

            var model = new SalaryPdfModel
            {
                EmployeeName = employee.EmpName ?? "-",
                Department = employee.Department?.DeptName ?? "-",
                Role = role,
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
            var doc = new SalaryTablePdf(model);
            var pdfBytes = pdfService.GeneratePdf(doc);

            BackgroundJob.Enqueue<IMediator>(mediator =>
                     mediator.Send(new SalaryEmailCommand(employee.Email, pdfBytes, "SalarySlip.pdf", model), default(CancellationToken))
                 );


            return pdfBytes;
        }

    }
}
