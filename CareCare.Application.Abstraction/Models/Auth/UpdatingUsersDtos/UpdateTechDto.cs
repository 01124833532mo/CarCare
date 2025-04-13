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

		public string? NationalId { get; set; }



		public required string Email { get; set; }
	}
}
