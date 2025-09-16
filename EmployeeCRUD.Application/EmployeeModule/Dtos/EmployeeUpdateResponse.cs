using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Dtos
{
    public class EmployeeUpdateResponse
    {

        public Guid Id { get; set; }
        public required string Name { get; set; }

        public required string Email { get; set; }
        public required string Phone { get; set; }
        public DateTime CreatedAt { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Nepal Standard Time"));
        public DateTime? UpdatedAt { get; set; }=TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Nepal Standard Time"));

    }
}
