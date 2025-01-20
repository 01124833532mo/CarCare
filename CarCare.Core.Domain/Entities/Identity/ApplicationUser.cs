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
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();


    }
}
