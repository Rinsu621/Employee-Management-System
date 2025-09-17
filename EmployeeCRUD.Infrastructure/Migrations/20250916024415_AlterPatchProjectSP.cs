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
            //var sqlFilePath= Path.Combine(AppContext.BaseDirectory, "Scripts", "PatchProject.sql");
            //migrationBuilder.Sql(File.ReadAllText(sqlFilePath));

            var assembly = typeof(AlterPatchProjectSP).Assembly;
            using var stream = assembly.GetManifestResourceStream("EmployeeCRUD.Infrastructure.Scripts.PatchProject.sql");
            using var reader = new StreamReader(stream);
            var sql = reader.ReadToEnd();
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS PatchProject");

        }
    }
}
