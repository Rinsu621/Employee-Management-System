using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Application.EmployeeModule.Dtos
{
    public class DeleteEmployeeResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
