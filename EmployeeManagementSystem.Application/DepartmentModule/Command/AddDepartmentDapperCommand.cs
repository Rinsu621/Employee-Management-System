using Dapper;
using EmployeeManagementSystem.Application.DepartmentModule.Dtos;
using EmployeeManagementSystem.Application.Interface;
using EmployeeManagementSystem.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Application.DepartmentModule.Command
{
    public record AddDepartmentDapperCommand(string DeptName):IRequest< DepartmentResponseDto>;

    public class AddDepartmentDapperHandler : IRequestHandler<AddDepartmentDapperCommand, DepartmentResponseDto>
    {
        private readonly IDbConnectionService connection;
        public AddDepartmentDapperHandler(IDbConnectionService _connection)
        {
            connection = _connection;
        }

        public async Task<DepartmentResponseDto> Handle(AddDepartmentDapperCommand request, CancellationToken cancellationToken)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@DeptName", request.DeptName);
            parameters.Add("@Id", dbType: DbType.Guid, direction: ParameterDirection.Output);

            using var db = connection.CreateConnection();
            var department = await db.QuerySingleAsync<DepartmentResponseDto>(
                 "AddDepartment",
                 parameters,
                 commandType: CommandType.StoredProcedure
             );

            return department;
        }

    }
}
