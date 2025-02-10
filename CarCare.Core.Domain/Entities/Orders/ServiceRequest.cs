using CarCare.Core.Domain.Common;
using CarCare.Core.Domain.Entities.Common;
using CarCare.Core.Domain.Entities.Identity;
using CarCare.Core.Domain.Entities.Orders.ServicesDetails;
using CarCare.Core.Domain.Entities.ServiceTypes;

namespace CarCare.Core.Domain.Entities.Orders
{
	public class ServiceRequest : BaseAuditableEntity<int>, IBaseUserId
	{
		public required string UserId { get; set; }
		public virtual required ApplicationUser User { get; set; }

		public string TechId { get; set; }
		public virtual required ApplicationUser Technical { get; set; }

		public int ServiceTypeId { get; set; }
		public virtual required ServiceType ServiceType { get; set; }

		public Status Status { get; set; } = Status.Pending;


		public TireSize? TireSize { get; set; }
		public int? TireCount { get; set; }


		public TypeOfFuel? TypeOfFuel { get; set; }
		public int? LitersOfFuel { get; set; }


		public BettaryType? BettaryType { get; set; }


		public TypeOfOil? TypeOfOil { get; set; }
		public int? LitersOfOil { get; set; }

		public TypeOfWinch? TypeOfWinch { get; set; }

		public decimal ServicePrice { get; set; }
		public int BasePrice { get; set; } = 5;
		public string? PaymentIntentId { get; set; }

	}
}
