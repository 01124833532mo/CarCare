using CarCare.Core.Application.Mapping;
using CarCare.Core.Application.Services.Auth;
using CarCare.Core.Application.Services.Auth.SendServices;
using CarCare.Shared.AppSettings;
using CareCare.Core.Application.Abstraction;
using CareCare.Core.Application.Abstraction.Services;
using CareCare.Core.Application.Abstraction.Services.Auth;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

			services.Configure<SMSSettings>(configuration.GetSection("SMSSettings"));
			services.AddTransient(typeof(ISMSServices), typeof(SMSServices));

			services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
			services.AddTransient(typeof(IEmailServices), typeof(EmailService));

			services.AddAutoMapper(typeof(MappingProfile));

			return services;
		}
	}
}
