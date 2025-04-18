using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Models.Contacts
{
	public class CreateContactDto
	{
		public required string Message { get; set; }
		public required int MessageFor { get; set; }

	}
}
