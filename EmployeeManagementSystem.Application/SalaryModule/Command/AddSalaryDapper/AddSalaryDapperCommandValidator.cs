using EmployeeManagementSystem.Application.Configuration;
using EmployeeManagementSystem.Application.Interface;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Application.SalaryModule.Command.AddSalaryDapper
{
    public class AddSalaryDapperCommandValidator:AbstractValidator<AddSalaryDapperCommand>
    {
        private readonly IAppDbContext appDbContext;
        private readonly ISalaryDbContext salaryDbContext;

        public AddSalaryDapperCommandValidator(ISalaryDbContext _salaryDbContext, IAppDbContext _appDbContext)
        {
            appDbContext= _appDbContext;
            salaryDbContext= _salaryDbContext;
            RuleFor(x => x.EmployeeId)
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

            RuleFor(x=>x.SalaryDate)
                .NotEmpty().WithMessage("SalaryDate is required.")
                .Must(date => date <= DateTime.Now).WithMessage("SalaryDate cannot be in the future.");


            RuleFor(x => x.SalaryDate)
                .NotEmpty().WithMessage("SalaryDate is required.")
                .Must(date => date <= DateTime.Now).WithMessage("SalaryDate cannot be in the future.");

            RuleFor(x => x)
               .MustAsync(async (command, cancellationToken) =>
                {
                 var exists = await salaryDbContext.Salaries
                 .AnyAsync(s => s.EmployeeId == command.EmployeeId
                     && s.SalaryDate.Month == command.SalaryDate.Month
                     && s.SalaryDate.Year == command.SalaryDate.Year,
                     cancellationToken);

               return !exists; // return true if no duplicate exists
                })
               .WithMessage("Salary for this employee has already been added for the selected month.");

                }


    }
}
