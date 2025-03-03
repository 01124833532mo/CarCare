using System.ComponentModel.DataAnnotations;

namespace CareCare.Core.Application.Abstraction.Models.Auth.UserDtos
{
    public class UpdateTechnicalLocationDto
    {

        [Required(ErrorMessage = "Technical Id Must Be Required ")]
        public string? Id { get; set; }
        [Required(ErrorMessage = "TechLatitude  Must Be Required ")]
        public double? TechLatitude { get; set; }
        [Required(ErrorMessage = "TechLongitude  Must Be Required ")]

        public double? TechLongitude { get; set; }
    }
}
