
using EmployeeCRUD.Application.EmployeeModule.Commands;
using EmployeeCRUD.Domain.Interface;
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
        public AddEmployeeCommandValidator(EmployeeDtoValidator employeeValidator)
        {
            //dbContext=_dbContext;

            //RuleFor(e=>e.employee).NotNull().WithMessage("Employee data is required.");

            //When(e => e.employee != null, () =>
            //{
            //    RuleFor(e => e.employee.EmpName)
            //    .NotEmpty().WithMessage("Employee name is required")
            //    .MaximumLength(100).WithMessage("Employee name must not exceed 100 characters.");

            //    RuleFor(e => e.employee.Email)
            //    .NotEmpty().WithMessage("Email is required.")
            //    .EmailAddress().WithMessage("Invalid email format.")
            //    .MustAsync(async (email, cancellation) =>
            //    !await dbContext.Employees.AnyAsync(x => x.Email == email, cancellation))
            //    .WithMessage("Email already exists.");

            //    RuleFor(e => e.employee.Phone)
            //    .NotEmpty().WithMessage("Phone number is required.")
            //    .Matches(@"^98\d{8}$").WithMessage("Phone number must be 10 digits and must start with 98.");
            //});

            RuleFor(x => x.employee)
            .NotNull().WithMessage("Employee data is required.")
            .SetValidator(employeeValidator);

        }

    }
}
