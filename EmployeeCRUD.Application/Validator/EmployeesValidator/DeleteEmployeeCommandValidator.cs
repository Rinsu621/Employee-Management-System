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
    public class DeleteEmployeeCommandValidator: AbstractValidator<DeleteEmployeeCommand>
    {
        private readonly AppDbContext dbContext;
        public DeleteEmployeeCommandValidator(AppDbContext _dbContext)
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
