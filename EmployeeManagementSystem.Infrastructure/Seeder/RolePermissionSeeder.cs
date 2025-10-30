using EmployeeManagementSystem.Application.Interface;
using EmployeeManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Infrastructure.Seeder
{
    public static class RolePermissionSeeder
    {
        public static async Task Seed(IServiceProvider serviceProvider, CancellationToken cancellationToken)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var dbContext = serviceProvider.GetRequiredService<IAppDbContext>();

            var permissions = await dbContext.Permissions.ToListAsync(cancellationToken);

            var roles = new List<string> { "Admin", "Manager" };

            // Define which permissions belong to which roles
            var rolePermissionMapping = new Dictionary<string, List<string>>
            {
                ["Admin"] = new List<string> { "salary:add", "salary:approve", "employee:read", "employee:update" , "department:add"},
                ["Manager"] = new List<string> { "salary:add" }
            };

            foreach (var roleName in roles)
            {
                var role = await roleManager.FindByNameAsync(roleName);
                if (role == null) continue;

                if (!rolePermissionMapping.ContainsKey(roleName)) continue;

                foreach (var permissionName in rolePermissionMapping[roleName])
                {
                    var permission = permissions.FirstOrDefault(p => p.Name == permissionName);
                    if (permission == null) continue;

                    bool alreadyExists = await dbContext.RolePermissions
                        .AnyAsync(rp => rp.RoleId == role.Id && rp.PermissionId == permission.Id, cancellationToken);

                    if (!alreadyExists)
                    {
                        dbContext.RolePermissions.Add(new RolePermission
                        {
                            RoleId = role.Id,
                            PermissionId = permission.Id
                        });
                    }
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
