using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Models.Auth
{
	public class LoginDto
	{
		[Required]
		[Phone]
		public required string PhoneNumber { get; set; }

		[Required]
		public required string Password { get; set; }

	}
}
