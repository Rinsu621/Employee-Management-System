using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Infrastructure.Data.keyless;
using EmployeeCRUD.Infrastructure.Data.Keyless;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Infrastructure.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Project> Projects { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //This say go to the assesmbly i.e the project where AppDbContext lies and find all configure entities and apply automatically
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            modelBuilder.Entity<EmployeeResponseKeyless>().HasNoKey();
            modelBuilder.Entity<EmployeeUpdateKeyless>().HasNoKey();
            modelBuilder.Entity<ProjectCreateKeyless>().HasNoKey();

            base.OnModelCreating(modelBuilder);
        }
    }
}
