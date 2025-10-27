
namespace EmployeeManagementSystem.Application.Interface
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
