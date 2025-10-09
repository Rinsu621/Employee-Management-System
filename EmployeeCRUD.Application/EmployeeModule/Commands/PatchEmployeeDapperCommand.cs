using Dapper;
using EmployeeCRUD.Application.EmployeeModule.Dtos;
using MediatR;
using System.Data;


namespace EmployeeCRUD.Application.EmployeeModule.Commands
{
    public record PatchEmployeeDapperCommand(Guid Id, string EmpName, string Email, string Phone): IRequest<EmployeeUpdateResponse>;
    
    public class PatchEmployeeDapperHandler : IRequestHandler<PatchEmployeeDapperCommand, EmployeeUpdateResponse>
    {
        private readonly IDbConnection connection;
        public PatchEmployeeDapperHandler(IDbConnection _connection)
        {
           connection = _connection;
        }
        public async Task<EmployeeUpdateResponse> Handle(PatchEmployeeDapperCommand request, CancellationToken cancellationToken)
        {
            var result= await connection.QuerySingleAsync<EmployeeUpdateResponse>("PatchEmployee",
                new { Id = request.Id, request.EmpName, request.Email, request.Phone },
                commandType: CommandType.StoredProcedure);
            return result;
        }
    }
}
