namespace CareCare.Core.Application.Abstraction.Models.ServiceRequest.UserRequests
{
    public class ReturnRequestDto
    {
        public int Id { get; set; }

        public required string TechId { get; set; }

        public required string TechName { get; set; }
        public required string TechJop { get; set; }

        public double Distance { get; set; }

        public int ServiceTypeId { get; set; }


        public string? BettaryType { get; set; }

        public string? TypeOfFuel { get; set; }

        public string? TypeOfOil { get; set; }

        public string? TireSize { get; set; }

        public string? TypeOfWinch { get; set; }

        public int? ServiceQuantity { get; set; }

        public decimal ServicePrice { get; set; }
        public required string UserId { get; set; }
        public required string UserName { get; set; }
        public double UserLatitude { get; set; }
        public double UserLongitude { get; set; }

        public required string BusnissStatus { get; set; }
        public required string PaymentStatus { get; set; }
        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public string? PaymentIntentId { get; set; }
        public string? ClientSecret { get; set; }
    }
}
