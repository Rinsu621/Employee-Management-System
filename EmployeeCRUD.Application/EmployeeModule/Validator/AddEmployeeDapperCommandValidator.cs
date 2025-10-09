using EmployeeCRUD.Application.EmployeeModule.Commands;
using EmployeeCRUD.Application.Interface;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCRUD.Application.EmployeeModule.Validator
{
    public class AddEmployeeDapperCommandValidator:AbstractValidator<AddEmployeeDapperCommand>
    {
        private readonly IAppDbContext dbContext;
        private readonly RoleManager<IdentityRole> roleManager;
        public AddEmployeeDapperCommandValidator(IAppDbContext _dbContext, RoleManager<IdentityRole> _roleManager)
        {
            dbContext = _dbContext;
            roleManager = _roleManager;

            RuleFor(e => e.EmpName)
               .NotEmpty().WithMessage("Employee name is required.")
               .MaximumLength(100).WithMessage("Employee name must not exceed 100 characters.");

            RuleFor(e => e.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MustAsync(async (email, cancellation) =>
                !await dbContext.Employees.AnyAsync(x => x.Email == email, cancellation))
            .WithMessage("Email already exists.");

            RuleFor(e => e.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^98\d{8}$").WithMessage("Phone number must be 10 digits and must start with 98.")
            .MustAsync(async (phone, cancellation) =>
            !await dbContext.Employees.AnyAsync(x => x.Phone == phone, cancellation))
            .WithMessage("Number already exists.");

            RuleFor(e=>e.Role)
                .NotEmpty().WithMessage("Role is required")
                .MustAsync(async (role, cancellation) =>
                    await roleManager.RoleExistsAsync(role))
                .WithMessage("Role does not exist in the system.");

        }

    }
}
