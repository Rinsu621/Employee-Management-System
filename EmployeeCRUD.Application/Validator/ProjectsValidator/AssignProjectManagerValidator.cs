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
  
    public class AssignProjectManagerValidator : AbstractValidator<AssignProjectManagerCommand>
    {
        private readonly AppDbContext dbContext;
        public AssignProjectManagerValidator(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("Project ID is required.")
                .MustAsync(async (Id, cancellation) =>
                {
                    return await dbContext.Project.AnyAsync(e => e.Id == Id, cancellation);
                }).WithMessage("Project with the specified ID does not exist.");

            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("Employee ID is required.")
                .MustAsync(async (Id, cancellation) =>
                {
                    return await dbContext.Employees.AnyAsync(e => e.Id == Id, cancellation);
                }).WithMessage("Employee with the specified ID does not exist.");
        }
    }
}
