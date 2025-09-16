using EmployeeCRUD.Application.EmployeeModule.Commands;
using EmployeeCRUD.Application.Interface;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Validator
{
   
    public class DeleteEmployeeDapperValidator : AbstractValidator<DeleteEmployeeDapperCommand>
    {
        private readonly IAppDbContext dbContext;
        public DeleteEmployeeDapperValidator(IAppDbContext _dbContext)
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
