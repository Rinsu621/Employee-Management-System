using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeCRUD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectSP : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProjects_Project_ProjectsId",
                table: "EmployeeProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Departments_DepartmentId",
                table: "Project");

            migrationBuilder.DropForeignKey(
                name: "FK_Project_Employees_ProjectManagerId",
                table: "Project");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Project",
                table: "Project");

            migrationBuilder.RenameTable(
                name: "Project",
                newName: "Projects");

            migrationBuilder.RenameIndex(
                name: "IX_Project_ProjectName",
                table: "Projects",
                newName: "IX_Projects_ProjectName");

            migrationBuilder.RenameIndex(
                name: "IX_Project_ProjectManagerId",
                table: "Projects",
                newName: "IX_Projects_ProjectManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Project_DepartmentId",
                table: "Projects",
                newName: "IX_Projects_DepartmentId");

            migrationBuilder.AddColumn<Guid>(
                name: "DepartmentId",
                table: "ProjectCreateKeyless",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectManagerId",
                table: "ProjectCreateKeyless",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TeamMember",
                table: "ProjectCreateKeyless",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projects",
                table: "Projects",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCreateKeyless_DepartmentId",
                table: "ProjectCreateKeyless",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectCreateKeyless_ProjectManagerId",
                table: "ProjectCreateKeyless",
                column: "ProjectManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeProjects_Projects_ProjectsId",
                table: "EmployeeProjects",
                column: "ProjectsId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCreateKeyless_Departments_DepartmentId",
                table: "ProjectCreateKeyless",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectCreateKeyless_Employees_ProjectManagerId",
                table: "ProjectCreateKeyless",
                column: "ProjectManagerId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Departments_DepartmentId",
                table: "Projects",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Employees_ProjectManagerId",
                table: "Projects",
                column: "ProjectManagerId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.Sql(@"
                        CREATE PROCEDURE dbo.CreateProjject
                            @ProjectName VARCHAR(100),
                            @Description VARCHAR(200),
                            @StartDate DATETIME,
                            @EndDate DATETIME,
                            @Budget DECIMAL(18,2),
                            @Status VARCHAR(50)='Planned',
                            @ClientName VARCHAR(200)=NULL,
                            @DepartmentId UNIQUEIDENTIFIER = NULL,
                            @ProjectManagerId UNIQUEIDENTIFIER = NULL,
                            @NewId UNIQUEIDENTIFIER OUTPUT
                        AS
                        BEGIN
                            SET NOCOUNT ON;
                            SET @NewId = NEWID();
                            INSERT INTO dbo.Projects (Id, ProjectName, Description, StartDate, EndDate, Budget, Status, ClientName)
                            VALUES (@NewId, @ProjectName, @Description, @StartDate, @EndDate, @Budget, @Status, @ClientName);
                        END
                    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeProjects_Projects_ProjectsId",
                table: "EmployeeProjects");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCreateKeyless_Departments_DepartmentId",
                table: "ProjectCreateKeyless");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectCreateKeyless_Employees_ProjectManagerId",
                table: "ProjectCreateKeyless");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Departments_DepartmentId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Employees_ProjectManagerId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_ProjectCreateKeyless_DepartmentId",
                table: "ProjectCreateKeyless");

            migrationBuilder.DropIndex(
                name: "IX_ProjectCreateKeyless_ProjectManagerId",
                table: "ProjectCreateKeyless");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Projects",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "ProjectCreateKeyless");

            migrationBuilder.DropColumn(
                name: "ProjectManagerId",
                table: "ProjectCreateKeyless");

            migrationBuilder.DropColumn(
                name: "TeamMember",
                table: "ProjectCreateKeyless");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "Project");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_ProjectName",
                table: "Project",
                newName: "IX_Project_ProjectName");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_ProjectManagerId",
                table: "Project",
                newName: "IX_Project_ProjectManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_DepartmentId",
                table: "Project",
                newName: "IX_Project_DepartmentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Project",
                table: "Project",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeProjects_Project_ProjectsId",
                table: "EmployeeProjects",
                column: "ProjectsId",
                principalTable: "Project",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Departments_DepartmentId",
                table: "Project",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Employees_ProjectManagerId",
                table: "Project",
                column: "ProjectManagerId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS dbo.AddProject");
        }
    }
}
