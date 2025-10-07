using EmployeeCRUD.Application.EmployeeModule.Dtos;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCRUD.Application.EmployeeModule.Commands
{
    public record UpdateEmployeeCommand(
        Guid Id,
        string EmpName,
        string Email,
        string Phone,
        Guid? DepartmentId,
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
            using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                var employee = await dbContext.Employees
                    .Include(e => e.Department)
                    .FirstOrDefaultAsync(e => e.Id == request.Id, cancellationToken);
                if (employee == null)
                    throw new Exception($"Employee with Id {request.Id} not found.");

                employee.EmpName = request.EmpName;
                employee.Email = request.Email;
                employee.Phone = request.Phone;
                employee.DepartmentId = request.DepartmentId;

                dbContext.Employees.Update(employee);
                await dbContext.SaveChangesAsync(cancellationToken);

                var user = await userManager.FindByIdAsync(employee.Id.ToString());
                if (user != null)
                {
                    user.UserName = request.EmpName;
                    user.Email = request.Email;

                    var updateResult = await userManager.UpdateAsync(user);
                    if (!updateResult.Succeeded)
                    {
                        throw new Exception($"Failed to update user: {string.Join(", ", updateResult.Errors.Select(e => e.Description))}");
                    }

                    var roles = await userManager.GetRolesAsync(user);
                    var currentRole = roles.FirstOrDefault();

                    if (currentRole != request.Role)
                    {
                        if (!string.IsNullOrEmpty(currentRole))
                        {
                            await userManager.RemoveFromRoleAsync(user, currentRole);
                        }
                        await userManager.AddToRoleAsync(user, request.Role);
                    }
                }
                await transaction.CommitAsync(cancellationToken);
                return new EmployeeUpdateResponse
                {
                    Id = employee.Id,
                    Name = employee.EmpName,
                    Email = employee.Email,
                    Phone = employee.Phone,
                    DepartmentName = employee.Department?.DeptName,
                    Role = request.Role,
                    CreatedAt = employee.CreatedAt,
                    UpdatedAt = DateTime.UtcNow
                };
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }

        }
    }
}
