﻿using CarCare.Core.Domain.Contracts.Persistence;
using CarCare.Core.Domain.Contracts.Persistence.DbInitializers;
using CarCare.Core.Domain.Contracts.Persistence.ServiceRequests;
using CarCare.Core.Domain.Contracts.Persistence.Vehicles;
using CarCare.Infrastructure.Persistence._Data;
using CarCare.Infrastructure.Persistence._Data.Interceptors;
using CarCare.Infrastructure.Persistence.Generic_Repository;
using CarCare.Infrastructure.Persistence.Repositories.ServiceRequests;
using CarCare.Infrastructure.Persistence.Repositories.Vehicles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using UniteOfWork = CarCare.Infrastructure.Persistence.UnitOfWork;

namespace CarCare.Infrastructure.Persistence
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddPersistenceServices(this IServiceCollection services, IConfiguration configuration)
		{

			services.AddDbContext<CarCarIdentityDbContext>((provider, options) =>
			{
				options.UseLazyLoadingProxies()
				.UseSqlServer(configuration.GetConnectionString("IdentityContext"))
				.AddInterceptors(provider.GetRequiredService<AuditInterceptor>(),
								 provider.GetRequiredService<SettedUserIdInterceptor>());
			});
			services.AddScoped(typeof(ICarCareIdentityDbInitializer), typeof(CarCareIdentityDbInitializer));




			services.AddScoped(typeof(AuditInterceptor));
			services.AddScoped(typeof(SettedUserIdInterceptor));



			services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
			services.AddScoped(typeof(IVehicleRepository), typeof(VehicleRepository));
			services.AddScoped(typeof(IServiceRequestRepository), typeof(ServiceRequestRepository));
			services.AddScoped(typeof(IUnitOfWork), typeof(UniteOfWork.UnitOfWork));

			return services;
		}
	}
}
