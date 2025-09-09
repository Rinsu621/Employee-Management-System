using EmployeeCRUD.Application.Command.Employees;
using EmployeeCRUD.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Validator.EmployeesValidator
{
    public class PatchEmployeeSpValidator : AbstractValidator<PatchEmployeeSpCommand>
    {
        private readonly AppDbContext dbContext;
        public PatchEmployeeSpValidator(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Employee ID is required.")
                .MustAsync(async (id, cancellation) =>
                {
                    return await dbContext.Employees.AnyAsync(e => e.Id == id, cancellation);
                }).WithMessage("Employee with the specified ID does not exist.");
            RuleFor(x => x.employee)
                .NotNull().WithMessage("Employee data is required.");

            RuleFor(x => x.employee.EmpName)
             .NotEmpty().WithMessage("Employee name is required.")
             .MaximumLength(100).WithMessage("Employee name must not exceed 100 characters.")
             .When(x => !string.IsNullOrEmpty(x.employee.EmpName)); // Only validate if EmpName is provided

            RuleFor(x => x.employee.Email)
               .EmailAddress().WithMessage("Invalid email format.")
               .When(x => !string.IsNullOrEmpty(x.employee.Email))
                 .MustAsync(async (email, cancellation) =>
                !await dbContext.Employees.AnyAsync(x => x.Email == email, cancellation))
                .WithMessage("Email already exists.");

            RuleFor(e => e.employee.Phone)
               .NotEmpty().WithMessage("Phone number is required.")
               .Matches(@"^98\d{8}$").WithMessage("Phone number must be 10 digits and must start with 98.")
              .When(x => !string.IsNullOrEmpty(x.employee.Phone));


        }
    }
}
