using EmployeeManagementSystem.Application;
using EmployeeManagementSystem.Infrastructure;
namespace EmployeeManagementSystem.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationDI(configuration)
                .AddInfrastructureDI(configuration);

            return services;
        }
    }
}
