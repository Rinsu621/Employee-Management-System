using Ardalis.GuardClauses;
using EmployeeManagementSystem.Domain.Common;
using EmployeeManagementSystem.Domain.Guards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Domain.Entities
{
    public class Employee : BaseEntity
    {
        public  string EmpName { get;  set; }
        public  string Email { get;  set; }
        public  string Phone { get;  set; }

        public Guid? DepartmentId { get;  set; }

        public Department? Department { get;  set; }

        public ICollection<Project> Projects { get;  set; } = new List<Project>();


        public ICollection<Project> ManagedProjects { get;  set; } = new List<Project>();

        public ApplicationUser? User { get; set; }


    }
}
