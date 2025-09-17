using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeCRUD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignDepartmentSp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TeamMemberAssignmentRows",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeamMember = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            //migrationBuilder.Sql(File.ReadAllText(@"..\EmployeeCRUD.Infrastructure\Scripts\AssignDepartment.sql"));
            //var sqlFilePath = Path.Combine(AppContext.BaseDirectory, "Scripts", "AssignDepartment.sql");
            //migrationBuilder.Sql(File.ReadAllText(sqlFilePath));

            var assembly = typeof(AddAssignDepartmentSp).Assembly;
            using var stream = assembly.GetManifestResourceStream("EmployeeCRUD.Infrastructure.Scripts.AssignDepartment.sql");
            using var reader = new StreamReader(stream);
            var sql = reader.ReadToEnd();
            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamMemberAssignmentRows");
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS AssignDepartment");
        }
    }
}
