using CarCare.Core.Domain.Entities.Contacts;
using CarCare.Core.Domain.Entities.FeedBacks;
using CarCare.Core.Domain.Entities.Orders;
using CarCare.Core.Domain.Entities.ServiceTypes;
using CarCare.Core.Domain.Entities.Vehicles;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CarCare.Core.Domain.Entities.Identity
{
	public class ApplicationUser : IdentityUser
	{
		public required string FullName { get; set; }

		public bool? IsActive { get; set; }
		public decimal? TechRate { get; set; }
		public double? TechLatitude { get; set; }
		public double? TechLongitude { get; set; }


		[Length(14, 14)]
		public string? NationalId { get; set; }
		public Types Type { get; set; }
		public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

		public int? PhoneConfirmResetCode { get; set; }
		public DateTime? PhoneConfirmResetCodeExpiry { get; set; }

		public int? EmailConfirmResetCode { get; set; }
		public DateTime? EmailConfirmResetCodeExpiry { get; set; }

		public virtual ICollection<Contact>? Contacts { get; set; } = new HashSet<Contact>();

		public virtual ICollection<FeedBack>? FeedBacks { get; set; } = new HashSet<FeedBack>();
		public virtual ICollection<Vehicle>? Vehicles { get; set; } = new HashSet<Vehicle>();

		public virtual ICollection<ServiceRequest> UserServiceRequests { get; set; } = new HashSet<ServiceRequest>();

		public virtual ICollection<ServiceRequest> TechServiceRequests { get; set; } = new HashSet<ServiceRequest>();

		public int? ServiceId { get; set; }
		public virtual ServiceType? ServiceType { get; set; }
	}
}
