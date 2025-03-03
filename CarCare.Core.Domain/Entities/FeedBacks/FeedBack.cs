using CarCare.Core.Domain.Common;
using CarCare.Core.Domain.Entities.Common;
using CarCare.Core.Domain.Entities.Identity;

namespace CarCare.Core.Domain.Entities.FeedBacks
{
    public class FeedBack : BaseAuditableEntity<int>, IBaseUserId
    {
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public string? Comment { get; set; }

        public decimal Rating { get; set; }

        public required string UserId { get; set; }
        public virtual required ApplicationUser User { get; set; }
    }
}
