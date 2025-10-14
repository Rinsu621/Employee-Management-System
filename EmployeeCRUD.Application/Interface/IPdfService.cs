
using EmployeeCRUD.Application.EmployeeModule.Dtos;

namespace EmployeeCRUD.Application.Interface
{
    public interface IPdfService
    {
        byte[] GenerateEmployeeTablePdf(List<EmployeePdfModel> employees);
    }
}
