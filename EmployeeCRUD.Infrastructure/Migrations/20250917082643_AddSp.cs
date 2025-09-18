using Microsoft.EntityFrameworkCore.Migrations;
using EmployeeCRUD.Infrastructure.Helper;

#nullable disable

namespace EmployeeCRUD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            MigrationHelper.RunSqlScript(migrationBuilder, "EmployeeCRUD.Infrastructure.Scripts.DeleteEmployee.sql");
            MigrationHelper.RunSqlScript(migrationBuilder, "EmployeeCRUD.Infrastructure.Scripts.UpdateEmployee.sql");
            MigrationHelper.RunSqlScript(migrationBuilder, "EmployeeCRUD.Infrastructure.Scripts.PatchEmployee.sql");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
