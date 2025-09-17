using System;
using EmployeeCRUD.Infrastructure.Helper;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeCRUD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAssignTeamMemberSp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamMember",
                table: "ProjectCreateKeyless");

            migrationBuilder.CreateTable(
                name: "TeamMemberAssignmentResponses",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TeamMembers = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            //migrationBuilder.Sql(File.ReadAllText(@"..\EmployeeCRUD.Infrastructure\Scripts\AssignTeamMember.sql"));

            //var sqlFilePath = Path.Combine(AppContext.BaseDirectory, "Scripts", "AssignTeamMember.sql");
            //migrationBuilder.Sql(File.ReadAllText(sqlFilePath));

            //var assembly = typeof(AddAssignTeamMemberSp).Assembly;
            //using var stream = assembly.GetManifestResourceStream("EmployeeCRUD.Infrastructure.Scripts.AssignTeamMember.sql");
            //using var reader = new StreamReader(stream);
            //var sql = reader.ReadToEnd();
            //migrationBuilder.Sql(sql);

            MigrationHelper.RunSqlScript(migrationBuilder, "EmployeeCRUD.Infrastructure.Scripts.AssignTeamMember.sql");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamMemberAssignmentResponses");

            migrationBuilder.AddColumn<string>(
                name: "TeamMember",
                table: "ProjectCreateKeyless",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS AssignTeamMember");
        }
    }
}
