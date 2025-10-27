using EmployeeManagementSystem.Application.EmployeeModule.Dtos;
using EmployeeManagementSystem.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Application.EmployeeModule.Validator
{
    public class EmployeeDtoValidator : AbstractValidator<EmployeeDto>
    {
        private readonly Application.Interface.IAppDbContext dbContext;
        public EmployeeDtoValidator(Application.Interface.IAppDbContext _dbContext)
            
        {
            dbContext = _dbContext;
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


        }
    }
}
