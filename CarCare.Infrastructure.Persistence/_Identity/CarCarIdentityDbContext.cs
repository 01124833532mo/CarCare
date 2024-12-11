using CarCare.Core.Domain.Entities.Identity;
using CarCare.Infrastructure.Persistence._Identity.Config;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Infrastructure.Persistence._Identity
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

			modelBuilder.ApplyConfiguration(new ApplicationUserConfigurations());
		}
	}
}
