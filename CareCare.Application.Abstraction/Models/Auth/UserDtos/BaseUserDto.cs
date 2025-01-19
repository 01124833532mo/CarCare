using CarCare.Core.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Models.Auth.UserDtos
{
	public class BaseUserDto
	{
		public required string Id { get; set; }
		public required string UserName { get; set; }
		public required string PhoneNumber { get; set; }
		public required string Type { get; set; }
		public required string Token { get; set; }
	}
}
