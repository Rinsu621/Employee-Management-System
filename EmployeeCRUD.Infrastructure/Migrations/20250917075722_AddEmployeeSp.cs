using EmployeeCRUD.Infrastructure.Helper;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeCRUD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeSp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            MigrationHelper.RunSqlScript(migrationBuilder, "EmployeeCRUD.Infrastructure.Scripts.AddEmployee.sql");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS AddEmployee");

        }
    }
}
