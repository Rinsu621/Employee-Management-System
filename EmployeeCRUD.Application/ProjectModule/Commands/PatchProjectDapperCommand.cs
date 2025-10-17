using Ardalis.GuardClauses;
using Dapper;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Application.ProjectModule.Dtos;
using EmployeeCRUD.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.ProjectModule.Commands
{
    public record PatchProjectDapperCommand(Guid Id, string? ProjectName, string? Description, DateTime? EndDate, decimal? Budget, string? Status, string? ClientName, Guid? ProjectManagerId, List<Guid>? TeamMembersIds) : IRequest<ProjectDto>;

    public class PatchProjectDapperHandler:IRequestHandler<PatchProjectDapperCommand, ProjectDto>
    {
        private readonly IEmployeeDbConnection connection;
        public PatchProjectDapperHandler(IEmployeeDbConnection _connection)
        {
            connection = _connection;
        }

        public async Task<ProjectDto> Handle(PatchProjectDapperCommand request, CancellationToken cancellationToken)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Id", request.Id);
            parameters.Add("@ProjectName", request.ProjectName);
            parameters.Add("@Description", request.Description);
            parameters.Add("@ClientName", request.ClientName);
            parameters.Add("@Status", request.Status);
            parameters.Add("@Budget", request.Budget);
            parameters.Add("@EndDate", request.EndDate);
            parameters.Add("@ProjectManagerId", request.ProjectManagerId);
            parameters.Add("@TeamMembersIds", request.TeamMembersIds != null ? string.Join(',', request.TeamMembersIds) : null);

            using var db=connection.CreateConnection();
            var result = await db.QueryFirstOrDefaultAsync<dynamic>(
                "PatchProject",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            if (result == null)
                throw new KeyNotFoundException($"Project with Id '{request.Id}' not found.");
            //Guard.Against.Null(result, nameof(result), $"Project with Id '{request.Id}' not found.");

            var teamMembers = (result.TeamMembers as string)?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>();

            return new ProjectDto
            {
                Id = result.Id,
                ProjectName = result.ProjectName,
                Description = result.Description,
                EndDate = result.EndDate,
                Budget = result.Budget,
                Status = result.Status,
                ClientName = result.ClientName,
                ProjectManagerName = result.ProjectManagerName,
                TeamMember = teamMembers
            };


        }
    }

}
