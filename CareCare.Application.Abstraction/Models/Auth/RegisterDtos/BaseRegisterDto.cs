using CarCare.Core.Domain.Entities.Identity;

namespace CareCare.Core.Application.Abstraction.Models.Auth.RegisterDtos
{
    public class BaseRegisterDto
    {


        public required string Email { get; set; }


        public required string FullName { get; set; }

        public required string PhoneNumber { get; set; }


        public required string Password { get; set; }
        public required Types Type { get; set; }
    }
}
