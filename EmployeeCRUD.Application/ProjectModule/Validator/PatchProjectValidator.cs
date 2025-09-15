using EmployeeCRUD.Application.ProjectModule.Commands;
using EmployeeCRUD.Domain.Interface;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.ProjectModule.Validator
{
    public class PatchProjectValidator : AbstractValidator<PatchProjectCommand>
    {
        private readonly IAppDbContext dbContext;

        public PatchProjectValidator(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;

            // Project ID is always required
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Project ID is required.");

            // Validate ProjectName only if provided
            RuleFor(x => x.project.ProjectName)
                .NotEmpty().WithMessage("Project name cannot be empty.")
                .MaximumLength(200).WithMessage("Project name cannot exceed 200 characters.")
                .When(x => !string.IsNullOrEmpty(x.project.ProjectName));

            // Validate Description only if provided
            RuleFor(x => x.project.Description)
                .NotEmpty().WithMessage("Description cannot be empty.")
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.")
                .When(x => !string.IsNullOrEmpty(x.project.Description));

            // Validate Budget only if provided
            RuleFor(x => x.project.Budget)
                .GreaterThanOrEqualTo(0).WithMessage("Budget must be zero or greater.")
                .When(x => x.project.Budget.HasValue);

            // Validate TeamMemberIds list only if provided
            RuleFor(x => x.project.TeamMembersIds)
                .Must(list => list == null || list.All(id => id != Guid.Empty))
                .WithMessage("Team member IDs cannot be empty GUIDs.")
                .When(x => x.project.TeamMembersIds != null);

            RuleForEach(x => x.project.TeamMembersIds)
                .NotEmpty().WithMessage("Team member ID cannot be empty.")
                .When(x => x.project.TeamMembersIds != null);

            // Validate if all employee IDs exist in DB
            RuleFor(x => x.project.TeamMembersIds)
                .MustAsync(AllEmployeesExist)
                .WithMessage("One or more team members do not exist.")
                .When(x => x.project.TeamMembersIds != null && x.project.TeamMembersIds.Any());
        }

        private async Task<bool> AllEmployeesExist(List<Guid> ids, CancellationToken cancellationToken)
        {
            if (ids == null || !ids.Any())
                return true;

            var existingIds = await dbContext.Employees
                .Where(e => ids.Contains(e.Id))
                .Select(e => e.Id)
                .ToListAsync(cancellationToken);

            return ids.All(id => existingIds.Contains(id));
        }
    }
}
