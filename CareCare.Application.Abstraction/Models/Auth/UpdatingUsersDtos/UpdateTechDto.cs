using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Models.Auth.UpdatingUsersDtos
{
	public class UpdateTechDto : BaseUpdateDto
	{
		[Length(14, 14)]
		public string? NationalId { get; set; }


		[EmailAddress]
		[DataType(DataType.EmailAddress)]
		public required string Email { get; set; }
	}
}
