using AutoMapper;
using CarCare.Core.Domain.Contracts.Persistence;
using CarCare.Core.Domain.Entities.Contacts;
using CarCare.Shared.ErrorModoule.Exeptions;
using CareCare.Core.Application.Abstraction.Models.Contacts;
using CareCare.Core.Application.Abstraction.Services.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Core.Application.Services.Contacts
{
	public class ContactService(IUnitOfWork _unitOfWork, IMapper _mapper) : IContactService
	{
		public async Task<ReturnContactDto> CreateContactAsync(CreateContactDto contactDto)
		{
			var repo = _unitOfWork.GetRepository<Contact, int>();

			var contact = _mapper.Map<Contact>(contactDto);

			var added = repo.AddAsync(contact);

			if (added is null)
				throw new BadRequestExeption("Contact not Created!");

			var created = await _unitOfWork.CompleteAsync() > 0;
			if (!created)
				throw new BadRequestExeption("Contact not Created!");

			var returnedContact = _mapper.Map<ReturnContactDto>(contact);

			return returnedContact;
		}


		public async Task<IEnumerable<ReturnContactDto>> GetAllContactsAsync()
		{
			var contact = await _unitOfWork.GetRepository<Contact, int>().GetAllAsync();
			return _mapper.Map<IEnumerable<ReturnContactDto>>(contact);
		}


		public async Task<ReturnContactDto> UpdateContactAsync(int id, CreateContactDto contactDto)
		{
			var repo = _unitOfWork.GetRepository<Contact, int>();

			var contact = await repo.GetAsync(id);

			if (contact is null)
				throw new NotFoundExeption(nameof(Contact), id);

			_mapper.Map(contactDto, contact);


			repo.Update(contact);

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

			repo.Delete(contact);

			var isDeleted = await _unitOfWork.CompleteAsync() > 0;

			if (!isDeleted)
				throw new BadRequestExeption("Delete is not Completed");

			return "Delete is Completed Successfully";
		}
	}
}
