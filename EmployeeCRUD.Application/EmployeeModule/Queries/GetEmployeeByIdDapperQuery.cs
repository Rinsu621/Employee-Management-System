using Dapper;
using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Interface;
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
            private readonly IDbConnectionService connection;
            public GetEmployeeByIdDapperHandler(IDbConnectionService _connection)
            {
                connection = _connection;
            }
            public async Task<EmployeeResponseDto> Handle(GetEmployeeByIdDapperQuery request, CancellationToken cancellationToken)
            {
            using var db= connection.CreateConnection();
            var result = await db.QuerySingleOrDefaultAsync<EmployeeResponseDto>(
                     "GetAllEmployeeById",
                     new { Id = request.Id },
                     commandType: CommandType.StoredProcedure);
                
                return result;
            }
    }
   
}
