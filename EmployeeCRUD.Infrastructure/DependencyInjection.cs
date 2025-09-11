
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Infrastructure.Data;
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
            services.AddDbContext<IAppDbContext, AppDbContext>(options =>
             options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
            );
            //services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            return services;
        }
    }
}
