using Dapper;
using EmployeeManagementSystem.Application.Configuration;
using EmployeeManagementSystem.Application.EmployeeModule.Dtos;
using EmployeeManagementSystem.Application.Interface;
using MediatR;
using System.Data;


namespace EmployeeManagementSystem.Application.EmployeeModule.Commands
{
    public record PatchEmployeeDapperCommand(Guid Id, string EmpName, string Email, string Phone): IRequest<EmployeeUpdateResponse>;
    
    public class PatchEmployeeDapperHandler : IRequestHandler<PatchEmployeeDapperCommand, EmployeeUpdateResponse>
    {
        private readonly IDbConnectionService connection;

        public PatchEmployeeDapperHandler(IDbConnectionService _connection)
        {
           connection = _connection;
        }
        public async Task<EmployeeUpdateResponse> Handle(PatchEmployeeDapperCommand request, CancellationToken cancellationToken)
        {
            using var conn = connection.CreateConnection();
            var result= await conn.QuerySingleAsync<EmployeeUpdateResponse>("PatchEmployee",
                new { Id = request.Id, request.EmpName, request.Email, request.Phone },
                commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}
