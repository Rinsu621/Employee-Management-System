using Ardalis.GuardClauses;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Domain.keyless;
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
    public record AssignTeamMemberSpCommand(Guid ProjectId, Guid EmployeeId) : IRequest<TeamMemberAssignmentResponse>;
    
    public class AssignTeamMemberSpHandler : IRequestHandler<AssignTeamMemberSpCommand, TeamMemberAssignmentResponse>
    {
        private readonly IAppDbContext dbContext;
        public AssignTeamMemberSpHandler(IAppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<TeamMemberAssignmentResponse> Handle(AssignTeamMemberSpCommand request, CancellationToken cancellationToken)
        {
            var rows = await dbContext.TeamMemberAssignmentRows
            .FromSqlInterpolated($"EXEC AssignTeamMember {request.ProjectId}, {request.EmployeeId}")
            .AsNoTracking()
            .ToListAsync(cancellationToken);

            var response = new TeamMemberAssignmentResponse
            {
                ProjectId = rows.First().ProjectId,
                ProjectName = rows.First().ProjectName,
                Status = rows.First().Status,
                TeamMembers = rows.Select(r => r.TeamMember).ToList()  // List<string>
            };
            return response;
        }
    }
}
