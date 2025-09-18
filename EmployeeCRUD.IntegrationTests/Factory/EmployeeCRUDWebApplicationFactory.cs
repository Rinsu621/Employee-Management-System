using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.IntegrationTests.Factory
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
                services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());


                services.RemoveAll<IDbConnection>();
                //services.AddScoped<IDbConnection>(sp =>
                //    new SqlConnection("Server=.;Database=EmployeeCRUD_Test;Trusted_Connection=True;TrustServerCertificate=True"));

                services.AddScoped<IDbConnection>(sp =>
                    new SqlConnection(
                        "Server=.;Database=EmployeeCRUD_Test;Trusted_Connection=True;TrustServerCertificate=True"
                    )
                );


                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureDeleted();
                db.Database.Migrate();
            });
        }
    }
}
