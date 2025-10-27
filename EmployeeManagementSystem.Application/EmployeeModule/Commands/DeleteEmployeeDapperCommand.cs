using Dapper;
using EmployeeManagementSystem.Application.Configuration;
using EmployeeManagementSystem.Application.EmployeeModule.Dtos;
using EmployeeManagementSystem.Application.Interface;
using MediatR;
using System.Data;

namespace EmployeeManagementSystem.Application.EmployeeModule.Commands
{
    public record DeleteEmployeeDapperCommand(Guid Id) : IRequest<DeleteEmployeeResponse>;
    public class  DeleteEmployeeDapperHandler: IRequestHandler<DeleteEmployeeDapperCommand, DeleteEmployeeResponse>
    {
        private readonly IDbConnectionService connection;
 
        public DeleteEmployeeDapperHandler(IDbConnectionService _connection)
        {
            connection = _connection;

        }
        public async Task<DeleteEmployeeResponse> Handle(DeleteEmployeeDapperCommand request, CancellationToken cancellationToken)
        {
            using var db = connection.CreateConnection();
          var result= await db.ExecuteAsync("DeleteEmployee",
                new { Id = request.Id },
                commandType: CommandType.StoredProcedure);
         
            return new DeleteEmployeeResponse
            {
                Success = true,
                Message = "Employee deleted successfully."
            };
        }

    }
    
}
