using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Core.Domain.Entities.Identity
{
	public class ApplicationUser : IdentityUser
	{
		public required DateTime BirthDate { get; set; }
		public required string Address { get; set; }
		public string? Specialization { get; set; }

		[Length(14, 14)]
		public string? CartNumber { get; set; }
		public required string Name { get; set; }
		public Type Type { get; set; }

	}
}
