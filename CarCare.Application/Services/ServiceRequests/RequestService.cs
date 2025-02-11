using AutoMapper;
using CarCare.Core.Domain.Contracts.Persistence;
using CarCare.Core.Domain.Entities.Identity;
using CarCare.Core.Domain.Entities.Orders;
using CarCare.Core.Domain.Specifications.SpecsHandlers;
using CarCare.Shared.ErrorModoule.Exeptions;
using CareCare.Core.Application.Abstraction.Models.Auth.UserDtos;
using CareCare.Core.Application.Abstraction.Models.ServiceRequest.UserRequests;
using CareCare.Core.Application.Abstraction.Services.ServiceRequests.UserRequestService;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Core.Application.Services.ServiceRequests
{
	public class RequestService(IUnitOfWork _unitOfWork, UserManager<ApplicationUser> userManager, IMapper _mapper) : IRequestService
	{

		#region User Request

		public async Task<ReturnRequestDto> CreateRequest(CreateRequestDto requestDto)
		{
			var repo = _unitOfWork.serviceRequestRepository;

			var activeTechnicals = await repo.GetAvailableTechniciansAsync(requestDto.ServiceTypeId);

			if (activeTechnicals is null)
				throw new BadRequestExeption("There is no Available Techincals");

			if (!activeTechnicals.Where(t => t.Id == requestDto.TechId).Any())
				throw new BadRequestExeption("not Available Techincal");


			var request = _mapper.Map<ServiceRequest>(requestDto);

			request.ServicePrice += request.BasePrice;

			await _unitOfWork.serviceRequestRepository.AddAsync(request);

			var complete = await _unitOfWork.CompleteAsync() > 0;

			if (!complete)
				throw new BadRequestExeption("There is an Error in Request");

			var returnedData = _mapper.Map<ReturnRequestDto>(request);

			return returnedData;

		}


		//public async Task<ReturnRequestDto> UpdateRequest(UpdateRequestDto requestDto, int requestId)
		//{
		//	var repo = _unitOfWork.serviceRequestRepository;

		//	var activeTechnicals = await repo.GetAvailableTechniciansAsync(requestDto.ServiceTypeId);

		//	if (activeTechnicals is null)
		//		throw new BadRequestExeption("There is no Available Techincals");

		//	if (!activeTechnicals.Where(t => t.Id == requestDto.TechId).Any())
		//		throw new BadRequestExeption("not Available Techincal");

		//	var returnedRequest = await ReturnRequest(requestId);

		//	var newTechincal = activeTechnicals.FirstOrDefault();

		//	if (newTechincal is null)
		//		throw new BadRequestExeption("not Available Techincal");

		//	returnedRequest.TechId = newTechincal.Id;

		//	var request = _mapper.Map<ServiceRequest>(returnedRequest);

		//	_unitOfWork.GetRepository<ServiceRequest, int>().Update(request);

		//	var updated = await _unitOfWork.CompleteAsync() > 0;

		//	if (!updated)
		//		throw new BadRequestExeption("Error While Updating Request!!");

		//	return returnedRequest;
		//}

		public async Task<ReturnRequestDto> ReturnRequest(int requestId)
		{
			var repo = _unitOfWork.serviceRequestRepository;

			var request = await repo.GetAsync(requestId);

			if (request is null)
				throw new NotFoundExeption(nameof(request), requestId);

			var returnedData = _mapper.Map<ReturnRequestDto>(request);

			if (returnedData is null)
				throw new BadRequestExeption("Error While Returning Request Data");

			return returnedData;
		}

		public async Task<IEnumerable<ReturnRequestDto>> GetAllRequeststoUserForAdmin(string UserId)
		{
			var requests = await _unitOfWork.serviceRequestRepository.GetAllAsync();

			if (!requests.Any())
				throw new NotFoundExeption(nameof(requests), UserId);

			var userRequests = requests.Where(r => r.UserId == UserId);

			if (!userRequests.Any())
				throw new NotFoundExeption(nameof(userRequests), UserId);

			var returnedData = _mapper.Map<IEnumerable<ReturnRequestDto>>(userRequests);

			return returnedData;

		}

		public Task<IEnumerable<ReturnRequestDto>> GetAllRequeststoUserForUser(ClaimsPrincipal claimsPrincipal)
		{
			var userId = claimsPrincipal.FindFirst(ClaimTypes.PrimarySid)?.Value;

			if (userId is null)
				throw new UnAuthorizedExeption("UnAuthorized , You Are Not Allowed");

			return GetAllRequeststoUserForAdmin(userId);

		}

		public Task<ReturnRequestDto> GetNearestTechnical(double UserLatitude, double UserLongitude)
		{
			throw new NotImplementedException();
		}

		public async Task<IEnumerable<ReturnTechRequestDto>> GetActiveTechincals(int serviceTypeId)
		{
			var activeTechnicals = await _unitOfWork.serviceRequestRepository.GetAvailableTechniciansAsync(serviceTypeId);


			return _mapper.Map<IEnumerable<ReturnTechRequestDto>>(activeTechnicals);
		}


		#endregion


		#region Techincal Received Request

		public async Task<string> ReceivedRequest(int requestId, Status status)
		{
			var repo = _unitOfWork.serviceRequestRepository;

			var request = await repo.GetAsync(requestId);

			if (request is null)
				throw new NotFoundExeption(nameof(request), requestId);

			var technicalId = request.TechId;

			var technical = await userManager.FindByIdAsync(technicalId);

			if (technical is null)
				throw new NotFoundExeption("No Technical For This Id", nameof(technicalId));


			if (technicalId != request.TechId)
				throw new UnAuthorizedExeption("You are not allowed!!");


			request.Status = status;
			repo.Update(request);

			switch (status)
			{
				case Status.Pending:

					await Task.Delay(TimeSpan.FromSeconds(5));

					if (request.Status == Status.Pending || request.Status == Status.Canceled)
						goto case Status.Canceled;

					else if (request.Status == Status.InProgress)
						goto case Status.InProgress;

					else
						goto case Status.Completed;

				case Status.InProgress:

					technical.IsActive = false;


					break;


				case Status.Completed:

					technical.IsActive = true;


					break;

				case Status.Canceled:

					technical.IsActive = false;

					await userManager.UpdateAsync(technical);

					var activeTechnicals = (await GetActiveTechincals(request.ServiceTypeId)).ToList();

					if (!activeTechnicals.Any())
						throw new BadRequestExeption("There is no Available Techincals");

					var updatedRequest = _mapper.Map<UpdateRequestDto>(request);

					foreach (var tech in activeTechnicals)
					{
						updatedRequest.TechId = tech.Id;

						_mapper.Map(updatedRequest, request);

						repo.Update(request);

						await ReceivedRequest(updatedRequest.Id, Status.Pending);
					}

					break;

			}

			var complete = await _unitOfWork.CompleteAsync() > 0;

			if (!complete)
				throw new BadRequestExeption("Request Can't Received");

			return $"Request status updated to {status}";

		}


		public async Task<string> TechincalBeActive(ClaimsPrincipal claimsPrincipal)
		{
			var techId = claimsPrincipal.FindFirstValue(ClaimTypes.PrimarySid);

			if (techId is null)
				throw new UnAuthorizedExeption("You are not allowed!!");

			var techincal = await userManager.FindByIdAsync(techId);

			if (techincal is null)
				throw new NotFoundExeption("No User For This Id", nameof(techId));

			techincal.IsActive = true;

			return $"Techincal {techincal.FullName} is Actived!!";
		}

		public async Task<string> TechincalBeInActive(ClaimsPrincipal claimsPrincipal)
		{
			var techId = claimsPrincipal.FindFirstValue(ClaimTypes.PrimarySid);

			if (techId is null)
				throw new UnAuthorizedExeption("You are not allowed!!");

			var techincal = await userManager.FindByIdAsync(techId);

			if (techincal is null)
				throw new NotFoundExeption("No User For This Id", nameof(techId));

			techincal.IsActive = false;

			return $"Techincal {techincal.FullName} is Inactived!!";
		}

		#endregion

	}
}
