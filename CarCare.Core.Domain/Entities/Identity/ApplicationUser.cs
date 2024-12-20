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
		public DateTime? BirthDate { get; set; }
		public string? Address { get; set; }
		public Specialization? Specialization { get; set; }

		[Length(14, 14)]
		public string? NationalId { get; set; }
		public required string Name { get; set; }
		public Types Type { get; set; }

	}
}
