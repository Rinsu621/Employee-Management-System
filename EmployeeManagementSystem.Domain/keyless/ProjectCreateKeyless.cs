using EmployeeManagementSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Infrastructure.Data.keyless
{
    public class ProjectCreateKeyless
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal Budget { get; set; }
        public string Status { get; set; } = "Planned";
        public string? ClientName { get; set; }
        public Department? Department { get; set; }
        public EmployeeManagementSystem.Domain.Entities.Employee? ProjectManager { get; set; }

        // List of team member names (strings)
        [NotMapped]
        public List<string> TeamMember { get; set; } = new List<string>();
    }
}

