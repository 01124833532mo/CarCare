using CarCare.Core.Domain.Entities.Vehicles;
using CarCare.Infrastructure.Persistence._Data.Config.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarCare.Infrastructure.Persistence._Data.Config.Data
{
    internal class VehicleConfigurations : BaseAuditableEntityConfigurations<Vehicle, int>
    {
        public override void Configure(EntityTypeBuilder<Vehicle> builder)
        {
            base.Configure(builder);

            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).UseIdentityColumn(1, 1);

            builder.Property(p => p.CarNumber).IsRequired();
            builder.HasIndex(p => p.CarNumber).IsUnique();

            builder.Property(p => p.Color)
                    .HasColumnType("nvarchar")
                    .HasMaxLength(50);
            builder.Property(p => p.Model)
                    .HasColumnType("nvarchar")
                    .HasMaxLength(50);

            builder.HasIndex(p => p.CarPlate).IsUnique();
            builder.Property(p => p.CarPlate)
                    .HasColumnType("nvarchar")
                    .HasMaxLength(50);

            builder.HasOne(p => p.User)
                    .WithMany(p => p.Vehicles)
                    .HasForeignKey(p => p.UserId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Cascade);



        }
    }
}
