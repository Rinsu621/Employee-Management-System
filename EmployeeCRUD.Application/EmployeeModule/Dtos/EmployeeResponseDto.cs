using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Dtos
{
    public class EmployeeResponseDto
    {
        public Guid Id { get; set; }
        public  string EmpName { get; set; }

        public  string Email { get; set; }
        public  string Phone { get; set; }
        public string? DepartmentName { get; set; } // nullable

        public string Role { get; set; }
        public DateTime CreatedAt { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(
        DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Nepal Standard Time"));
    }
}
