using EmployeeManagementSystem.Application.EmployeeModule.Dtos;
using MediatR;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using EmployeeManagementSystem.Application.Interface;
using DocumentFormat.OpenXml.Drawing;

namespace EmployeeManagementSystem.Application.EmployeeModule.Commands
{
    public record UpdateEmployeeWithDapperCommand(
        Guid Id,
        string EmpName,
        string Email,
        string Phone,
        Guid? DepartmentId,
        string Role, 
        string Position
    ) : IRequest<EmployeeUpdateResponse>;

    public class UpdateEmployeeWithDapperHandler : IRequestHandler<UpdateEmployeeWithDapperCommand, EmployeeUpdateResponse>
    {
        private readonly IConfiguration _configuration;
        private readonly IDbConnectionService connection;

        public UpdateEmployeeWithDapperHandler(IConfiguration configuration, IDbConnectionService _connection)
        {
            _configuration = configuration;
            connection= _connection;
        }

        public async Task<EmployeeUpdateResponse> Handle(UpdateEmployeeWithDapperCommand request, CancellationToken cancellationToken)
        {
            using var db = connection.CreateConnection();


            var parameters = new DynamicParameters();
            parameters.Add("@EmployeeId", request.Id, DbType.Guid);
            parameters.Add("@EmpName", request.EmpName, DbType.String);
            parameters.Add("@Email", request.Email, DbType.String);
            parameters.Add("@Phone", request.Phone, DbType.String);
            parameters.Add("@DepartmentId", request.DepartmentId, DbType.Guid);
            parameters.Add("@Role", request.Role, DbType.String);
            parameters.Add("@Position", request.Position);



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
