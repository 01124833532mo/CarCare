using CarCare.Core.Domain.Entities.Identity;
using CarCare.Infrastructure.Persistence._Identity;
using Microsoft.AspNetCore.Identity;

namespace CarCare.Apis.Extinsions
{
	public static class IdentityExtension
	{
		public static IServiceCollection AddIdentityServices(this IServiceCollection services)
		{
			services.AddIdentity<ApplicationUser, IdentityRole>((identityOptions) =>
			{
				//identityOptions.SignIn.RequireConfirmedPhoneNumber = true;

				identityOptions.Lockout.AllowedForNewUsers = true;
				identityOptions.Lockout.MaxFailedAccessAttempts = 5;
				identityOptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromHours(5);
			})
				.AddEntityFrameworkStores<CarCarIdentityDbContext>();

			return services;
		}
	}
}
