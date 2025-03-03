using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Models.Auth.ForgetPassword
{
	public class ForgetPasswordByPhoneDto
	{
		[Required]
		public required string PhoneNumber { get; set; }
	}
}
