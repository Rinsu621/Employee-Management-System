using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Infrastructure.Data.keyless
{
    public class EmployeeUpdateKeyless
    {
        public Guid Id { get; set; }
        public required string EmpName { get; set; }

        public required string Email { get; set; }
        public required string Phone { get; set; }
        public DateTime CreatedAt { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Nepal Standard Time"));
        public DateTime? UpdatedAt { get; set; } = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Nepal Standard Time"));
    }
}
