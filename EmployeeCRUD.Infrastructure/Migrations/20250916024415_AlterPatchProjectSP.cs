using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeCRUD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AlterPatchProjectSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.Sql(File.ReadAllText(@"..\EmployeeCRUD.Infrastructure\Scripts\PatchProject.sql"));
            var sqlFilePath= Path.Combine(AppContext.BaseDirectory, "Scripts", "PatchProject.sql");
            migrationBuilder.Sql(File.ReadAllText(sqlFilePath));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS PatchProject");

        }
    }
}
