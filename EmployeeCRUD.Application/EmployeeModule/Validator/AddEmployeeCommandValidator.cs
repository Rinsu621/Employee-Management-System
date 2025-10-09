
using EmployeeCRUD.Application.EmployeeModule.Commands;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Validator
{
    public class AddEmployeeCommandValidator:AbstractValidator<AddEmployeeCommand>

    {
        private readonly IAppDbContext dbContext;
        public AddEmployeeCommandValidator(EmployeeDtoValidator employeeValidator, IAppDbContext _dbContext)
        {
            dbContext = _dbContext;

         
                RuleFor(e => e.EmpName)
                .NotEmpty().WithMessage("Employee name is required")
                .MaximumLength(100).WithMessage("Employee name must not exceed 100 characters.");

                RuleFor(e => e.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MustAsync(async (email, cancellation) =>
                !await dbContext.Employees.AnyAsync(x => x.Email == email, cancellation))
                .WithMessage("Email already exists.");

                RuleFor(e => e.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^98\d{8}$").WithMessage("Phone number must be 10 digits and must start with 98.");


            //RuleFor(x => x.employee)
            //.NotNull().WithMessage("Employee data is required.")
            //.SetValidator(employeeValidator);

        }

    }
}
