using CarCare.Core.Domain.Contracts.Persistence.DbInitializers;
using CarCare.Core.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Infrastructure.Persistence._Identity
{
	public class CarCareIdentityDbInitializer(CarCarIdentityDbContext dbContext, UserManager<ApplicationUser> userManager) : ICarCareIdentityDbInitializer
	{
		public async Task InitializeAsync()
		{
			var penddingMigrations = await dbContext.Database.GetPendingMigrationsAsync();

			if (penddingMigrations.Any())
			{
				await dbContext.Database.MigrateAsync();
			}
		}

		public async Task SeedAsync()
		{
			if (!dbContext.Users.Any())
			{
				var user = new ApplicationUser
				{
					Address = "Al3abody",
					BirthDate = new DateTime(),
					UserName = "Mahmoud.Ahmed",
					Email = "Mahmoud.Ahmed@gmail.com",
					PhoneNumber = "01029442023",
					Type = Types.Technical,
					NationalId = "12345678909876",
					
				};

				await userManager.CreateAsync(user, "P@ssw0rd");
			}
		}
	}
}
