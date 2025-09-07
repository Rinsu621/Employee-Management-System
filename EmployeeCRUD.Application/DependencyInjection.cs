using EmployeeCRUD.Application.Command.Employees;
using EmployeeCRUD.Application.Dtos.Employees;
using EmployeeCRUD.Application.Pipeline;
using EmployeeCRUD.Application.Validator;
using EmployeeCRUD.Infrastructure.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EmployeeCRUD.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddEmployeeSPHandler).Assembly));


            services.AddValidatorsFromAssembly(typeof(EmployeeDtoValidator).Assembly);


            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


            // Register application services here  
            return services;
        }
    }
}
