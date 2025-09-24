using Ardalis.GuardClauses;
using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application.EmployeeModule.Commands
{
    public record UpdateEmployeeCommand(
        Guid Id,
        string EmpName,
        string Email,
        string Phone,
        string Role
    ) : IRequest<EmployeeUpdateResponse>;

    public class UpdateEmployeeHandler : IRequestHandler<UpdateEmployeeCommand, EmployeeUpdateResponse>
    {
        private readonly IAppDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UpdateEmployeeHandler(
            IAppDbContext _dbContext,
            UserManager<ApplicationUser> _userManager,
            RoleManager<IdentityRole> _roleManager)
        {
            dbContext = _dbContext;
            userManager = _userManager;
            roleManager = _roleManager;
        }

        public async Task<EmployeeUpdateResponse> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            // 1. Update Employee entity
            var employee = await dbContext.Employees.FindAsync(new object[] { request.Id }, cancellationToken);
            Guard.Against.Null(employee, nameof(employee), $"Employee with Id '{request.Id}' not found.");

            employee.EmpName = request.EmpName;
            employee.Email = request.Email;
            employee.Phone = request.Phone;

            dbContext.Employees.Update(employee);
            await dbContext.SaveChangesAsync(cancellationToken);

            // 2. Fetch the user via UserManager
            var user = await userManager.Users
                .AsNoTracking() // Ensure EF doesn't track
                .FirstOrDefaultAsync(u => u.EmployeeId == employee.Id, cancellationToken);

            if (user != null)
            {
                // Update user details
                user.UserName = request.Email;
                user.Email = request.Email;
                await userManager.UpdateAsync(user); // Explicitly update user

                // Remove old roles
                var oldRoles = await userManager.GetRolesAsync(user);
                if (oldRoles.Any())
                {
                    await userManager.RemoveFromRolesAsync(user, oldRoles);
                }

                // Ensure new role exists
                if (!await roleManager.RoleExistsAsync(request.Role))
                {
                    await roleManager.CreateAsync(new IdentityRole(request.Role));
                }

                // Add new role
                await userManager.AddToRoleAsync(user, request.Role);
            }

            return new EmployeeUpdateResponse
            {
                Id = employee.Id,
                Name = employee.EmpName,
                Email = employee.Email,
                Phone = employee.Phone,
                Role = request.Role,
                CreatedAt = employee.CreatedAt,
                UpdatedAt = DateTime.UtcNow
            };
        }
    }
}
