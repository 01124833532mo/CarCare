using System.ComponentModel.DataAnnotations;

namespace CareCare.Core.Application.Abstraction.Models.Auth._Common
{
    public class ChangePasswordDto
    {
        [Required(ErrorMessage = "Current Password Must Be Required")]
        public required string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New Password Must Be Required")]
        public required string NewPassword { get; set; }
    }
}
