using CarCare.Core.Domain.Common;
using CarCare.Core.Domain.Entities.Identity;
using CarCare.Core.Domain.Entities.Orders;

namespace CarCare.Core.Domain.Entities.ServiceTypes
{
	public class ServiceType : BaseEntity<int>
	{
		public required string Name { get; set; }
		public required string NormalizedName { get; set; }


		public required string Description { get; set; }

		//public required decimal Price { get; set; }

		public string? PictureUrl { get; set; }

		public virtual ICollection<ApplicationUser>? Technicals { get; set; } = new HashSet<ApplicationUser>();
		public virtual ICollection<ServiceRequest> ServiceRequests { get; set; } = new HashSet<ServiceRequest>();
	}
}
