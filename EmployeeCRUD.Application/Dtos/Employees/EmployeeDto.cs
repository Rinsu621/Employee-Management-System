using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Dtos.Employees
{
    public class EmployeeDto
    {
        public required string EmpName { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }

    }
}
