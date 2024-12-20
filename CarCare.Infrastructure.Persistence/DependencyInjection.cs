using CarCare.Core.Domain.Contracts.Persistence.DbInitializers;
using CarCare.Infrastructure.Persistence._Identity;
using CarCare.Infrastructure.Persistence._Identity.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Infrastructure.Persistence
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
		{

			services.AddDbContext<CarCarIdentityDbContext>((options) =>
			{
				options.UseSqlServer(configuration.GetConnectionString("IdentityContext"));
			});

			services.AddScoped(typeof(ICarCareIdentityDbInitializer), typeof(CarCareIdentityDbInitializer));

			services.AddScoped(typeof(ISaveChangesInterceptor), typeof(IdentityInterceptor));

			return services;
		}
	}
}
