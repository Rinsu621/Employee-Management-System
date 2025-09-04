using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.Dtos.Departments
{
    public class EmployeeReturnForDepartmentDto
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }

        public required string Email { get; set; }
        public required string Phone { get; set; }
    }
}
