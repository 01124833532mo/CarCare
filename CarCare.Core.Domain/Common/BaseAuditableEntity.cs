using CarCare.Core.Domain.Contracts.Common;

namespace CarCare.Core.Domain.Common
{
    public abstract class BaseAuditableEntity<TKey> : BaseEntity<TKey>, IBaseAuditableEntity where TKey : IEquatable<TKey>
    {
        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public string LastModifiedBy { get; set; } = null!;


        public DateTime LastModifiedOn { get; set; }
    }
}
