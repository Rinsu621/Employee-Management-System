using Dapper;
using EmployeeCRUD.Application.EmployeeModule.Document;
using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Interface;
using MediatR;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Queries
{
    public record ExportEmployeesToPdfUsingQuestPdfQuery (
        string? Role = null,
        Guid? DepartmentId = null,
        DateTime? FromDate = null,
        DateTime? ToDate = null,
        string? SearchTerm = null,
        string? SortKey = "CreatedAt",
        bool SortAsc = true
    ) : IRequest<byte[]>;

    public class ExportEmployeesToPdfUsingQuestPdfHandler : IRequestHandler<ExportEmployeesToPdfUsingQuestPdfQuery, byte[]>
    {
        private readonly IPdfService pdfService;
        private IEmployeeDbConnection connection;
        public ExportEmployeesToPdfUsingQuestPdfHandler(IPdfService _pdfService, IEmployeeDbConnection _connection)
        {
            pdfService= _pdfService;
            connection= _connection;
        }
        public async Task<byte[]> Handle(ExportEmployeesToPdfUsingQuestPdfQuery request, CancellationToken cancellationToken)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@Page", 1);
            parameters.Add("@PageSize", int.MaxValue); // Get all records
            parameters.Add("@Role", request.Role);
            parameters.Add("@DepartmentId", request.DepartmentId);
            parameters.Add("@FromDate", request.FromDate);
            parameters.Add("@ToDate", request.ToDate);
            parameters.Add("@SearchTerm", request.SearchTerm);
            parameters.Add("@SortKey", request.SortKey);
            parameters.Add("@SortAsc", request.SortAsc);

            using var db = connection.CreateConnection();
            using var multi = await db.QueryMultipleAsync(
                "GetAllEmployees",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            var employees = multi.Read<EmployeeResponseDto>().ToList();

            var employeePdfModels = employees.Select((emp, index) => new EmployeePdfModel
            {
                SNo = index + 1,
                Name = emp.EmpName,
                Email = emp.Email,
                Phone = emp.Phone,
                Department = emp.DepartmentName,
                Role = emp.Role,
                CreatedAt = emp.CreatedAt
            }).ToList();
            var employeeDocument = new EmployeeTablePdf(employeePdfModels);
            var pdfBytes = pdfService.GeneratePdf(employeeDocument);
            return pdfBytes;
        }

    }
    }
