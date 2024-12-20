using CarCare.Core.Application.Mapping;
using CarCare.Core.Application.Services;
using CareCare.Core.Application.Abstraction;
using CareCare.Core.Application.Abstraction.Services.Auth;
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
		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{


			services.AddScoped(typeof(IServiceManager), typeof(ServiceManger));
			services.AddScoped(typeof(IAuthService), typeof(AuthService));
			services.AddScoped(typeof(Func<IAuthService>), (serviceprovider) =>
			{
				return () => serviceprovider.GetRequiredService<IAuthService>();
			});

			services.AddAutoMapper(typeof(MappingProfile));

			return services;
		}
	}
}
