using Dapper;
using EmployeeCRUD.Application.EmployeeModule.Dtos;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Queries
{
    public record GetAllEmployeeDapperQuery():IRequest<IEnumerable<EmployeeResponseDto>>;

    public class GetAllEmployeeDapperHandler : IRequestHandler<GetAllEmployeeDapperQuery, IEnumerable<EmployeeResponseDto>>
    {
        private readonly IDbConnection connection;
        public GetAllEmployeeDapperHandler(IDbConnection _connection)
        {
            connection = _connection;
        }
        public async Task<IEnumerable<EmployeeResponseDto>> Handle(GetAllEmployeeDapperQuery request, CancellationToken cancellationToken)
        {
           var result= await connection.QueryAsync<EmployeeResponseDto>(
                "GetAllEmployees",
                commandType: CommandType.StoredProcedure);
            return result;
        }
    }

}
