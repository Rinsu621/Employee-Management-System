using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Department.Dtos
{
    public class DeleteDepartmentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
