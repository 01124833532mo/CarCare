using CarCare.Core.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Models.Auth.UserDtos
{
	public class TechDto : BaseUserDto
	{
		public required string Email { get; set; }
		public required string NationalId { get; set; }
		public required string Specialization { get; set; }
	}
}
