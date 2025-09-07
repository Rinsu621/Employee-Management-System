using EmployeeCRUD.Application.Dtos.Departments;
using EmployeeCRUD.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Validator
{
    public class DepartmentDtoValidator: AbstractValidator<DepartmentCreateDto>
    {
        private readonly AppDbContext dbContext;

        public DepartmentDtoValidator(AppDbContext _dbContext)
        {
            dbContext=_dbContext;
            RuleFor(d => d.DeptName)
        .NotEmpty().WithMessage("Department name is required.")
        .MaximumLength(100).WithMessage("Department name must not exceed 100 characters.")
        .MustAsync(async (deptName, cancellationToken) =>
            !await dbContext.Departments.AnyAsync(x => x.DeptName == deptName, cancellationToken)
        )
        .WithMessage("Department Name already exist.");
        }

    }
}
