using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Infrastructure.Data.keyless
{
        public class TeamMemberAssignmentResponse
        {
            public Guid ProjectId { get; set; }
            public string ProjectName { get; set; }
            public string Status { get; set; }
        public List<string> TeamMembers { get; set; }
    }
}
