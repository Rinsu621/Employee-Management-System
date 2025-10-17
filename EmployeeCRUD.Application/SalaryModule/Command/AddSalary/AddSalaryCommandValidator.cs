using EmployeeCRUD.Application.Interface;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.SalaryModule.Command.AddSalary
{
    public class AddSalaryCommandValidator:AbstractValidator<AddSalaryCommand>

    {
        private readonly IAppDbContext appDbContext;
        private readonly ISalaryDbContext salaryDbContext;
        public AddSalaryCommandValidator(ISalaryDbContext _salaryDbContext, IAppDbContext _appDbContext)
        {
            salaryDbContext = _salaryDbContext;
            appDbContext = _appDbContext;
            RuleFor(x=> x.EmployeeId)
                .NotEmpty().WithMessage("EmployeeId is required.")
                .MustAsync(async (employeeId, cancellationToken) =>
                {
                    var employeeExists = await appDbContext.Employees.FindAsync(new object[] { employeeId }, cancellationToken);
                    return employeeExists != null;
                }).WithMessage("Employee with the given EmployeeId does not exist.");

            RuleFor(x => x.BasicSalary)
                .NotEmpty().WithMessage("BasicSalary is required.")
                .GreaterThan(0).WithMessage("Basic Salary must be greater than zero");

            RuleFor(x => x.Conveyance)
                .NotEmpty().WithMessage("Conveyance is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Conveyance must be greater than zero");

            RuleFor(x => x.Tax)
                .NotEmpty().WithMessage("Tax is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Tax must be greater than zero");

            RuleFor(x => x.Pf) 
                .NotEmpty().WithMessage("Pf is required.")
                .GreaterThanOrEqualTo(0).WithMessage("Pf must be greater than zero");

            RuleFor(x => x.ESI)
                .NotEmpty().NotEmpty().WithMessage("ESI is required.")
                .GreaterThanOrEqualTo(0).WithMessage("ESI must be greater than zero");


        }
    }
}
