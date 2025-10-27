
using MediatR;
using EmployeeManagementSystem.Application.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Application.EmployeeModule.Queries
{
    public record ExportEmployeesToExcelQuery(
        string? Role = null,
        Guid? DepartmentId = null,
        DateTime? FromDate = null,
        DateTime? ToDate = null,
        string? SearchTerm = null,
        string? SortKey = "CreatedAt",
        bool SortAsc = true
    ) : IRequest<byte[]>;
   
    public class ExportEmployeesToExcelHandler : IRequestHandler<ExportEmployeesToExcelQuery, byte[]>
    {
        private readonly IExcelExpoter excelExpoter;
        public ExportEmployeesToExcelHandler(IExcelExpoter _excelExpoter)
        {
            excelExpoter = _excelExpoter;
        }
        public async Task<byte[]> Handle(ExportEmployeesToExcelQuery request, CancellationToken cancellationToken)
        {
            return await excelExpoter.ExportToExcel(
                request.Role,
                request.DepartmentId,
                request.FromDate,
                request.ToDate,
                request.SearchTerm,
                request.SortKey,
                request.SortAsc);
        }
    }
}
