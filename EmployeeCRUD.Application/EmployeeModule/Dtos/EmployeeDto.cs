using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Dtos
{
    public class EmployeeDto
    {
        public  string EmpName { get; set; }
        public  string Email { get; set; }
        public  string Phone { get; set; }

        public string Role { get; set; }

    }
}
