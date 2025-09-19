using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Infrastructure.Seeder
{
    public static class IdentitySeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            //Manage Identity role(create , check the existence of rolw)
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //manage identity like create , assign role , validate password
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            //access congiguration values like admin email, password
            var config = serviceProvider.GetRequiredService<IConfiguration>();
            //Defining role to seed
            var roles = new[] { "Admin", "Manager", "Employee" };

            //Seed roles
            foreach(var role in roles)
            {
                if(!await roleManager.RoleExistsAsync(role))
                {
                    //if not exists it create using CreateAsync
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            //Seed admin user, read credential from config
            var adminEmail = config["AdminUser:Email"];
            var adminPassword = config["AdminUser:Password"];
            var userExist = await userManager.FindByEmailAsync(adminEmail);

            if (userExist==null)
            {
                var employee = new Employee
                {
                    EmpName = "Admin User",
                    Email = adminEmail,
                    Phone = "9876543210"
                };
                var dbContext = serviceProvider.GetRequiredService<AppDbContext>();
                dbContext.Employees.Add(employee);
                await dbContext.SaveChangesAsync();

                var adminUser = new ApplicationUser
                {
                    UserName = employee.Email,
                    Email = employee.Email,
                    EmployeeId = employee.Id
                };
                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }


            }
        }
    }
}
