using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Domain.keyless
{
    public class TeamMemberAssignmentRow
    {
        public Guid ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Status { get; set; }
        public string TeamMember { get; set; }
    }
}
