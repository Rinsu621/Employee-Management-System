using EmployeeCRUD.Application.EmployeeModule.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Interface
{
    public interface IPdfService
    {
        Task<byte[]> GeneratePdfAsync(string htmlContent);
    }
}
