using CarCare.Core.Domain.Entities.FeedBacks;
using CarCare.Core.Domain.Entities.Identity;
using CarCare.Core.Domain.Entities.Orders;
using CarCare.Core.Domain.Entities.ServiceTypes;
using CarCare.Core.Domain.Entities.Vehicles;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CarCare.Infrastructure.Persistence._Data
{
	public class CarCarIdentityDbContext : IdentityDbContext<ApplicationUser>
	{


		public CarCarIdentityDbContext(DbContextOptions<CarCarIdentityDbContext> options)
			: base(options)
		{

		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemblyInformtion).Assembly);
		}

		public DbSet<FeedBack> FeedBacks { get; set; }

		public DbSet<ServiceType> ServiceTypes { get; set; }

		public DbSet<Vehicle> Vehicles { get; set; }

		public DbSet<ServiceRequest> ServiceRequests { get; set; }


	}
}
