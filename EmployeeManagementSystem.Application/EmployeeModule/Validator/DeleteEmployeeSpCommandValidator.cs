using EmployeeManagementSystem.Application.EmployeeModule.Commands;
using EmployeeManagementSystem.Application.Interface;
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
    public class DeleteEmployeeSpCommandValidator:AbstractValidator<DeleteEmployeeSpCommand>
    {
        private readonly IAppDbContext dbContext;
        public DeleteEmployeeSpCommandValidator(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Employee ID is required.")
                .MustAsync(async (Id, cancellation) =>
                {
                    return await dbContext.Employees.AnyAsync(e => e.Id == Id, cancellation);
                }).WithMessage("Employee with the specified ID does not exist.");
        }
    }
}
