using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Models.Auth._Common
{
	public class SuccessDto
	{
		public required string Status { get; set; }
		public required string Message { get; set; }

	}
}
