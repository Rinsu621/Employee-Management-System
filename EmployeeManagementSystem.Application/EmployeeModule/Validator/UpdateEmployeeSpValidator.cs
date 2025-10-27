using EmployeeManagementSystem.Application.EmployeeModule.Commands;
using EmployeeManagementSystem.Application.Interface;
using EmployeeManagementSystem.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Application.EmployeeModule.Validator
{
    public class UpdateEmployeeSpValidator : AbstractValidator<UpdateEmployeeSpCommand>
    {
        private readonly IAppDbContext dbContext;
        public UpdateEmployeeSpValidator(IAppDbContext _dbContext, EmployeeDtoValidator employeeValidator)
        {
            dbContext = _dbContext;
            RuleFor(x => x.Id)
                 .NotEmpty().WithMessage("Employee ID is required.")
                 .MustAsync(async (id, cancellation) =>
                 {
                     return await dbContext.Employees.AnyAsync(e => e.Id == id, cancellation);
                 }).WithMessage("Employee with the specified ID does not exist.");
            
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
                .Matches(@"^98\d{8}$").WithMessage("Phone number must be 10 digits and must start with 98.");
        }
    }
    }
