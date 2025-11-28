using CarCare.Core.Domain.Entities.Orders;
using CarCare.Core.Domain.Entities.Orders.ServicesDetails;
using CarCare.Infrastructure.Persistence._Data.Config.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarCare.Infrastructure.Persistence._Data.Config.ServiceRequests
{
    internal class OrderConfigurations : BaseAuditableEntityConfigurations<ServiceRequest, int>
    {
        public override void Configure(EntityTypeBuilder<ServiceRequest> builder)
        {
            base.Configure(builder);

            builder.HasOne(s => s.User)
                .WithMany(u => u.UserServiceRequests)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(s => s.Technical)
                .WithMany(u => u.TechServiceRequests)
                .HasForeignKey(s => s.TechId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(s => s.ServiceType)
                .WithMany(u => u.ServiceRequests)
                .HasForeignKey(s => s.ServiceTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(s => s.PaymentStatus)
                .HasConversion(
                Sstatus => Sstatus.ToString(),
                (UStatus) => (PaymentStatus)Enum.Parse(typeof(PaymentStatus), UStatus)
                );
            builder.Property(s => s.BusnissStatus)
                .HasConversion(
                Sstatus => Sstatus.ToString(),
                (UStatus) => (BusnissStatus)Enum.Parse(typeof(BusnissStatus), UStatus)
                );

            builder.Property(s => s.BettaryType)
                .HasConversion(
                BettaryType => BettaryType.ToString(),
                (BettaryType) => (BettaryType)Enum.Parse(typeof(BettaryType), BettaryType)
                );

            builder.Property(s => s.TireSize)
                .HasConversion(
                TireSize => TireSize.ToString(),
                (TireSize) => (TireSize)Enum.Parse(typeof(TireSize), TireSize)
                );


            builder.Property(s => s.TypeOfFuel)
                .HasConversion(
                TypeOfFuel => TypeOfFuel.ToString(),
                (TypeOfFuel) => (TypeOfFuel)Enum.Parse(typeof(TypeOfFuel), TypeOfFuel)
                );

            builder.Property(s => s.TypeOfOil)
                .HasConversion(
                TypeOfOil => TypeOfOil.ToString(),
                (TypeOfOil) => (TypeOfOil)Enum.Parse(typeof(TypeOfOil), TypeOfOil)
                );

            builder.Property(s => s.TypeOfWinch)
                .HasConversion(
                TypeOfWinch => TypeOfWinch.ToString(),
                (TypeOfWinch) => (TypeOfWinch)Enum.Parse(typeof(TypeOfWinch), TypeOfWinch)
                );

        }
    }
}
