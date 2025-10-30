using EmployeeManagementSystem.Domain.Entities;
using EmployeeManagementSystem.Domain.keyless;
using EmployeeManagementSystem.Infrastructure.Data.keyless;
using EmployeeManagementSystem.Infrastructure.Data.Keyless;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;



namespace EmployeeManagementSystem.Application.Interface
{
    public interface IAppDbContext
    {
        DbSet<EmployeeManagementSystem.Domain.Entities.Employee> Employees { get; set; }
        DbSet<EmployeeManagementSystem.Domain.Entities.Department> Departments { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<EmployeeResponseKeyless> EmployeeResponseKeyless { get; set; }
        DbSet<EmployeeUpdateKeyless> EmployeeUpdateKeyless { get; set; }
        DbSet<ProjectCreateKeyless> ProjectCreateKeyless { get; set; }
        DbSet<TeamMemberAssignmentResponse> TeamMemberAssignmentResponses { get; set; }
        DbSet<TeamMemberAssignmentRow> TeamMemberAssignmentRows { get; set; }
        DbSet<ProjectDepartmentResponse> ProjectDepartmentResponses { get; set; }
        DbSet<Permission> Permissions { get; set; }
        DbSet<RolePermission> RolePermissions { get; set; }

        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        ChangeTracker ChangeTracker { get; }
        DbSet<T> Set<T>() where T : class;
    }
}
