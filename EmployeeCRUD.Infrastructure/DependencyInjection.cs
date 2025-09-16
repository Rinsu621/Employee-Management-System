
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Infrastructure.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EmployeeCRUD.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            // Register the concrete AppDbContext
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Map IAppDbContext to AppDbContext for DI
            services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());

            //Register Dapper
            services.AddScoped<IDbConnection>(sp =>
           new SqlConnection(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }
    }
}
