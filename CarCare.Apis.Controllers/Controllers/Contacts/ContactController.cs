using CarCare.Apis.Controllers.Controllers.Base;
using CareCare.Core.Application.Abstraction;
using CareCare.Core.Application.Abstraction.Models.Contacts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Apis.Controllers.Controllers.Contacts
{
	[Authorize]
	public class ContactController(IServiceManager serviceManager) : BaseApiController
	{
		[HttpGet("GetAllMessages")]
		public async Task<ActionResult<IEnumerable<ReturnContactDto>>> GetAllContacts()
		{
			var result = await serviceManager.ContactService.GetAllContactsAsync(User);
			return Ok(result);
		}
	}
}
