using CarCare.Core.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace CareCare.Core.Application.Abstraction.Models.Auth.DashBoardDto.Users
{
    public class CreateUserDro
    {
        [Required]
        public required string Name { get; set; }



        [Required]
        public required string PhoneNumber { get; set; }

        //[RegularExpression("(?=^.{6,10}$)(?=.\\d)(?=.[a-z])(?=.[A-Z])(?=.[!@#%^&amp;()_+}{&quot;:;'?/&gt;.&lt;,])(?!.\\s).*$",
        //					ErrorMessage = "Password must have 1 UpperCase,1 LowerCase,1 number , 1 non alphanumberic and at least 6 characters ")]

        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        public required string Password { get; set; }
        public Types Type = Types.User;
    }
}
