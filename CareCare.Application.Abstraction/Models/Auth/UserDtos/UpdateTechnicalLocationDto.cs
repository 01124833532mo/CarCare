using System.ComponentModel.DataAnnotations;

namespace CareCare.Core.Application.Abstraction.Models.Auth.UserDtos
{
    public class UpdateTechnicalLocationDto
    {


        public string? Id { get; set; }

        public double? TechLatitude { get; set; }

        public double? TechLongitude { get; set; }
    }
}
