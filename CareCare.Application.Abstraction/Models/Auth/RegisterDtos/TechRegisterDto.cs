using System.ComponentModel.DataAnnotations;

namespace CareCare.Core.Application.Abstraction.Models.Auth.RegisterDtos
{
    public class TechRegisterDto : BaseRegisterDto
    {




        [Required]
        [Length(14, 14)]
        public required string NationalId { get; set; }

        [Required(ErrorMessage = "Service Id Must Be Required")]
        public int ServiceId { get; set; }


    }
}
