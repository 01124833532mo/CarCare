namespace CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Technicals
{
    public class TechViewModel
    {
        public required string Id { get; set; }
        public required string FullName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Type { get; set; }

        public required string Email { get; set; }
        public required string NationalId { get; set; }
        public string? ServiceName { get; set; }

        public decimal? TechRate { get; set; }



        public IEnumerable<string> Roles { get; set; }

    }
}
