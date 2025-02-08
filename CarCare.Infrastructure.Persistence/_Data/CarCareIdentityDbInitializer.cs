using CarCare.Core.Domain.Contracts.Persistence.DbInitializers;
using CarCare.Core.Domain.Entities.Identity;
using CarCare.Core.Domain.Entities.ServiceTypes;
using CarCare.Shared.Models.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace CarCare.Infrastructure.Persistence._Data
{
    public class CarCareIdentityDbInitializer(CarCarIdentityDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager) : ICarCareIdentityDbInitializer
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
            if (!dbContext.Roles.Any())
            {

                var roles = new[] { Roles.User, Roles.Technical, Roles.Admin };

                foreach (var role in roles)
                {

                    await roleManager.CreateAsync(new IdentityRole(role));

                }



            }
            if (!dbContext.Users.Any())
            {
                var user = new ApplicationUser
                {
                    FullName = "Mahmoud.Ahmed",
                    UserName = "Mahmoud.Ahmed@gmail.com",
                    Email = "Mahmoud.Ahmed@gmail.com",
                    PhoneNumber = "01029442023",
                    Type = Types.Technical,
                    NationalId = "12345678909876",

                };

                await userManager.CreateAsync(user, "01124833532");
                await userManager.AddToRoleAsync(user, Roles.Admin);
            }
            if (!dbContext.ServiceTypes.Any())
            {
                var ServiceTypesData = await File.ReadAllTextAsync("../CarCare.Infrastructure.Persistence/_Data/Seeds/ServiceTypes.json");

                var servicetypes = JsonSerializer.Deserialize<List<ServiceType>>(ServiceTypesData);

                if (servicetypes?.Count > 0)
                {
                    await dbContext.Set<ServiceType>().AddRangeAsync(servicetypes);
                    await dbContext.SaveChangesAsync();

                }
            }
        }
    }
}
