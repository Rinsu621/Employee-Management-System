using Dapper;
using EmployeeCRUD.Application.Configuration;
using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Interface;
using MediatR;
using System.Data;

namespace EmployeeCRUD.Application.EmployeeModule.Commands
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
