using EmployeeCRUD.Application.ProjectModule.Commands;
using EmployeeCRUD.Domain.Interface;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCRUD.Application.ProjectModule.Validator
{
    public class PatchProjectDapperValidator : AbstractValidator<PatchProjectDapperCommand>
    {
        private readonly IAppDbContext dbContext;

        public PatchProjectDapperValidator(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required.")
                .MustAsync(async (id, cancellationToken) =>
                {
                    var exists = await dbContext.Projects.FindAsync(new object[] { id }, cancellationToken);
                    return exists != null;
                }).WithMessage("Project with the specified Id does not exist.");

            RuleFor(x => x.ProjectName)
     .MaximumLength(200).WithMessage("Project name cannot exceed 200 characters.")
     .When(x => !string.IsNullOrEmpty(x.ProjectName));

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.")
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Budget)
                .GreaterThanOrEqualTo(0).WithMessage("Budget must be zero or greater.")
                .When(x => x.Budget.HasValue);

            RuleFor(x => x.TeamMembersIds)
                .Must(list => list == null || list.All(id => id != Guid.Empty))
                .WithMessage("Team member IDs cannot be empty GUIDs.")
                .When(x => x.TeamMembersIds != null);

            RuleForEach(x => x.TeamMembersIds)
                .NotEmpty().WithMessage("Team member ID cannot be empty.")
                .When(x => x.TeamMembersIds != null);

            RuleFor(x => x.TeamMembersIds)
                .MustAsync(AllEmployeesExist)
                .WithMessage("One or more team members do not exist.")
                .When(x => x.TeamMembersIds != null && x.TeamMembersIds.Any());
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
