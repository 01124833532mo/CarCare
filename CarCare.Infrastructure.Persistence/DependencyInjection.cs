using CarCare.Core.Domain.Contracts.Persistence.DbInitializers;
using CarCare.Infrastructure.Persistence._Identity;
using Microsoft.EntityFrameworkCore;
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

			return services;
		}
	}
}
