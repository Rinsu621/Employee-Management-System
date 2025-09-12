using EmployeeCRUD.Application.ProjectModule.Commands;
using EmployeeCRUD.Domain.Interface;
using EmployeeCRUD.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.ProjectModule.Validator
{
    public class AssignTeamMemberValidator:AbstractValidator<AssignTeamMemberCommand>
    {
        private readonly IAppDbContext dbContext;
        public AssignTeamMemberValidator(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
            RuleFor(x => x.ProjectId)
                .NotEmpty().WithMessage("Project ID is required.")
                .MustAsync(async (Id, cancellation) =>
                {
                    return await dbContext.Projects.AnyAsync(e => e.Id == Id, cancellation);
                }).WithMessage("Project with the specified ID does not exist.");

            RuleFor(x => x.EmployeeId)
                .NotEmpty().WithMessage("Employee ID is required.")
                .MustAsync(async (Id, cancellation) =>
                {
                    return await dbContext.Employees.AnyAsync(e => e.Id == Id, cancellation);
                }).WithMessage("Employee with the specified ID does not exist.");

            RuleFor(x => x)
               .MustAsync(async (command, cancellation) =>
               {
                   var project = await dbContext.Projects
                       .Include(p => p.TeamMember)  
                       .FirstOrDefaultAsync(p => p.Id == command.ProjectId, cancellation);

                   if (project != null)
                   {
                       return !project.TeamMember.Any(e => e.Id == command.EmployeeId);  
                   }

                   return true;  
               })
               .WithMessage("The employee is already assigned to this project.");
        }
    }
}
