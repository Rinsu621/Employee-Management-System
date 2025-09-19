using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmployeeCRUD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UndoSalaryTableInAppDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
               name: "Salary"
           );

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "Salary",
               columns: table => new
               {
                   Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                   EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                   BasicSalary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                   Conveyance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                   Tax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                   PF = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                   ESI = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                   PaymentMode = table.Column<string>(type: "nvarchar(50)", nullable: false),
                   CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                   UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Salary", x => x.Id);
               });

        }
    }
}
