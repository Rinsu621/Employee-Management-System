using ClosedXML.Excel;
using Dapper;
using DocumentFormat.OpenXml.Vml.Office;
using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Infrastructure.Services
{
    public class ExportEmployeesToExcelService : IExcelExpoter
    {
        private readonly IDbConnectionService connection;

        public ExportEmployeesToExcelService(IDbConnectionService _connection)
        {
            connection = _connection;
        }
        public async Task<byte[]> ExportToExcel(string? role = null,
             Guid? departmentId = null,
             DateTime? fromDate = null,
             DateTime? toDate = null,
             string? searchTerm = null,
             string? sortKey = "CreatedAt",
             bool sortAsc = true)

        {
            var parameters = new DynamicParameters();
            parameters.Add("@Page", 1);
            parameters.Add("@PageSize", int.MaxValue);
            parameters.Add("@Role", role);
            parameters.Add("@DepartmentId", departmentId);
            parameters.Add("@FromDate", fromDate);
            parameters.Add("@ToDate", toDate);
            parameters.Add("@SearchTerm", searchTerm);
            parameters.Add("@SortKey", sortKey);
            parameters.Add("@SortAsc", sortAsc);

            using var db= connection.CreateConnection();
            var query = await db.QueryMultipleAsync("GetAllEmployees",
                parameters,
                commandType: CommandType.StoredProcedure);
            var employees = query.Read<EmployeeResponseDto>().ToList();

            using var wb = new XLWorkbook();
            var worksheet = wb.AddWorksheet("Employees");
            worksheet.Cell(1, 1).Value = "Id";
            worksheet.Cell(1, 2).Value = "Name";
            worksheet.Cell(1, 3).Value = "Email";
            worksheet.Cell(1, 4).Value = "Phone";
            worksheet.Cell(1, 5).Value = "Department";
            worksheet.Cell(1, 6).Value = "Role";
            worksheet.Cell(1, 7).Value = "Created At";

            //styling
            var headerRange = worksheet.Range("A1:G1");
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Font.FontColor = XLColor.White;
            headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#000000");
            headerRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
            headerRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;
            worksheet.SheetView.FreezeRows(1);




            int row = 2;
            int count = 1;
            foreach (var emp in employees)
            {
                worksheet.Cell(row, 1).Value = count++;
                worksheet.Cell(row, 2).Value = emp.EmpName;
                worksheet.Cell(row, 3).Value = emp.Email;
                worksheet.Cell(row, 4).Value = emp.Phone;
                worksheet.Cell(row, 5).Value = emp.DepartmentName;
                worksheet.Cell(row, 6).Value = emp.Role;
                worksheet.Cell(row, 7).Value = emp.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss");
                row++;
            }

            worksheet.Columns().AdjustToContents();
            worksheet.Row(1).Height = 20;

            using var stream = new MemoryStream();
            wb.SaveAs(stream);
            return stream.ToArray(); //convert to byte[]

        }
    }
}
