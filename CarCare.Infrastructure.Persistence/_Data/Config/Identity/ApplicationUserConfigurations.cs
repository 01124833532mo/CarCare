using CarCare.Core.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarCare.Infrastructure.Persistence._Data.Config.Identity
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
			
			builder.Property(t => t.TechRate)
					.HasColumnType("decimal(2,1)");

			builder.Property(t => t.IsActive)
				.HasDefaultValue(true);
			
			// builder.HasIndex(u => u.UserName)
			//.IsUnique(false);


		}
	}
}
