using Ardalis.GuardClauses;
using Dapper;
using EmployeeCRUD.Application.EmployeeModule.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Commands
{
    public record DeleteEmployeeDapperCommand(Guid Id) : IRequest<DeleteEmployeeResponse>;
    public class  DeleteEmployeeDapperHandler: IRequestHandler<DeleteEmployeeDapperCommand, DeleteEmployeeResponse>
    {
        private readonly IDbConnection connection;
        public DeleteEmployeeDapperHandler(IDbConnection _connection)
        {
            connection = _connection;
        }
        public async Task<DeleteEmployeeResponse> Handle(DeleteEmployeeDapperCommand request, CancellationToken cancellationToken)
        {
          var result= await connection.ExecuteAsync("DeleteEmployee",
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
