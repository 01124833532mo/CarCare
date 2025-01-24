using CarCare.Core.Domain.Entities.ServiceTypes;
using CarCare.Infrastructure.Persistence._Data.Config.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarCare.Infrastructure.Persistence._Data.Config.Data
{
    internal class ServiceTypeConfigurations : BaseEntityConfigurations<ServiceType, int>
    {
        public override void Configure(EntityTypeBuilder<ServiceType> builder)
        {
            base.Configure(builder);
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).UseIdentityColumn();

            builder.Property(p => p.Name)
               .HasColumnType("nvarchar")
               .HasMaxLength(50);

            builder.Property(p => p.Description)
               .HasColumnType("nvarchar")
               .HasMaxLength(500);

            builder.Property(p => p.Price)
                   .HasColumnType("decimal(7,2)");

            builder.HasMany(p => p.Technicals)
                    .WithOne(p => p.ServiceType)
                    .HasForeignKey(p => p.ServiceId)
                    .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
