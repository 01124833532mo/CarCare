using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Models.Auth.ForgetPasswordByEmailDtos
{
	public class EmailDto
	{

		public required string To { get; set; }
		public required string Subject { get; set; }
		public required string Body { get; set; }

	}
}
