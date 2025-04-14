using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Models.Auth.ForgetPasswordByEmailDtos
{
	public class SendCodeByEmailDto
	{
		[Required]
		public required string Email { get; set; }
	}
}
