using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Domain.keyless;
using EmployeeCRUD.Infrastructure.Data.keyless;
using EmployeeCRUD.Infrastructure.Data.Keyless;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;


namespace EmployeeCRUD.Application.Interface
{
    public interface IAppDbContext
    {
        DbSet<Employee> Employees { get; set; }
        DbSet<EmployeeCRUD.Domain.Entities.Department> Departments { get; set; }
        DbSet<Project> Projects { get; set; }
        DbSet<EmployeeResponseKeyless> EmployeeResponseKeyless { get; set; }
        DbSet<EmployeeUpdateKeyless> EmployeeUpdateKeyless { get; set; }
        DbSet<ProjectCreateKeyless> ProjectCreateKeyless { get; set; }
        DbSet<TeamMemberAssignmentResponse> TeamMemberAssignmentResponses { get; set; }
        DbSet<TeamMemberAssignmentRow> TeamMemberAssignmentRows { get; set; }
        DbSet<ProjectDepartmentResponse> ProjectDepartmentResponses { get; set; }

        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        ChangeTracker ChangeTracker { get; }
        DbSet<T> Set<T>() where T : class;
    }
}
