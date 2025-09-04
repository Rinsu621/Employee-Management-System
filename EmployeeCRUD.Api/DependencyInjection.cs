using EmployeeCRUD.Application;
using EmployeeCRUD.Infrastructure;
namespace EmployeeCRUD.Api
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiDI(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationDI()
                .AddInfrastructureDI(configuration);

            return services;
        }
    }
}
