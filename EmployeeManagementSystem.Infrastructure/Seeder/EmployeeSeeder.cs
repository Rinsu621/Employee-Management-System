using Bogus;
using EmployeeManagementSystem.Domain.Entities;
using EmployeeManagementSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Infrastructure.Seeder
{
    public static class EmployeeSeeder
    {
        public static async Task Seed(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            if (!context.Departments.Any())
                throw new InvalidOperationException("Please seed departments first.");

            if (context.Employees.Any())
                return;

            var departmentIds = context.Departments.Select(d => d.Id).ToList();
            var existingEmails = context.Employees
                            .Select(e => e.Email.ToLower())
                            .ToHashSet();

            var faker = new Faker<EmployeeManagementSystem.Domain.Entities.Employee>()
                .RuleFor(e => e.Id, f => Guid.NewGuid())
                .RuleFor(e => e.EmpName, f => f.Name.FullName())
              
                .RuleFor(e => e.Phone, f => "98" + f.Random.Number(10000000, 99999999).ToString())
                .RuleFor(e => e.DepartmentId, f => f.PickRandom(departmentIds))
                 .RuleFor(e => e.Email, f =>
                 {
                     string email;
                     do
                     {
                         email = f.Internet.Email().ToLower();
                     } while (existingEmails.Contains(email));

                     existingEmails.Add(email); // mark it as used
                     return email;
                 });

            var employees = faker.Generate(40000);

            context.Employees.AddRange(employees);
            await context.SaveChangesAsync();

            // Ensure roles exist
            string[] roles = { "Employee", "Manager" };
            foreach (var role in roles)
            {
                if (!await context.Roles.AnyAsync(r => r.Name == role))
                    throw new InvalidOperationException($"Role '{role}' does not exist. Seed roles first.");
            }

            foreach (var emp in employees)
            {
                // Skip if user already exists
                if (await userManager.FindByEmailAsync(emp.Email) != null)
                    continue;

                var user = new ApplicationUser
                {
                    UserName = emp.Email,
                    Email = emp.Email,
                    EmployeeId = emp.Id
                };

                await userManager.CreateAsync(user, "Default@123");

               
                await userManager.AddToRoleAsync(user, "Employee");
            }
        }
    }
}
