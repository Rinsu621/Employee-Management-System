using EmployeeCRUD.Application.EmployeeModule.Dtos;
using MediatR;
using System.Data;
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




            var employee = await db.QueryFirstOrDefaultAsync<EmployeeUpdateResponse>(
               "UpdateEmployee",
               parameters,
               commandType: CommandType.StoredProcedure
           );

            if (employee != null)
                employee.UpdatedAt = DateTime.UtcNow;

            return employee!;
        }
    }
}
