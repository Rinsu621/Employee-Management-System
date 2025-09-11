using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.ProjectModule.Dtos
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string ProjectName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal Budget { get; set; }
        public string Status { get; set; } = "Planned";
        public string? ClientName { get; set; }

        // Only include relevant info to avoid circular refs
        public string? DepartmentName { get; set; }
        public string? ProjectManagerName { get; set; }

        public List<string> TeamMember { get; set; } = new List<string>();

    }
}
