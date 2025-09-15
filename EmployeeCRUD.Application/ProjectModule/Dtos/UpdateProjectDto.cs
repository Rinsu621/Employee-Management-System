using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.ProjectModule.Dtos
{
    public class UpdateProjectDto
    {
        public string? ProjectName { get; set; }
        public string? Description { get; set; }
        public Guid? DepartmentId { get; set; }
        public DateTime? EndDate { get; set; }
        public decimal? Budget { get; set; }
        public string? ClientName { get; set; }
        public string? Status { get; set; }
        public Guid? ProjectManagerId { get; set; }
        public bool? IsArchived { get; set; }          
        public List<Guid>? TeamMemberIds { get; set; }
    }
}
