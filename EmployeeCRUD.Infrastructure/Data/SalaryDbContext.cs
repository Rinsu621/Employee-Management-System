using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Infrastructure.Data
{
    public class SalaryDbContext:DbContext,ISalaryDbContext
    {
        public SalaryDbContext(DbContextOptions<SalaryDbContext> options) : base(options) { }

        public DbSet<Salary> Salaries { get; set; }
      

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var salaryConfigurations = typeof(SalaryDbContext).Assembly
         .GetTypes()
         .Where(t => t.GetInterfaces().Any(i =>
             i.IsGenericType &&
             i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>) &&
             i.GetGenericArguments()[0] == typeof(Salary))); // only Salary

            foreach (var config in salaryConfigurations)
            {
                dynamic instance = Activator.CreateInstance(config);
                builder.ApplyConfiguration(instance);
            }
            base.OnModelCreating(builder);
        }

         public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
