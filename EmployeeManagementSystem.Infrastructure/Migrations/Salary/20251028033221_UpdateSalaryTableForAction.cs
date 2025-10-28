using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeManagementSystem.Infrastructure.Migrations.Salary
{
    /// <inheritdoc />
    public partial class UpdateSalaryTableForAction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ActionAt",
                table: "Salaries",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ActionBy",
                table: "Salaries",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Salaries",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActionAt",
                table: "Salaries");

            migrationBuilder.DropColumn(
                name: "ActionBy",
                table: "Salaries");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Salaries");
        }
    }
}
