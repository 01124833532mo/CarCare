using CarCare.Core.Domain.Entities.FeedBacks;
using CarCare.Infrastructure.Persistence._Data.Config.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarCare.Infrastructure.Persistence._Data.Config.Data
{
    internal class FeedBackConfigurations : BaseAuditableEntityConfigurations<FeedBack, int>
    {
        public override void Configure(EntityTypeBuilder<FeedBack> builder)
        {
            base.Configure(builder);
            builder.HasKey(e => e.Id);
            builder.Property(p => p.Id).UseIdentityColumn();

            builder.Property(p => p.Comment)
                  .HasColumnType("nvarchar")
                  .HasMaxLength(200);

            builder.Property(p => p.Rating)
                    .HasColumnType("decimal(2,1)");

            builder.HasOne(p => p.User)
                    .WithMany(p => p.FeedBacks)
                    .HasForeignKey(p => p.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
