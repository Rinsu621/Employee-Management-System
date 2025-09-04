using EmployeeCRUD.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Domain.Entities
{
    public class Employee : BaseEntity
    {
        public required string EmpName { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
    }
}
