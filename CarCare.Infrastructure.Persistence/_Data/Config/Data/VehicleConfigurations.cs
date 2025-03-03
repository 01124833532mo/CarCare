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

            builder.Property(p => p.PlateNumber).IsRequired();
            builder.HasIndex(p => p.PlateNumber).IsUnique();

            builder.Property(p => p.Color)
                    .HasColumnType("nvarchar")
                    .HasMaxLength(50);
            builder.Property(p => p.Model)
                    .HasColumnType("nvarchar")
                    .HasMaxLength(50);

            builder.HasIndex(p => p.VIN_Number).IsUnique();
            builder.Property(p => p.VIN_Number)
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
