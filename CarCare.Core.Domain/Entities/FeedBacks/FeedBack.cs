using CarCare.Core.Domain.Common;
using CarCare.Core.Domain.Entities.Identity;

namespace CarCare.Core.Domain.Entities.FeedBacks
{
    public class FeedBack : BaseAuditableEntity<int>
    {
        public DateTime Date { get; set; } = DateTime.UtcNow;

        public required string Comment { get; set; }

        public decimal Rating { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
