namespace CareCare.Core.Application.Abstraction.Models.ServiceRequest.UserRequests
{
	public class	ReturnTechRequestDto
	{
		public required string Id { get; set; }
		public required string FullName { get; set; }

		public required string PhoneNumber { get; set; }
		public required string Email { get; set; }
		public required string Type { get; set; }
		public required string NationalId { get; set; }
		public string? ServiceName { get; set; }
		public double? Distance { get; set; }

		public decimal Rate { get; set; }
	}
}
