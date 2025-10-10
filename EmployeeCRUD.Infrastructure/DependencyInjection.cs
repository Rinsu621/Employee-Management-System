using DinkToPdf;
using DinkToPdf.Contracts;
using EmployeeCRUD.Application.Interface;
using EmployeeCRUD.Domain.Entities;
using EmployeeCRUD.Infrastructure.Data;
using EmployeeCRUD.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection; // Add this line
using System.Data;

namespace EmployeeCRUD.Infrastructure
{
    public static class DependencyInjection
    {
        private static CustomAssemblyLoadContext? _loadContext;
        public interface IEmployeeDbConnection : IDbConnection { }
        public interface ISalaryDbConnection : IDbConnection { }

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

            services.AddScoped<IDbConnection>(sp =>
                new SqlConnection(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IJobTestServices, JobTestService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IExcelExpoter, ExportEmployeesToExcelService>();

            if (_loadContext == null)
            {
                _loadContext = new CustomAssemblyLoadContext();
                var dllPath = Path.Combine(AppContext.BaseDirectory, "lib", "libwkhtmltox.dll");
                _loadContext.LoadUnmanagedLibrary(dllPath);
            }
            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));
            services.AddScoped<IPdfService, PdfService>();
            return services;
        }
    }
    public class CustomAssemblyLoadContext : System.Runtime.Loader.AssemblyLoadContext
    {
        public IntPtr LoadUnmanagedLibrary(string absolutePath)
        {
            return LoadUnmanagedDll(absolutePath);
        }

        protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
        {
            return LoadUnmanagedDllFromPath(unmanagedDllName);
        }

        protected override System.Reflection.Assembly Load(System.Reflection.AssemblyName assemblyName)
        {
            return null;
        }
    }
}