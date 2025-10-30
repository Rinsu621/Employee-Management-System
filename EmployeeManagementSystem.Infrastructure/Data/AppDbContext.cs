using EmployeeManagementSystem.Application.Interface;
using EmployeeManagementSystem.Domain.Entities;
using EmployeeManagementSystem.Domain.keyless;
using EmployeeManagementSystem.Infrastructure.Configurations.AppDBContextConfiguration;
using EmployeeManagementSystem.Infrastructure.Data.keyless;
using EmployeeManagementSystem.Infrastructure.Data.Keyless;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagementSystem.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>, IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<EmployeeManagementSystem.Domain.Entities.Employee> Employees { get; set; }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<EmployeeResponseKeyless> EmployeeResponseKeyless { get; set; }
        public DbSet<EmployeeUpdateKeyless> EmployeeUpdateKeyless { get; set; }
        public DbSet<ProjectCreateKeyless> ProjectCreateKeyless { get; set; }
        public DbSet<TeamMemberAssignmentResponse> TeamMemberAssignmentResponses { get; set; }
        public DbSet<TeamMemberAssignmentRow> TeamMemberAssignmentRows { get; set; }
        public DbSet<ProjectDepartmentResponse>ProjectDepartmentResponses { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new DepartmentConfiguration());
            modelBuilder.ApplyConfiguration(new ProjectConfiguration());
            modelBuilder.ApplyConfiguration(new RolePermissionConfiguration());



            modelBuilder.Entity<EmployeeResponseKeyless>().HasNoKey();
            modelBuilder.Entity<EmployeeUpdateKeyless>().HasNoKey();
            modelBuilder.Entity<ProjectCreateKeyless>().HasNoKey();
            modelBuilder.Entity<TeamMemberAssignmentResponse>().HasNoKey();
            modelBuilder.Entity<TeamMemberAssignmentRow>().HasNoKey();
            modelBuilder.Entity<ProjectDepartmentResponse>().HasNoKey();

            modelBuilder.Entity<EmployeeResponseKeyless>().ToView(null);

            base.OnModelCreating(modelBuilder);


        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
