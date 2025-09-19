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
    public record AddEmployeeDapperCommand(string EmpName, string Email, string Phone): IRequest<EmployeeResponseDto>;

    public class AddEmployeeDapperHandler : IRequestHandler<AddEmployeeDapperCommand, EmployeeResponseDto>
    {
        private readonly IDbConnection connection;
        public AddEmployeeDapperHandler(IDbConnection _connection)
        {
            connection = _connection;
        }
        public async Task<EmployeeResponseDto> Handle(AddEmployeeDapperCommand request, CancellationToken cancellationToken)
        {
           

            var result = await connection.QuerySingleAsync<EmployeeResponseDto>("AddEmployee",
                new { request.EmpName, request.Email, request.Phone },
                commandType: CommandType.StoredProcedure);

            Guard.Against.Null(result, nameof(result), "Failed to add employee");
            return result;

        }
    }
}
