using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace CareCare.Core.Application.Abstraction.Models.ServiceTypes
{
    public class ServiceTypeDto
    {
        public required string Name { get; set; }


        public required string Description { get; set; }


        public IFormFile? PictureUrl { get; set; }
    }
}
