using EmployeeCRUD.Infrastructure.Helper;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeCRUD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            MigrationHelper.RunSqlScript(migrationBuilder, "EmployeeCRUD.Infrastructure.Scripts.AddEmployee.sql");
            MigrationHelper.RunSqlScript(migrationBuilder, "EmployeeCRUD.Infrastructure.Scripts.AddProject.sql");
            MigrationHelper.RunSqlScript(migrationBuilder, "EmployeeCRUD.Infrastructure.Scripts.AssignDepartment.sql");
            MigrationHelper.RunSqlScript(migrationBuilder, "EmployeeCRUD.Infrastructure.Scripts.AssignTeamMember.sql");
            MigrationHelper.RunSqlScript(migrationBuilder, "EmployeeCRUD.Infrastructure.Scripts.DeleteEmployee.sql");
            MigrationHelper.RunSqlScript(migrationBuilder, "EmployeeCRUD.Infrastructure.Scripts.GetAllEmployees.sql");
            MigrationHelper.RunSqlScript(migrationBuilder, "EmployeeCRUD.Infrastructure.Scripts.GetEmployeeById.sql");
            MigrationHelper.RunSqlScript(migrationBuilder, "EmployeeCRUD.Infrastructure.Scripts.PatchEmployee.sql");
            MigrationHelper.RunSqlScript(migrationBuilder, "EmployeeCRUD.Infrastructure.Scripts.PatchProject.sql");
            MigrationHelper.RunSqlScript(migrationBuilder, "EmployeeCRUD.Infrastructure.Scripts.UpdateEmployee.sql");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS AddEmployee");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS AddProject");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS AssignDepartment");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS AssignTeamMember");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS DeleteEmployee");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetAllEmployees");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetEmployeeById");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS PatchEmployee");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS PatchProject");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS UpdateEmployee");

        }
    }
}
