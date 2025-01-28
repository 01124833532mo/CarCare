using CarCare.Core.Domain.Common;
using CarCare.Core.Domain.Entities.Identity;

namespace CarCare.Core.Domain.Entities.ServiceTypes
{
    public class ServiceType : BaseEntity<int>
    {
        public required string Name { get; set; }

        public required string Description { get; set; }

        //public required decimal Price { get; set; }

        public string? Image { get; set; }

        public virtual ICollection<ApplicationUser>? Technicals { get; set; } = new HashSet<ApplicationUser>();
    }
}
