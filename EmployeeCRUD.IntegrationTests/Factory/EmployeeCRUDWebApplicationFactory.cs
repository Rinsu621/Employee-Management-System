using EmployeeCRUD.Infrastructure.Data;
using EmployeeCRUD.Infrastructure.Seeder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Data;

namespace EmployeeCRUD.IntegrationTests.Factory
{
    public class EmployeeCRUDWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json")
                      .AddJsonFile("appsettings.Test.json", optional: true);
            });

            builder.ConfigureTestServices(services =>
            {
                // Remove existing DbContext registrations
                services.RemoveAll(typeof(DbContextOptions<AppDbContext>));

                // Get configuration
                var sp = services.BuildServiceProvider();
                var configuration = sp.GetRequiredService<IConfiguration>();
                var testConnection = configuration.GetConnectionString("TestConnection");

                // Register AppDbContext with SQL Server
                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseSqlServer(testConnection)
                    .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information)  // Logs all SQL
                    .EnableSensitiveDataLogging();
                });

                services.AddScoped<IDbConnection>(sp =>
               new SqlConnection(configuration.GetConnectionString("TestConnection")));

                // Seed roles & admin
                using var scope = services.BuildServiceProvider().CreateScope();
                var scopedProvider = scope.ServiceProvider;
                var dbContext = scopedProvider.GetRequiredService<AppDbContext>();

                dbContext.Database.EnsureDeleted(); // Optional: start fresh
                dbContext.Database.Migrate();

                IdentitySeeder.SeedRolesAndAdminAsync(scopedProvider).GetAwaiter().GetResult();
            });
        }
    }
}
