using CarCare.Core.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Models.Auth.RegisterDtos
{
	public class BaseRegisterDto
	{
		[Required]
		public required string UserName { get; set; }

		[Required]
		public required string PhoneNumber { get; set; }

		[Required]
		//[RegularExpression("(?=^.{6,10}$)(?=.\\d)(?=.[a-z])(?=.[A-Z])(?=.[!@#%^&amp;()_+}{&quot;:;'?/&gt;.&lt;,])(?!.\\s).*$",
		//					ErrorMessage = "Password must have 1 UpperCase,1 LowerCase,1 number , 1 non alphanumberic and at least 6 characters ")]
		public required string Password { get; set; }
		public required Types Type { get; set; }
	}
}
