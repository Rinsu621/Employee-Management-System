using EmployeeCRUD.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Microsoft.Data.SqlClient;

namespace EmployeeCRUD.IntegrationTests
{
    public class EmployeeCRUDWebApplicationFactory:WebApplicationFactory<Program>
    {
       protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll(typeof(DbContextOptions<AppDbContext>));

                services.AddDbContext<AppDbContext>(options =>
                {
                    options.UseSqlServer("Server=.;Database=EmployeeCRUD_Test;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True"
                    );
                });

                services.RemoveAll<IDbConnection>();
                services.AddScoped<IDbConnection>(sp =>
                    new SqlConnection("Server=.;Database=EmployeeCRUD_Test;Trusted_Connection=True;TrustServerCertificate=True"));

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureDeleted();
                db.Database.Migrate();
            });
        }
    }
}
