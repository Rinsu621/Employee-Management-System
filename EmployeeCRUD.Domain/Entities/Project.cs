using EmployeeCRUD.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Domain.Entities
{
    public class Project:BaseEntity
    {
        public required string ProjectName{ get; set; }

        public required string Description { get; set; }

        //Relation with the Department i.e one to many
        public Guid? DepartmentId { get; set; }

        public Department? Department { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public decimal Budget {  get; set; }
        public string? ClientName { get; set; }

        public string Status { get; set; } = "Planned";

        public Guid? ProjectManagerId { get; set; }

        public Employee? ProjectManager { get; set; }

        public ICollection<Employee> TeamMember { get; set; } = new List<Employee>();

        public bool IsArchived { get; set; } = false;








    }
}
