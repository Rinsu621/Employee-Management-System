using EmployeeCRUD.Application.Command.Employees;
using EmployeeCRUD.Application.Dtos.Employees;
using EmployeeCRUD.Application.Interfaces;
using EmployeeCRUD.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeCRUD.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationDI(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddEmployeeHandler).Assembly));

            services.AddValidatorsFromAssembly(typeof(EmployeeDto).Assembly);


            // Register application services here  
            return services;
        }
    }
}
