using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Application.ProjectModule.Dtos;
using EmployeeCRUD.Infrastructure.Data;
using EmployeeCRUD.Infrastructure.Data.keyless;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.ProjectModule.Commands
{
    public record AssignTeamMemberCommand(Guid ProjectId, Guid EmployeeId) : IRequest<TeamMemberAssignmentDto>;

    public class AssignTeamMemberHandler:IRequestHandler<AssignTeamMemberCommand, TeamMemberAssignmentDto>
    {
        private readonly IAppDbContext dbContext;
        public AssignTeamMemberHandler(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<TeamMemberAssignmentDto> Handle(AssignTeamMemberCommand request, CancellationToken cancellationToken)
        {
            var project = await dbContext.Projects.FirstOrDefaultAsync(p => p.Id == request.ProjectId, cancellationToken);
            var employee = await dbContext.Employees.FirstOrDefaultAsync(e => e.Id == request.EmployeeId, cancellationToken);

            project.TeamMember.Add(employee);
            dbContext.Projects.Update(project);
            await dbContext.SaveChangesAsync(cancellationToken);
            var teamMembers = project.TeamMember.Select(e => e.EmpName).ToList();

            return new TeamMemberAssignmentDto
            {
                ProjectId = project.Id,
                ProjectName = project.ProjectName,
                Status = project.Status,
                TeamMembers = teamMembers
            };

        }
    }
}
