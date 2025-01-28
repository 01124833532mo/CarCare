using CarCare.Core.Application.Mapping;
using CarCare.Core.Application.Services.Auth;
using CarCare.Core.Application.Services.Auth.SendServices;
using CarCare.Core.Application.Services.Vehicles;
using CarCare.Shared.AppSettings;
using CareCare.Core.Application.Abstraction;
using CareCare.Core.Application.Abstraction.Services;
using CareCare.Core.Application.Abstraction.Services.Auth;
using CareCare.Core.Application.Abstraction.Services.Vehicles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CarCare.Core.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {


            services.AddScoped(typeof(IServiceManager), typeof(ServiceManger));
            services.AddScoped(typeof(IAuthService), typeof(AuthService));
            services.AddScoped(typeof(Func<IAuthService>), (serviceprovider) =>
            {
                return () => serviceprovider.GetRequiredService<IAuthService>();
            });
            services.AddScoped(typeof(IVehicleService), typeof(VehicleService));

            services.AddScoped(typeof(Func<IVehicleService>), (serviceprovider) =>
            {
                return () => serviceprovider.GetRequiredService<IVehicleService>();

            });

            services.Configure<SMSSettings>(configuration.GetSection("SMSSettings"));
            services.AddTransient(typeof(ISMSServices), typeof(SMSServices));

            services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
            services.AddTransient(typeof(IEmailServices), typeof(EmailService));

            services.AddAutoMapper(typeof(MappingProfile));

            return services;
        }
    }
}
