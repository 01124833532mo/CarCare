using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Models.Auth.ForgetPassword
{
	public class ConfirmationPhoneCodeDto
	{
		[Required]
		public required string PhoneNumber { get; set; }

		[Required]
		public int ConfirmationCode { get; set; }
	}
}
