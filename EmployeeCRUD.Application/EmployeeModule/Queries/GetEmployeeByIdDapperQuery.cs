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
    public record GetEmployeeByIdDapperQuery(Guid Id):IRequest<EmployeeResponseDto>;
        public class GetEmployeeByIdDapperHandler : IRequestHandler<GetEmployeeByIdDapperQuery, EmployeeResponseDto>
        {
            private readonly IDbConnection connection;
            public GetEmployeeByIdDapperHandler(IDbConnection _connection)
            {
                connection = _connection;
            }
            public async Task<EmployeeResponseDto> Handle(GetEmployeeByIdDapperQuery request, CancellationToken cancellationToken)
            {
                var result = await connection.QuerySingleOrDefaultAsync<EmployeeResponseDto>(
                     "GetEmployeeById",
                     new { Id = request.Id },
                     commandType: CommandType.StoredProcedure);
                
                return result;
            }
    }
   
}
