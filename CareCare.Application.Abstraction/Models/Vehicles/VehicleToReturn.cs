namespace CareCare.Core.Application.Abstraction.Models.Vehicles
{
    public class VehicleToReturn
    {
        public required string Model { get; set; }
        public required string Color { get; set; }
        public required int Year { get; set; }
        public required string VIN_Number { get; set; }
        public required string PlateNumber { get; set; }

        public required string UserId { get; set; }

        public required string FullName { get; set; }


        public string CreatedBy { get; set; } = null!;

        public DateTime CreatedOn { get; set; }

        public string LastModifiedBy { get; set; } = null!;


        public DateTime LastModifiedOn { get; set; }

    }
}
