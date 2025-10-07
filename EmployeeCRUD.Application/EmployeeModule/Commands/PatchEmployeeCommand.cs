using Ardalis.GuardClauses;
using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Domain.Entities;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCRUD.Application.EmployeeModule.Commands
{
    public record PatchEmployeeCommand(Guid Id, string? EmpName, string? Phone, string? Email, string? Role) : IRequest<EmployeeUpdateResponse>;

    public class PatchEmployeeHandler : IRequestHandler<PatchEmployeeCommand, EmployeeUpdateResponse>
    {

        private readonly IAppDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        public PatchEmployeeHandler(IAppDbContext _dbContext, UserManager<ApplicationUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {

            dbContext = _dbContext;
            userManager = _userManager;
            roleManager = _roleManager;
        }
        public async Task<EmployeeUpdateResponse> Handle(PatchEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employee = await dbContext.Employees.FindAsync(request.Id);
            Guard.Against.Null(employee, nameof(employee), $"Employee with Id '{request.Id}' not found.");

            if (!string.IsNullOrEmpty(request.EmpName))
                employee.EmpName = request.EmpName;

            if (!string.IsNullOrEmpty(request.Email))
                employee.Email = request.Email;

            if (!string.IsNullOrEmpty(request.Phone))
                employee.Phone = request.Phone;

            employee.UpdatedAt = DateTime.UtcNow;
            dbContext.Employees.Update(employee);
            await dbContext.SaveChangesAsync(cancellationToken);

            var user = await userManager.Users.FirstOrDefaultAsync(u => u.EmployeeId == employee.Id, cancellationToken);
            if (user != null)
            {
                if (!string.IsNullOrEmpty(request.Email))
                {
                    user.Email = request.Email;
                    user.UserName = request.Email;
                    var result = await userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                        throw new Exception($"Failed to update user email: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
                if (!string.IsNullOrEmpty(request.Role))
                {
                    var currentRole = await userManager.GetRolesAsync(user);
                    if (currentRole != null)
                    {
                        await userManager.RemoveFromRolesAsync(user, currentRole);
                    }
                    await userManager.AddToRoleAsync(user, request.Role);

                }
            }



                return new EmployeeUpdateResponse
                {
                    Id = employee.Id,
                    Name = employee.EmpName,
                    Email = employee.Email,
                    Phone = employee.Phone,
                    Role = request.Role ?? (user != null ? (await userManager.GetRolesAsync(user)).FirstOrDefault() : null),
                    CreatedAt = employee.CreatedAt,
                    UpdatedAt = DateTime.UtcNow
                };

            }
        }
}
