using EmployeeCRUD.Application.EmployeeModule.Commands;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCRUD.Application.EmployeeModule.Validator
{
    public class PatchEmployeeValidator : AbstractValidator<PatchEmployeeCommand>
    {
        private readonly IAppDbContext dbContext;
        public PatchEmployeeValidator(IAppDbContext _dbContext)
        {
           dbContext = _dbContext;
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Employee ID is required.")
                .MustAsync(async (id, cancellation) =>
                {
                    return await dbContext.Employees.AnyAsync(e => e.Id == id, cancellation);
                }).WithMessage("Employee with the specified ID does not exist.");
            

            RuleFor(x => x.EmpName)
             .NotEmpty().WithMessage("Employee name is required.")
             .MaximumLength(100).WithMessage("Employee name must not exceed 100 characters.")
             .When(x => !string.IsNullOrEmpty(x.EmpName)); // Only validate if EmpName is provided

            RuleFor(x => x.Email)
               .EmailAddress().WithMessage("Invalid email format.")
               .When(x => !string.IsNullOrEmpty(x.Email))
                 .MustAsync(async (email, cancellation) =>
                !await dbContext.Employees.AnyAsync(x => x.Email == email, cancellation))
                .WithMessage("Email already exists.");

            RuleFor(e => e.Phone)
               .NotEmpty().WithMessage("Phone number is required.")
               .Matches(@"^98\d{8}$").WithMessage("Phone number must be 10 digits and must start with 98.")
              .When(x => !string.IsNullOrEmpty(x.Phone));


        }
    }
}
