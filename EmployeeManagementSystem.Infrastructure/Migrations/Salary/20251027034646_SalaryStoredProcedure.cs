using EmployeeManagementSystem.Infrastructure.Helper;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementSystem.Infrastructure.Migrations.Salary
{
    /// <inheritdoc />
    public partial class SalaryStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            MigrationHelper.RunSqlScript(migrationBuilder, "EmployeeManagementSystem.Infrastructure.Scripts.AddSalary.sql");
            MigrationHelper.RunSqlScript(migrationBuilder, "EmployeeManagementSystem.Infrastructure.Scripts.GetSalaryDetails.sql");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS AddSalary");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS GetSalaryDetails");
        }
    }
}
