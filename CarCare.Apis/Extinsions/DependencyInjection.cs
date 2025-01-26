using LinkDev.Talabat.APIs.Services;
using LinkDev.Talabat.Core.Application.Abstraction;

namespace CarCare.Apis.Extinsions
{
    public static class DependencyInjection
    {

        public static IServiceCollection RegesteredPresestantLayer(this IServiceCollection services)
        {
            services.AddScoped(typeof(ILoggedInUserService), typeof(LoggedInUserService));
            return services;
        }

    }
}
