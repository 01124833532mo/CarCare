using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Models.Auth.ForgetPassword
{
	public class SMSDto
	{
		public required string PhoneNumber { get; set; }

		public required string Body { get; set; }
	}
}
