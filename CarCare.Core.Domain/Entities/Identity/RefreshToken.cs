using Microsoft.EntityFrameworkCore;

namespace CarCare.Core.Domain.Entities.Identity
{
    [Owned]

    public class RefreshToken
    {
        public required string Token { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ExpireOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpireOn;
        public bool IsActice => RevokedOn is null && !IsExpired;
    }
}
