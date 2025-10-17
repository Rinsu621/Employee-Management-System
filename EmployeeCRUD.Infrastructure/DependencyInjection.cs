using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Infrastructure.Data;
using EmployeeCRUD.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; 
using System.Data;

namespace EmployeeCRUD.Infrastructure
{
    public static class DependencyInjection
    {
       

        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")),
                ServiceLifetime.Scoped);

            services.AddDbContext<SalaryDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("SalaryConnection")),
                ServiceLifetime.Scoped);

          


           
            services.AddScoped<IAppDbContext>(provider => provider.GetRequiredService<AppDbContext>());
            services.AddScoped<ISalaryDbContext>(provider => provider.GetRequiredService<SalaryDbContext>());

            //services.AddScoped<IEmployeeDbConnection>(sp =>
            //     (IEmployeeDbConnection)new DbConnectionService(configuration.GetConnectionString("DefaultConnection")));

            //services.AddScoped<ISalaryDbConnection>(sp =>
            //    (ISalaryDbConnection)new DbConnectionService(configuration.GetConnectionString("SalaryConnection")));

            services.AddScoped<IDbConnectionService, DbConnectionService>();




            services.AddScoped<IJobTestServices, JobTestService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<ISalaryEmailService, SalaryEmailService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IExcelExpoter, ExportEmployeesToExcelService>();
            services.AddScoped<IPdfService, PdfService>();

            return services;
        }
    }
  
}