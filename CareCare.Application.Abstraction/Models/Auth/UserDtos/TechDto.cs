namespace CareCare.Core.Application.Abstraction.Models.Auth.UserDtos
{
    public class TechDto : BaseUserDto
    {
        public required string NationalId { get; set; }
        public string? ServiceName { get; set; }

        public double TechLatitude { get; set; }

        public double TechLongitude { get; set; }
    }
}
