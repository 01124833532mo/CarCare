using AutoMapper;
using CarCare.Core.Domain.Contracts.Persistence;
using CarCare.Core.Domain.Entities.Contacts;
using CarCare.Core.Domain.Entities.Identity;
using CarCare.Shared.ErrorModoule.Exeptions;
using CarCare.Shared.Models.Roles;
using CareCare.Core.Application.Abstraction.Models.Contacts;
using CareCare.Core.Application.Abstraction.Services.Contacts;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;

namespace CarCare.Core.Application.Services.Contacts
{
	public class ContactService(IUnitOfWork _unitOfWork, IMapper _mapper, UserManager<ApplicationUser> _userManager) : IContactService
	{
		public async Task<ReturnContactDto> CreateContactAsync(CreateContactDto contactDto)
		{
			var repo = _unitOfWork.GetRepository<Contact, int>();

			var contact = _mapper.Map<Contact>(contactDto);

			try
			{
				await repo.AddAsync(contact);
			}
			catch (Exception ex)
			{
				throw new BadRequestExeption(ex.Message);

			}


			var created = await _unitOfWork.CompleteAsync() > 0;

			if (!created)
				throw new BadRequestExeption("Contact not Created!");

			var user = await _userManager.FindByIdAsync(contact.UserId);
			if (user is null)
				throw new NotFoundExeption(nameof(ApplicationUser), contact.UserId);

			var returnedContact = _mapper.Map<ReturnContactDto>(contact);

			return returnedContact;
		}


		public async Task<IEnumerable<ReturnContactDto>> GetAllContactsAsync(ClaimsPrincipal claimsPrincipal)
		{

			var userId = claimsPrincipal.FindFirstValue(ClaimTypes.PrimarySid);

			var roles = claimsPrincipal.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

			if (userId is null)
				throw new UnAuthorizedExeption("Not Allowed");

			var user = await _userManager.FindByIdAsync(userId!);

			if (user is null)
				throw new UnAuthorizedExeption("Not Allowed");

			var contacts = await _unitOfWork.GetRepository<Contact, int>().GetAllAsync();


			IOrderedEnumerable<Contact> orderedContacts;

			if (roles.Any(r => r == Roles.Admin))
				orderedContacts = contacts.OrderByDescending(contact => contact.CreatedOn);
			else if (roles.Any(R => R == Roles.Technical))
				orderedContacts = contacts.Where(c => c.MessageFor == Types.All || c.MessageFor == Types.Technical).OrderByDescending(contact => contact.CreatedOn);
			else
				orderedContacts = contacts.Where(c => c.MessageFor == Types.All || c.MessageFor == Types.User).OrderByDescending(contact => contact.CreatedOn);



			return _mapper.Map<IEnumerable<ReturnContactDto>>(orderedContacts);
		}


		public async Task<ReturnContactDto> UpdateContactAsync(int id, CreateContactDto contactDto)
		{
			var repo = _unitOfWork.GetRepository<Contact, int>();

			var contact = await repo.GetAsync(id);

			if (contact is null)
				throw new NotFoundExeption(nameof(Contact), id);

			_mapper.Map(contactDto, contact);

			try
			{

				repo.Update(contact);

			}
			catch (Exception ex)
			{
				throw new BadRequestExeption(ex.Message);
			}
			var success = await _unitOfWork.CompleteAsync() > 0;

			if (!success)
				throw new BadRequestExeption("Update is not Completed");

			return _mapper.Map<ReturnContactDto>(contact);
		}

		public async Task<string> DeleteContactAsync(int id)
		{
			var repo = _unitOfWork.GetRepository<Contact, int>();

			var contact = await repo.GetAsync(id);

			if (contact is null)
				throw new NotFoundExeption(nameof(Contact), id);
			try
			{

				repo.Delete(contact);
			}
			catch (Exception ex)
			{
				throw new BadRequestExeption(ex.Message);
			}

			var isDeleted = await _unitOfWork.CompleteAsync() > 0;

			if (!isDeleted)
				throw new BadRequestExeption("Delete is not Completed");

			return "Delete is Completed Successfully";
		}
	}
}
