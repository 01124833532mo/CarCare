using System.ComponentModel.DataAnnotations;

namespace CareCare.Core.Application.Abstraction.Models.Vehicles
{
	public class CreateVehicleDto
	{
		public required string Model { get; set; }
		public required string Color { get; set; }
		public required int Year { get; set; }
		public required string VIN_Number { get; set; }
		public required string PlateNumber { get; set; }

	}
}
