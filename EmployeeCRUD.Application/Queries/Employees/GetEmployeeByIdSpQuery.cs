using EmployeeCRUD.Application.Dtos.Employees;
using EmployeeCRUD.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Queries.Employees
{
    public record GetEmployeeByIdSpQuery(Guid EmployeeId) : IRequest<EmployeeResponseDto>;

    public class GetEmployeeByIdSpHandler : IRequestHandler<GetEmployeeByIdSpQuery, EmployeeResponseDto>
    {
        private readonly AppDbContext dbContext;
        public GetEmployeeByIdSpHandler(AppDbContext _dbContext)
        {
            dbContext = _dbContext;
        }

        public async Task<EmployeeResponseDto> Handle(GetEmployeeByIdSpQuery request, CancellationToken cancellationToken)
        {
            EmployeeResponseDto employee = null;
            var connection = dbContext.Database.GetDbConnection();
            await using (connection)
            {
                await connection.OpenAsync(cancellationToken);
                await using var command = connection.CreateCommand();
                command.CommandText = "GetEmployeeById";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                var param = command.CreateParameter();
                param.ParameterName = "@Id";
                param.Value = request.EmployeeId;
                command.Parameters.Add(param);
                await using var reader = await command.ExecuteReaderAsync(cancellationToken);
                if (await reader.ReadAsync(cancellationToken))
                {
                    employee = new EmployeeResponseDto
                    {
                        Id = reader.GetGuid(reader.GetOrdinal("Id")),
                        Name = reader.GetString(reader.GetOrdinal("Name")),
                        Email = reader.GetString(reader.GetOrdinal("Email")),
                        Phone = reader.GetString(reader.GetOrdinal("Phone")),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                    };
                }
            }
            if (employee == null)
            {
                throw new KeyNotFoundException($"Employee with Id '{request.EmployeeId}' not found.");
            }
            return employee;
            //      var employees = await dbContext.Employees
            //.FromSqlInterpolated($"EXEC GetEmployeeById @EmployeeId={request.EmployeeId}")
            //.ToListAsync(cancellationToken); // materialize first

            //      var result = employees.Select(e => new EmployeeResponseDto
            //      {
            //          Id = e.Id,
            //          Name = e.EmpName,
            //          Email = e.Email,
            //          Phone = e.Phone,
            //          CreatedAt = e.CreatedAt
            //      }).FirstOrDefault();

            //      return result ?? throw new KeyNotFoundException($"Employee with Id '{request.EmployeeId}' not found.");
            //  }
        }
    }
   
}
