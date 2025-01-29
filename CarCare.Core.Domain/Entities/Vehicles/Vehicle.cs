using CarCare.Core.Domain.Common;
using CarCare.Core.Domain.Entities.Common;
using CarCare.Core.Domain.Entities.Identity;

namespace CarCare.Core.Domain.Entities.Vehicles
{
    public class Vehicle : BaseAuditableEntity<int>, IBaseUserId
    {
        public required string PlateNumber { get; set; } // 1234523 // unique
        public required string Color { get; set; }

        public required string Model { get; set; }

        public required int Year { get; set; }

        public required string VIN_Number { get; set; } // the number ==> 456سصر  // unique

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
