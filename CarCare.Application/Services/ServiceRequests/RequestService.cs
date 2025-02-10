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

		public async Task<ReturnRequestDto> CreateRequest(CreateRequestDto requestDto, List<string>? _notactivetechnicalId)
		{
			var repo = _unitOfWork.serviceRequestRepository;

			var activeTechnicals = await repo.GetAvailableTechniciansAsync(requestDto.ServiceTypeId);

			if (activeTechnicals is null)
				throw new BadRequestExeption("There is no Available Techincals");

			if (_notactivetechnicalId is not null && !activeTechnicals.Where(t => t.Id == requestDto.TechId && !_notactivetechnicalId.Contains(t.Id)).Any())
				throw new BadRequestExeption("not Available Techincal");


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

		public async Task<string> ReceivedRequest(ClaimsPrincipal claimsPrincipal, int requestId, Status status)
		{
			var technicalId = claimsPrincipal.FindFirst(ClaimTypes.PrimarySid)!.Value;

			if (technicalId is null)
				throw new UnAuthorizedExeption("You are not allowed!!");

			var technical = await userManager.FindByIdAsync(technicalId);

			if (technical is null)
				throw new NotFoundExeption("No User For This Id", nameof(technicalId));

			var request = await _unitOfWork.GetRepository<ServiceRequest, int>().GetAsync(requestId);

			if (request is null)
				throw new NotFoundExeption(nameof(request), requestId);

			List<string> notActiveTechincals = new();

			request.Status = status;

			switch (status)
			{
				case Status.InProgress:

					technical.IsActive = false;

					break;

				case Status.Pending:

					await Task.Delay(TimeSpan.FromMinutes(3));

					notActiveTechincals.Append(technicalId);

					var Createdrequest = _mapper.Map<CreateRequestDto>(request);

					await CreateRequest(Createdrequest, notActiveTechincals);

					break;

				case Status.Completed:

					technical.IsActive = true;

					break;

				case Status.Canceled:

					technical.IsActive = true;

					notActiveTechincals.Append(technicalId);

					Createdrequest = _mapper.Map<CreateRequestDto>(request);

					await CreateRequest(Createdrequest, notActiveTechincals);

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
