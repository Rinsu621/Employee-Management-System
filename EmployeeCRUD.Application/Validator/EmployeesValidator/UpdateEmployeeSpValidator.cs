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
    public class UpdateEmployeeSpValidator : AbstractValidator<UpdateEmployeeCommand>
    {
        private readonly AppDbContext dbContext;
        public UpdateEmployeeSpValidator(AppDbContext _dbContext, EmployeeDtoValidator employeeValidator)
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
