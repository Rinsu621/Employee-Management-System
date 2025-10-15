using Dapper;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Application.ProjectModule.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.ProjectModule.Commands
{
    public record AddProjectDapperCommand(string ProjectName, string Description, DateTime StartDate, DateTime? EndDate, decimal Budget, string Status, string? ClientName):IRequest<ProjectDto>;
    
    public class AddProjectDapperHandler : IRequestHandler<AddProjectDapperCommand, ProjectDto>
    {
        private readonly IEmployeeDbConnection connection;
        public AddProjectDapperHandler(IEmployeeDbConnection _connection)
        {
            connection = _connection;
        }
        public async Task<ProjectDto> Handle(AddProjectDapperCommand request, CancellationToken cancellationToken)
        {
            using var db = connection.CreateConnection();
            var result = await db.QuerySingleAsync<ProjectDto>("AddProject",
                new { request.ProjectName, request.Description, request.StartDate, request.EndDate, request.Budget, request.Status, request.ClientName },
                commandType: CommandType.StoredProcedure);

            return result;
        }
    }
}
