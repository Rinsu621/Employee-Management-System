using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Domain.Interface;
using EmployeeCRUD.Domain.keyless;
using EmployeeCRUD.Infrastructure.Data.keyless;
using EmployeeCRUD.Infrastructure.Data.Keyless;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCRUD.Infrastructure.Data
{
    public class AppDbContext : DbContext,IAppDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<EmployeeResponseKeyless> EmployeeResponseKeyless { get; set; }
        public DbSet<EmployeeUpdateKeyless> EmployeeUpdateKeyless { get; set; }
        public DbSet<ProjectCreateKeyless> ProjectCreateKeyless { get; set; }
        public DbSet<TeamMemberAssignmentResponse> TeamMemberAssignmentResponses { get; set; }
        public DbSet<TeamMemberAssignmentRow> TeamMemberAssignmentRows { get; set; }
        public DbSet<ProjectDepartmentResponse>ProjectDepartmentResponses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            modelBuilder.Entity<EmployeeResponseKeyless>().HasNoKey();
            modelBuilder.Entity<EmployeeUpdateKeyless>().HasNoKey();
            modelBuilder.Entity<ProjectCreateKeyless>().HasNoKey();
            modelBuilder.Entity<TeamMemberAssignmentResponse>().HasNoKey();
            modelBuilder.Entity<TeamMemberAssignmentRow>().HasNoKey();
            modelBuilder.Entity<ProjectDepartmentResponse>().HasNoKey();



            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
