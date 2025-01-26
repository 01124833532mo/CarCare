using CarCare.Core.Domain.Entities.Identity;
using CareCare.Core.Application.Abstraction.Models.Auth.UserDtos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Models.Auth.UpdatingUsersDtos
{
	public class BaseUpdateDto 
	{
		public required string UserName { get; set; }
		public required string PhoneNumber { get; set; }
		public string? Address { get; set; }
	}
}
