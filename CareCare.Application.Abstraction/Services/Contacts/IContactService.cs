using CareCare.Core.Application.Abstraction.Models.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CareCare.Core.Application.Abstraction.Services.Contacts
{
	public interface IContactService
	{
		Task<ReturnContactDto> CreateContactAsync(CreateContactDto contactDto);
		Task<IEnumerable<ReturnContactDto>> GetAllContactsAsync(ClaimsPrincipal claimsPrincipal);
		Task<ReturnContactDto> UpdateContactAsync(int id, CreateContactDto contactDto);
		Task<string> DeleteContactAsync(int id);

	}
}
