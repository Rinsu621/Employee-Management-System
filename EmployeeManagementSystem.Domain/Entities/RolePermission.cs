using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Domain.Entities
{
    public class RolePermission
    {
        public string RoleId { get; set; } = null!; // matches AspNetRoles.Id
        public Guid PermissionId { get; set; }

        public IdentityRole Role { get; set; } = null!;
        public Permission Permission { get; set; } = null!;
    }
}
