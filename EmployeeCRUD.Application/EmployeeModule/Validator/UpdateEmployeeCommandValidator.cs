using EmployeeCRUD.Application.EmployeeModule.Commands;
using EmployeeCRUD.Domain.Interface;
using EmployeeCRUD.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCRUD.Application.EmployeeModule.Validator
{
    public class UpdateEmployeeCommandValidator:AbstractValidator<UpdateEmployeeCommand>
    {
        private readonly IAppDbContext dbContext;
        public UpdateEmployeeCommandValidator(IAppDbContext _dbContext, EmployeeDtoValidator employeeValidator)
        {
            dbContext = _dbContext;
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Employee ID is required.")
                .MustAsync(async (id, cancellation) =>
                {
                    return await dbContext.Employees.AnyAsync(e => e.Id == id, cancellation);
                }).WithMessage("Employee with the specified ID does not exist.");

            RuleFor(x => x.Employee)
            .NotNull().WithMessage("Employee data is required.")
            .SetValidator(employeeValidator);
        }

    }
}
