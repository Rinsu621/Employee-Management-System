using EmployeeManagementSystem.Application.Interface;
using EmployeeManagementSystem.Domain.Entities;
using EmployeeManagementSystem.Domain.Enums;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Application.SalaryModule.Command.AddSalary
{
    public class AddSalaryCommandHandler : IRequestHandler<AddSalaryCommand, Guid>
    {
        private readonly IAppDbContext appDbContext;
        private readonly ISalaryDbContext salaryDbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public AddSalaryCommandHandler(ISalaryDbContext _salaryDbContext, IAppDbContext _appDbContext, IHttpContextAccessor httpContextAccessor)
        {
            salaryDbContext = _salaryDbContext;
            appDbContext = _appDbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Guid> Handle(AddSalaryCommand request, CancellationToken cancellationToken)
        {
            if (!Enum.TryParse<PaymentMethod>(request.PaymentMethod, true, out var paymentEnum))
            {
                throw new ArgumentException($"Invalid PaymentMethod: {request.PaymentMethod}");
            }

            if (!Enum.TryParse<SalaryStatus>(request.Status, true, out var statusEnum))
            {
                throw new ArgumentException($"Invalid Status: {request.Status}");
            }
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new Exception("User Id not found in token");
            var createdBy = Guid.Parse(userIdClaim.Value);
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
                SalaryDate = request.SalaryDate,
                CreatedBy = createdBy,
                CreatedAt = DateTime.Now,
                UpdatedAt = null
            };

            salaryDbContext.Salaries.Add(salary);
            await salaryDbContext.SaveChangesAsync(cancellationToken);
           
            return salary.Id;
        }
    }

}
