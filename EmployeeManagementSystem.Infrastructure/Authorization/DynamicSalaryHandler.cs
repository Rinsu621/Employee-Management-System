using Dapper;
using EmployeeManagementSystem.Application.Common.Authorization;
using EmployeeManagementSystem.Application.Configuration;
using EmployeeManagementSystem.Application.Exceptions;
using EmployeeManagementSystem.Application.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Infrastructure.Authorization
{
    public class DynamicSalaryHandler: AuthorizationHandler<DynamicSalaryRequirement>
    {
        private readonly IDbConnectionService dbConnection;
        private readonly DbSettings dbSettings;
        public DynamicSalaryHandler(IDbConnectionService _dbConnection, IOptions<DbSettings> options)
        {
            dbConnection = _dbConnection;
            dbSettings = options.Value;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, DynamicSalaryRequirement requirement)
        {
            var userIdClaim = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new AuthorizationException("User is not authenticated.");
            }
            Guid userId = Guid.Parse(userIdClaim);

            using var conn = dbConnection.CreateConnection();
            var employee = await conn.QueryFirstOrDefaultAsync<dynamic>(
                 @"SELECT 
                    d.DeptName, 
                    e.Position, 
                    r.Name AS Role
                  FROM Employees e
                  LEFT JOIN Departments d ON e.DepartmentId = d.Id
                  INNER JOIN AspNetUsers u ON e.Id = u.EmployeeId
                  INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
                  INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
                  WHERE u.Id = @UserId",
                 new { UserId = userId }
                        );

            if (employee == null)
            {
                context.Fail();
                return;
            }

            string dept = employee.DeptName;
            string position = employee.Position;
            string role = employee.Role;

            if (requirement.Action == "AddSalary" &&
                dept == "Finance" &&
                role == "Manager" &&
                (position == "JuniorAccountant" || position == "SeniorAccountant"))
            {
                context.Succeed(requirement);
            }
            else if (requirement.Action == "ApproveSalary" &&
                     dept == "Finance" &&
                     role == "Manager" &&
                     position == "SeniorAccountant")
            {
                context.Succeed(requirement);
            }
            else
            {
                throw new AuthorizationException($"You are not allowed to perform '{requirement.Action}'.");
            }

        }
    }
}
