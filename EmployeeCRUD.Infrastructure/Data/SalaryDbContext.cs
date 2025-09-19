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
    public class SalaryDbContext:IdentityDbContext<ApplicationUser, IdentityRole, string>,ISalaryDbContext
    {
        public SalaryDbContext(DbContextOptions<SalaryDbContext> options) : base(options) { }

        public DbSet<Salary> Salarys { get; set; }
      

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(SalaryDbContext).Assembly);
            base.OnModelCreating(builder);
        }
    }
}
