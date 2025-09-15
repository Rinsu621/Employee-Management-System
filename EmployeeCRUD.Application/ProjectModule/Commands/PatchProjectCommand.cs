using Ardalis.GuardClauses;
using EmployeeCRUD.Application.ProjectModule.Dtos;
using EmployeeCRUD.Domain.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.ProjectModule.Commands
{
    public record PatchProjectCommand(Guid Id, UpdateProjectDto project):IRequest<ProjectDto>;
    public class PatchProjectHandler : IRequestHandler<PatchProjectCommand, ProjectDto>
    {
        private readonly IAppDbContext dbContext;
        public PatchProjectHandler(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task<ProjectDto> Handle(PatchProjectCommand request, CancellationToken cancellationToken)
        {
            var project =await dbContext.Projects.Include(p=>p.TeamMember).SingleOrDefaultAsync(predicate: p => p.Id == request.Id, cancellationToken);
            Guard.Against.Null(project, nameof(project), $"Project with Id '{request.Id}' not found.");

            project.ProjectName = request.project.ProjectName ?? project.ProjectName;
            project.Description = request.project.Description ?? project.Description;
            project.ClientName = request.project.ClientName ?? project.ClientName;
            project.Status = request.project.Status ?? project.Status;

            project.Budget = request.project.Budget ?? project.Budget;
            project.EndDate = request.project.EndDate ?? project.EndDate;
            project.ProjectManagerId = request.project.ProjectManagerId ?? project.ProjectManagerId;
            project.IsArchived = request.project.IsArchived ?? project.IsArchived;


            if (request.project.TeamMemberIds?.Any() == true)
            {
                var employees = await dbContext.Employees
                    .Where(e => request.project.TeamMemberIds.Contains(e.Id))
                    .ToListAsync(cancellationToken);

                foreach (var emp in employees)
                {
                    if (!project.TeamMember.Any(e => e.Id == emp.Id))
                        project.TeamMember.Add(emp);
                }
            }
            await dbContext.SaveChangesAsync(cancellationToken);

            return new ProjectDto
            {
                Id = project.Id,
                ProjectName = project.ProjectName,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Budget = project.Budget,
                Status = project.Status,
                ClientName = project.ClientName,
                ProjectManagerName= project.ProjectManager?.EmpName,
                TeamMember = project.TeamMember.Select(e => e.EmpName!).ToList() ?? new List<string>()
            };





        }
    }
}
