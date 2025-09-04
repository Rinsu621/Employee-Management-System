using EmployeeCRUD.Application.Dtos.Employees;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Validator
{
    public class EmployeeDtoValidator : AbstractValidator<EmployeeDto>
    {
        public EmployeeDtoValidator()
        {
            RuleFor(e => e.EmpName)
                .NotEmpty().WithMessage("Employee name is required.")
                .MaximumLength(100).WithMessage("Employee name must not exceed 100 characters.");

            RuleFor(e => e.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.");

            RuleFor(e => e.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^98\d{8}$").WithMessage("Phone number must be 10 digits and must start with 98.");

        }
    }
}
