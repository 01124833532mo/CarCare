using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Models.Auth.UpdatingUsersDtos
{
	public class UpdateUserDto : BaseUpdateDto
	{
		public required string Email { get; set; }
	}
}
