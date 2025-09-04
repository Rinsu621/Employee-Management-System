using EmployeeCRUD.Application.Interfaces;
using EmployeeCRUD.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
            services.AddDbContext<Data.AppDbContext>(optiond =>
            {
                optiond.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            return services;
        }
    }
}
