using EmployeeManagementSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Infrastructure.Factory
{
    //IDesignTimeDbContextFactory is used by EF core at design time when run command like add-migration and update db
    public class SalaryDbContextFactory : IDesignTimeDbContextFactory<SalaryDbContext>
    {
        public SalaryDbContext CreateDbContext(string[] args)
        {
            // Build configuration manually
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
                .Build();

            // Read connection string
            var connectionString = config.GetConnectionString("SalaryConnection");

            var optionsBuilder = new DbContextOptionsBuilder<SalaryDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new SalaryDbContext(optionsBuilder.Options);
        }
    }
}
