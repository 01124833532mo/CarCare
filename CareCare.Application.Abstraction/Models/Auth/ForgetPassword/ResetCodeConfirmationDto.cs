using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Models.Auth.ForgetPassword
{
	public class ResetCodeConfirmationDto : ForgetPasswordDto
	{

		[Required]
		public required int ResetCode { get; set; }
	}
}
