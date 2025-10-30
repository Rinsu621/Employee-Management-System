using DocumentFormat.OpenXml.InkML;
using EmployeeManagementSystem.Application.Interface;
using EmployeeManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Infrastructure.Seeder
{
    public static class PermissionSeeder
    {
        public static async Task SeedAsync(IAppDbContext dbContext, CancellationToken cancellationToken = default)
        {
           
            var permissionsList = new List<Permission>
            {
                new Permission { Name = "salary:add", Description = "Add salary" },
                new Permission { Name = "salary:approve", Description = "Approve salary" },
                new Permission { Name = "employee:read", Description = "Read employee" },
                new Permission { Name = "employee:update", Description = "Update employee" },
                new Permission { Name = "employee:delete", Description = "Delete employee" },
                new Permission { Name = "salary:read", Description = "Read salary" },
                new Permission { Name = "department:add", Description = "Add Department" }
            };

            foreach (var permission in permissionsList)
            {
                if (!await dbContext.Permissions.AnyAsync(p => p.Name == permission.Name, cancellationToken))
                {
                    dbContext.Permissions.Add(permission);
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
