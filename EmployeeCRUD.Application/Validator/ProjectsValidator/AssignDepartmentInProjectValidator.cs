using EmployeeCRUD.Application.Command.Projects;
using EmployeeCRUD.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Validator.ProjectsValidator
{
    public class AssignDepartmentInProjectValidator : AbstractValidator<AssignDepartmentInProjectCommand>
    {
        private readonly AppDbContext dbContext;
        public AssignDepartmentInProjectValidator(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("Project ID is required.")
                .MustAsync(async (Id, cancellation) =>
                {
                    return await dbContext.Project.AnyAsync(e => e.Id == Id, cancellation);
                }).WithMessage("Project with the specified ID does not exist.");

            RuleFor(x => x.DepartmentId)
                .NotEmpty().WithMessage("Department ID is required.")
                .MustAsync(async (Id, cancellation) =>
                {
                    return await dbContext.Departments.AnyAsync(e => e.Id == Id, cancellation);
                }).WithMessage("Department with the specified ID does not exist.");
        }
    }
}
