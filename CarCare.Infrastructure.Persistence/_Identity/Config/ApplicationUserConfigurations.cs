using CarCare.Core.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Infrastructure.Persistence._Identity.Config
{
	internal class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder.Property(p => p.Id)
				.ValueGeneratedOnAdd();

			builder.Property(user => user.Type)
				.HasConversion
				(
				(UStatus) => UStatus.ToString(),
				(UStatus) => (Types)Enum.Parse(typeof(Types), UStatus)
				);

			builder.Property(user => user.Specialization)
				.HasConversion
				(
				(UStatus) => UStatus.ToString(),
				(UStatus) => (Specialization)Enum.Parse(typeof(Specialization), UStatus)
				);

		}
	}
}
