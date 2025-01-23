using CarCare.Core.Domain.Contracts.Persistence.DbInitializers;
using CarCare.Infrastructure.Persistence._Data;
using CarCare.Infrastructure.Persistence._Data.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarCare.Infrastructure.Persistence
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<CarCarIdentityDbContext>((options) =>
            {
                options.UseLazyLoadingProxies().UseSqlServer(configuration.GetConnectionString("IdentityContext"));
            });

            services.AddScoped(typeof(ICarCareIdentityDbInitializer), typeof(CarCareIdentityDbInitializer));

            services.AddScoped(typeof(ISaveChangesInterceptor), typeof(IdentityInterceptor));

            return services;
        }
    }
}
