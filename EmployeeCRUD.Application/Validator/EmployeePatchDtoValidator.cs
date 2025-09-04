using EmployeeCRUD.Application.Dtos.Employees;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Validator
{
    public class EmployeePatchDtoValidator : AbstractValidator<EmployeePatchDto>
    {
        public EmployeePatchDtoValidator()
        {
            RuleFor(e => e.EmpName)
                .NotEmpty().WithMessage("Employee name is required.")
                .MaximumLength(100).WithMessage("Employee name must not exceed 100 characters.")
                .When(e => e.EmpName != null);

            RuleFor(e => e.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
              .When(e => e.Email != null);

            RuleFor(e => e.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^98\d{8}$").WithMessage("Phone number must be 10 digits and must start with 98.")
                .When(e => e.Phone != null);

        }
    }
}
