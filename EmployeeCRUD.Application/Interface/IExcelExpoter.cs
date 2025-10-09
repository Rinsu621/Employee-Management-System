using EmployeeCRUD.Application.EmployeeModule.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Interface
{
    public interface IExcelExpoter
    {
       Task < byte[]> ExportToExcel(string? role = null,
            Guid? departmentId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null,
            string? searchTerm = null,
            string? sortKey = "CreatedAt",
            bool sortAsc = true);
    }
}
