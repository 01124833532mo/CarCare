using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CareCare.Core.Application.Abstraction.Models.ServiceTypes
{
    public class ServiceTypeDto
    {
        [Required(ErrorMessage = "Name Must Be Required ")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Description Must Be Required ")]

        public required string Description { get; set; }


        public IFormFile? PictureUrl { get; set; }
    }
}
