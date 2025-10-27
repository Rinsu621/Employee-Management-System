using EmployeeManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Application.Department.Dtos
{
    public class DepartmentResultDto
    {
        public Guid Id { get; set; }
        public  required string Name { get; set; }
        public ICollection<EmployeeReturnForDepartmentDto> Employees { get; set; } = new List<EmployeeReturnForDepartmentDto>();


    }
}
