using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Application.EmployeeModule.Dtos
{
    public class EmployeePatchDto
    {
        public string? EmpName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
    }
}
