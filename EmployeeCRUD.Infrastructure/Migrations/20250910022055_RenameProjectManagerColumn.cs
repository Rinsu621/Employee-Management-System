using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeCRUD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameProjectManagerColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_Employees_ProjectMamagerId",
                table: "Project");

            migrationBuilder.RenameColumn(
                name: "ProjectMamagerId",
                table: "Project",
                newName: "ProjectManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Project_ProjectMamagerId",
                table: "Project",
                newName: "IX_Project_ProjectManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Employees_ProjectManagerId",
                table: "Project",
                column: "ProjectManagerId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_Employees_ProjectManagerId",
                table: "Project");

            migrationBuilder.RenameColumn(
                name: "ProjectManagerId",
                table: "Project",
                newName: "ProjectMamagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Project_ProjectManagerId",
                table: "Project",
                newName: "IX_Project_ProjectMamagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Employees_ProjectMamagerId",
                table: "Project",
                column: "ProjectMamagerId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
