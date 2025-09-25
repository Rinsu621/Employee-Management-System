using EmployeeCRUD.Application.EmployeeModule.Dtos;
using MediatR;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace EmployeeCRUD.Application.EmployeeModule.Commands
{
    public record UpdateEmployeeWithDapperCommand(
        Guid Id,
        string EmpName,
        string Email,
        string Phone,
        Guid? DepartmentId,
        string Role
    ) : IRequest<EmployeeUpdateResponse>;

    public class UpdateEmployeeWithDapperHandler : IRequestHandler<UpdateEmployeeWithDapperCommand, EmployeeUpdateResponse>
    {
        private readonly IConfiguration _configuration;

        public UpdateEmployeeWithDapperHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<EmployeeUpdateResponse> Handle(UpdateEmployeeWithDapperCommand request, CancellationToken cancellationToken)
        {
            using var db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            await db.OpenAsync(cancellationToken);

            var parameters = new DynamicParameters();
            parameters.Add("@EmployeeId", request.Id, DbType.Guid);
            parameters.Add("@EmpName", request.EmpName, DbType.String);
            parameters.Add("@Email", request.Email, DbType.String);
            parameters.Add("@Phone", request.Phone, DbType.String);
            parameters.Add("@DepartmentId", request.DepartmentId, DbType.Guid);
            parameters.Add("@Role", request.Role, DbType.String);

            await db.ExecuteAsync("UpdateEmployee", parameters, commandType: CommandType.StoredProcedure);

            // Optionally, return updated employee info
            var employee = await db.QueryFirstOrDefaultAsync<EmployeeUpdateResponse>(
                "SELECT e.Id, e.EmpName AS Name, e.Email, e.Phone, d.DeptName AS DepartmentName " +
                "FROM Employees e " +
                "LEFT JOIN Departments d ON e.DepartmentId = d.Id " +
                "WHERE e.Id = @Id",
                new { Id = request.Id }
            );

            employee.Role = request.Role;
            employee.UpdatedAt = DateTime.UtcNow;

            return employee;
        }
    }
}
