namespace CareCare.Core.Application.Abstraction.Models.Auth.UserDtos
{
    public class TechDto : BaseUserDto
    {
        public required string NationalId { get; set; }
        public string? ServiceName { get; set; }

        public double TechLatitude { get; set; }

        public double Profit { get; set; }
        public double TechLongitude { get; set; }
        public decimal TechRate { get; set; }

        public int? CompletedRequestes { get; set; }
    }
}
