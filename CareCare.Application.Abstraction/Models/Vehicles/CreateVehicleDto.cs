using System.ComponentModel.DataAnnotations;

namespace CareCare.Core.Application.Abstraction.Models.Vehicles
{
    public class CreateVehicleDto
    {
        public required string Model { get; set; }
        public required string Color { get; set; }
        public required int Year { get; set; }
        [Required(ErrorMessage = "Vin Must Be Requried")]
        public required string VIN_Number { get; set; }
        [Required(ErrorMessage = "Plate Number Must Be Requried")]

        public required string PlateNumber { get; set; }

    }
}
