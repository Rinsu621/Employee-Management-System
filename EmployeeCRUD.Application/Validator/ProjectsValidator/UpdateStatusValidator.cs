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
    public class UpdateStatusValidator:AbstractValidator<UpdateStatusCommand>
    {
        private readonly AppDbContext dbContext;
        public UpdateStatusValidator(AppDbContext _dbContext)
        {
            dbContext = _dbContext;

            RuleFor(x => x.id)
                .NotEmpty().WithMessage("Project ID is required.")
                .MustAsync(async (Id, cancellation) =>
                {
                    return await dbContext.Project.AnyAsync(e => e.Id == Id, cancellation);
                }).WithMessage("Project with the specified ID does not exist.");
        }

    }
}
