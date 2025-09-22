using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Application.SalaryModule.DTO;
using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Domain.Enums;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.SalaryModule.Command
{
    public record AddSalaryCommand(Guid EmployeeId, decimal BasicSalary, decimal Conveyance, decimal Tax, decimal Pf, decimal ESI, string PaymentMethod, string Status ):IRequest<SalaryResponseDto>;

    public class AddSalaryCommandHandler : IRequestHandler<AddSalaryCommand, SalaryResponseDto>
    {
        private readonly ISalaryDbContext salaryDbContext;
        public AddSalaryCommandHandler(ISalaryDbContext _salaryDbContext)
        {
            salaryDbContext = _salaryDbContext;
        }

        public async Task<SalaryResponseDto> Handle(AddSalaryCommand request, CancellationToken cancellationToken)
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
                Status= statusEnum,
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            };

            salaryDbContext.Salarys.Add(salary);
            await salaryDbContext.SaveChangesAsync(cancellationToken);

            return new SalaryResponseDto
            {
                Id = salary.Id,
                EmployeeId = salary.EmployeeId,
                BasicSalary = salary.BasicSalary,
                Conveyance = salary.Conveyance,
                Tax = salary.Tax,
                PF = salary.PF,
                ESI = salary.ESI,
                PaymentMode = salary.PaymentMode.ToString(),
                Status=salary.Status.ToString(),
                CreatedAt = salary.CreatedAt,
                GrossSalary = salary.GrossSalary,
                NetSalary = salary.NetSalary
            };
        }
    }
    
}
