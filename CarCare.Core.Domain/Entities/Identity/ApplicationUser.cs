using CarCare.Core.Domain.Entities.FeedBacks;
using CarCare.Core.Domain.Entities.ServiceTypes;
using CarCare.Core.Domain.Entities.Vehicles;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CarCare.Core.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime? BirthDate { get; set; }
        public string? Address { get; set; }

        [Length(14, 14)]
        public string? NationalId { get; set; }
        public Types Type { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

        public int? PhoneConfirmResetCode { get; set; }
        public DateTime? PhoneConfirmResetCodeExpiry { get; set; }


        public virtual ICollection<FeedBack>? FeedBacks { get; set; } = new HashSet<FeedBack>();
        public virtual ICollection<Vehicle>? Vehicles { get; set; } = new HashSet<Vehicle>();

        public int? ServiceId { get; set; }
        public virtual ServiceType? ServiceType { get; set; }
    }
}
