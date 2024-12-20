using CarCare.Core.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Models.Auth.RegisterDtos
{
	public class TechRegisterDto : BaseRegisterDto
	{

		[Required]
		[EmailAddress]
		public required string Email { get; set; }


		[Required]
		[Length(14, 14)]
		public required string NationalId { get; set; }

		[Required]
		public required Specialization Specialization { get; set; }
	}
}
