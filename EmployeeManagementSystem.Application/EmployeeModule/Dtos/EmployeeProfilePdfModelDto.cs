using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Application.EmployeeModule.Dtos
{
    public class EmployeeProfilePdfModelDto
    {
        public string EmpName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public string? Department { get; set; }
        public DateTime JoinedDate { get; set; }
        public string AvatarBase64 { get; set; }
    }
}
