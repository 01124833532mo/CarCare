using System.ComponentModel.DataAnnotations;

namespace CareCare.Core.Application.Abstraction.Models.Auth._Common
{
    public class ChangePasswordDto
    {

        public required string CurrentPassword { get; set; }


        public required string NewPassword { get; set; }
    }
}
