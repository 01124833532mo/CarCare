using AutoMapper;
using CarCare.Core.Domain.Contracts.Persistence;
using CarCare.Core.Domain.Entities.Identity;
using CarCare.Core.Domain.Entities.Orders;
using CarCare.Core.Domain.Entities.Vehicles;
using CarCare.Core.Domain.Specifications;
using CarCare.Core.Domain.Specifications.SpecsHandlers.Vehicles;
using CarCare.Shared.ErrorModoule.Exeptions;
using CareCare.Core.Application.Abstraction.Common;
using CareCare.Core.Application.Abstraction.Common.Contract.Infrastructure;
using CareCare.Core.Application.Abstraction.Models.ServiceRequest.UserRequests;
using CareCare.Core.Application.Abstraction.Services.ServiceRequests.UserRequestService;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CarCare.Core.Application.Services.ServiceRequests
{
	public class RequestService
		(IUnitOfWork _unitOfWork,
		UserManager<ApplicationUser> userManager,
		IMapper _mapper,
		IPaymentService paymentService)
		: IRequestService
	{

		#region User Request

		/// Notes
		/// will request be pending after 10 minutes will chosse another technical and make the current technical inactive
		/// 

		public async Task<ReturnRequestDto> CreateRequestAutomatic(CreateRequestDto requestDto)
		{

			var repo = _unitOfWork.serviceRequestRepository;

			var activeTechnicals = await repo.GetNearestTechincal(requestDto.ServiceTypeId, requestDto.UserLatitude, requestDto.UserLongitude);

			if (activeTechnicals.Count() == 0)
				throw new BadRequestExeption("There is no Available Techincals");

			if (requestDto.ServiceQuantity is null)
				requestDto.ServiceQuantity = 1;

			var request = _mapper.Map<ServiceRequest>(requestDto);


			request.TechId = activeTechnicals.FirstOrDefault()!.Technical.Id;

			request.IsAutomatic = true;

			request.Distance = activeTechnicals.FirstOrDefault()!.Distance;

			request.ServicePrice = ((decimal)request.Distance * 10) + request.BasePrice + (request.ServiceQuantity * request.ServicePrice);

			await _unitOfWork.serviceRequestRepository.AddAsync(request);


			var complete = await _unitOfWork.CompleteAsync() > 0;

			if (!complete)
				throw new BadRequestExeption("There is an Error in Request");

			var Orderid = request.Id;

			var result = await paymentService.CreateOrUpdatePaymentIntent(Orderid);

			var returnedData = _mapper.Map<ReturnRequestDto>(request);
			returnedData.PaymentIntentId = result.PaymentIntentId;
			returnedData.ClientSecret = result.ClientSecret;


			RecurringJob.AddOrUpdate(request.Id.ToString(), () => CheckStatusAutomatic(returnedData.Id), Cron.Minutely());


			return returnedData;

		}
		public async Task<ReturnRequestDto> CreateRequestManually(CreateRequestDto requestDto)
		{

			var repo = _unitOfWork.serviceRequestRepository;

			var activeTechnicals = await repo.GetAvailableTechniciansAsync(requestDto.ServiceTypeId, requestDto.UserLatitude, requestDto.UserLongitude);

			if (activeTechnicals.Count() == 0)
				throw new BadRequestExeption("There is no Available Techincals");

			if (!activeTechnicals.Where(t => t.Technician.Id == requestDto.TechId).Any())
				throw new NotFoundExeption("No Technical Found For This Id Or Technical is In Active ", nameof(requestDto.TechId));
			if (requestDto.ServiceQuantity is null)
				requestDto.ServiceQuantity = 1;

			var request = _mapper.Map<ServiceRequest>(requestDto);

			#region Calculate Price
			var technical = await userManager.FindByIdAsync(requestDto.TechId ?? string.Empty);

			if (technical is null)
				throw new NotFoundExeption("No Technical Found For This Id ", nameof(requestDto.TechId));

			var distance = repo.CalculateDistance(requestDto.UserLatitude, requestDto.UserLongitude, technical.TechLatitude, technical.TechLongitude);


			request.ServicePrice = request.BasePrice + (request.ServiceQuantity * request.ServicePrice) + (decimal)(distance * 10);

			request.Distance = distance;
			#endregion

			await _unitOfWork.serviceRequestRepository.AddAsync(request);

			var complete = await _unitOfWork.CompleteAsync() > 0;

			if (!complete)
				throw new BadRequestExeption("There is an Error in Request");

			var Orderid = request.Id;

			var result = await paymentService.CreateOrUpdatePaymentIntent(Orderid);

			var returnedData = _mapper.Map<ReturnRequestDto>(request);
			returnedData.PaymentIntentId = result.PaymentIntentId;
			returnedData.ClientSecret = result.ClientSecret;

			return returnedData;
		}


		public async Task<ReturnRequestDto> ResendRequestAutomatic(int requestId, double userLatitude, double userLongitude)
		{
			var repo = _unitOfWork.serviceRequestRepository;

			var request = repo.GetAsync(requestId).Result;

			if (request is null)
				throw new NotFoundExeption(nameof(request), requestId);

			var activeTechnicals = await repo.GetNearestTechincal(request.ServiceTypeId, userLatitude, userLongitude);

			var allowedTime = (DateTime.UtcNow - request.CreatedOn).TotalMinutes;

			if (activeTechnicals.Count() == 0 || allowedTime >= 20)
			{
				//BackgroundJob.Delete(request.JopId);
				RecurringJob.RemoveIfExists(request.Id.ToString());

				request.BusnissStatus = BusnissStatus.Canceled;

				repo.Update(request);

				var Canceledcomplete = await _unitOfWork.CompleteAsync() > 0;

				if (!Canceledcomplete)
					throw new BadRequestExeption("There is an Error in Request");

				if (allowedTime >= 20)
					throw new BadRequestExeption("Time Exceeded 20 Minutes");

				throw new BadRequestExeption("There is no Available Techincals");
			}

			request.TechId = activeTechnicals.FirstOrDefault()!.Technical.Id;

			request.Distance = activeTechnicals.FirstOrDefault()!.Distance;

			request.ServicePrice = ((decimal)request.Distance * 10) + request.BasePrice + (request.ServiceQuantity * request.ServicePrice);

			request.BusnissStatus = BusnissStatus.Pending;

			repo.Update(request);

			var complete = await _unitOfWork.CompleteAsync() > 0;

			if (!complete)
				throw new BadRequestExeption("There is an Error in Request");

			var returnedData = _mapper.Map<ReturnRequestDto>(request);

			return returnedData;

		}

		public async Task<ReturnRequestDto> UpdateTechnicalinRequest(int requestid, string techid, int serviceId)
		{
			var repo = _unitOfWork.serviceRequestRepository;

			var technicians = await userManager.Users
				.Where(t => t.IsActive == true && t.Type == Types.Technical && t.ServiceType!.Id == serviceId)
				.ToListAsync();

			if (string.IsNullOrEmpty(techid))
				throw new BadRequestExeption("Technical id is null or empty");


			if (technicians.Count == 0)
				throw new BadRequestExeption("There is no Available Techincals Or Servic Id Incorrect");

			var checkexsistingtechnical = await userManager.Users.FirstOrDefaultAsync(e => e.Id == techid);
			if (checkexsistingtechnical is null) throw new NotFoundExeption("No Technical Found For This Id", nameof(techid));


			var request = await repo.GetAsync(requestid);
			if (request is null)
				throw new NotFoundExeption("Request not found", nameof(requestid));

			request.ServicePrice -= (decimal)(request.Distance * 10);

			request!.TechId = techid;
			request.BusnissStatus = BusnissStatus.Pending;

			var distance = repo.CalculateDistance(request.UserLatitude, request.UserLongitude, checkexsistingtechnical.TechLatitude, checkexsistingtechnical.TechLongitude);
			request.Distance = distance;
			request.ServicePrice = request.ServicePrice + (decimal)(distance * 10);
			repo.Update(request);

			var complete = await _unitOfWork.CompleteAsync() > 0;

			if (!complete)
				throw new BadRequestExeption("There is an Error in Update Request");

			var Orderid = request.Id;

			var result = await paymentService.CreateOrUpdatePaymentIntent(Orderid);

			var returnedData = _mapper.Map<ReturnRequestDto>(request);
			returnedData.PaymentIntentId = result.PaymentIntentId;
			returnedData.ClientSecret = result.ClientSecret;





			return returnedData;

		}

		public async Task<string> DeleteRequest(int requestid)
		{
			var repo = _unitOfWork.GetRepository<ServiceRequest, int>();
			var request = await repo.GetAsync(requestid);

			if (request is null) throw new NotFoundExeption("Not request With This Id:", nameof(requestid));


			if (request.BusnissStatus == BusnissStatus.Pending || request.BusnissStatus == BusnissStatus.Canceled)
			{
				repo.Delete(request);

				var result = await _unitOfWork.CompleteAsync() > 0;

				if (result is true) return "Deleted Successfully";

				else
					throw new BadRequestExeption("Operation Faild");
			}
			else
			{
				throw new BadRequestExeption("This Request Not Pending ,Is Already Proccessing");
			}

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

		public async Task<Pagination<ReturnRequestDto>> GetAllRequeststoUserForAdmin(string userId, int pageSize, int pageIndex)
		{

			var specs = new ServiceRequestSpecifications(userId, pageSize, pageIndex);

			var requests = await _unitOfWork.serviceRequestRepository.GetAllWithSpecAsync(specs);

			var countSpec = new ServiceRequestSpecificationsCount(userId);

			var count = await _unitOfWork.GetRepository<ServiceRequest, int>().GetCountAsync(countSpec);

			var returnedData = _mapper.Map<IEnumerable<ReturnRequestDto>>(requests);

			return new Pagination<ReturnRequestDto>(pageIndex, pageSize, count) { Data = returnedData };

		}

		public async Task<Pagination<ReturnRequestDto>> GetAllRequeststoUserForUser(ClaimsPrincipal claimsPrincipal, int pageSize, int pageIndex)
		{
			var userId = claimsPrincipal.FindFirst(ClaimTypes.PrimarySid)?.Value;

			if (userId is null)
				throw new UnAuthorizedExeption("UnAuthorized , You Are Not Allowed");

			return await GetAllRequeststoUserForAdmin(userId, pageSize, pageIndex);

		}

		public async Task<IEnumerable<ReturnTechRequestDto>> GetNearestTechnicals(int serviceTypeId, double UserLatitude, double UserLongitude)
		{
			var techs = await _unitOfWork.serviceRequestRepository.GetNearestTechincal(serviceTypeId, UserLatitude, UserLongitude);
			return _mapper.Map<IEnumerable<ReturnTechRequestDto>>(techs);
		}


		//public async Task<IEnumerable<ReturnTechRequestDto>> GetActiveTechincals(int serviceTypeId)
		//{
		//	var activeTechnicals = await _unitOfWork.serviceRequestRepository.GetAvailableTechniciansAsync(serviceTypeId);


		//	return _mapper.Map<IEnumerable<ReturnTechRequestDto>>(activeTechnicals);
		//}

		public async Task<IEnumerable<ReturnTechRequestDto>> GetActiveTechincals(int serviceTypeId, double userlongitude, double userlatitdute)
		{
			var activeTechnicals = await _unitOfWork.serviceRequestRepository.GetAvailableTechniciansAsync(serviceTypeId, userlongitude, userlatitdute);


			return _mapper.Map<IEnumerable<ReturnTechRequestDto>>(activeTechnicals);
		}



		#endregion


		#region Techincal Received Request

		public async Task<string> ReceivedRequest(int requestId, BusnissStatus status)
		{
			var repo = _unitOfWork.serviceRequestRepository;

			var request = await repo.GetAsync(requestId);

			if (request is null)
				throw new NotFoundExeption(nameof(request), requestId);


			var technicalId = request.TechId;

			var technical = await userManager.FindByIdAsync(technicalId);

			if (technical is null)
				throw new NotFoundExeption("No Technical For This Id", nameof(technicalId));


			request.BusnissStatus = status;
			repo.Update(request);


			switch (status)
			{

				case BusnissStatus.Completed:
					technical.IsActive = true;
					technical.TechProfit += (double)request.ServicePrice * 80 / 100;
					RecurringJob.RemoveIfExists(request.Id.ToString());

					var complete = await _unitOfWork.CompleteAsync() > 0;

					if (!complete)
						throw new BadRequestExeption("Request Can't Received");
					break;

				case BusnissStatus.InProgress:

					technical.IsActive = false;

					RecurringJob.RemoveIfExists(request.Id.ToString());

					complete = await _unitOfWork.CompleteAsync() > 0;

					if (!complete)
						throw new BadRequestExeption("Request Can't Received");
					break;

				case BusnissStatus.Canceled:

					technical.IsActive = false;

					complete = await _unitOfWork.CompleteAsync() > 0;

					if (!complete)
						throw new BadRequestExeption("Request Can't Received");

					if (request.IsAutomatic)
					{
						var updatedrequest = _mapper.Map<CreateRequestDto>(request);

						await ResendRequestAutomatic(requestId, updatedrequest.UserLatitude, updatedrequest.UserLongitude);
					}
					break;

			}



			return $"Request status updated to {status}";

		}


		public async Task<string> AcceptRequest(int requestId)
		{
			var result = await ReceivedRequest(requestId, BusnissStatus.InProgress);
			return result;
		}

		public async Task<string> RejectRequest(int requestId)
		{
			var request = await _unitOfWork.serviceRequestRepository.GetAsync(requestId);

			if (request is null)
				throw new NotFoundExeption(nameof(request), requestId);

			string result = await ReceivedRequest(requestId, BusnissStatus.Canceled);

			return result;
		}

		public async Task<string> CompleteRequest(int requestId)
		{
			var request = await _unitOfWork.serviceRequestRepository.GetAsync(requestId);

			if (request is null)
				throw new NotFoundExeption(nameof(request), requestId);

			var result = await ReceivedRequest(requestId, BusnissStatus.Completed);

			return result;
		}



		public async Task<IEnumerable<ReturnRequestDto>> GetAllRequestsToTechnical(ClaimsPrincipal claimsPrincipal, BusnissStatus? status)
		{
			var techId = claimsPrincipal.FindFirst(ClaimTypes.PrimarySid)?.Value;

			if (techId is null)
				throw new UnAuthorizedExeption("UnAuthorized , You Are Not Allowed");

			var specs = new ServiceRequestSpecifications(status);

			var requests = await _unitOfWork.serviceRequestRepository.GetAllWithSpecAsync(specs);

			if (!requests.Any())
				throw new NotFoundExeption(nameof(requests), techId);


			var userRequests = requests.Where(r => r.TechId == techId);

			if (!userRequests.Any())
				throw new NotFoundExeption(nameof(userRequests), techId);

			var returnedData = _mapper.Map<IEnumerable<ReturnRequestDto>>(userRequests);

			return returnedData;

		}

		public async Task<IEnumerable<ReturnRequestDto>> GetAllPendingRequestsToTechnical(ClaimsPrincipal claimsPrincipal, string? sort)
		{
			var spec = new GetAllRequestsPenddingOrderingSpec(sort);
			var techId = claimsPrincipal.FindFirst(ClaimTypes.PrimarySid)?.Value;
			if (techId is null)
				throw new UnAuthorizedExeption("UnAuthorized , You Are Not Allowed");
			var requests = await _unitOfWork.serviceRequestRepository.GetAllWithSpecAsync(spec);
			if (!requests.Any())
				throw new NotFoundExeption(nameof(requests), techId);
			var userRequests = requests.Where(r => r.TechId == techId && (r.BusnissStatus == BusnissStatus.Pending));
			if (!userRequests.Any())
				throw new NotFoundExeption("Not Exsist Pendding Requestes With This Technical", nameof(techId));
			var returnedData = _mapper.Map<IEnumerable<ReturnRequestDto>>(userRequests);
			return returnedData;

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

			var succeed = await userManager.UpdateAsync(techincal);

			if (!succeed.Succeeded)
				throw new BadRequestExeption("Error While Save Details");

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

			var succeed = await userManager.UpdateAsync(techincal);

			if (!succeed.Succeeded)
				throw new BadRequestExeption("Error While Save Details");

			return $"Techincal {techincal.FullName} is Inactived!!";
		}




		public async Task CheckStatusAutomatic(int requestId)
		{
			var request = await _unitOfWork.serviceRequestRepository.GetAsync(requestId);

			if (request is null)
				throw new NotFoundExeption(nameof(request), nameof(requestId));

			double time = (DateTime.UtcNow - request.LastModifiedOn).TotalMinutes;

			if ((time >= 5) || request.BusnissStatus == BusnissStatus.InProgress)
			{
				if (request.BusnissStatus == BusnissStatus.Pending || request.BusnissStatus == BusnissStatus.Canceled)
					await RejectRequest(requestId);
				else
					RecurringJob.RemoveIfExists(request.Id.ToString());
			}
		}
		public async Task<object> CheckStatusManual(int requestId)
		{
			var request = await _unitOfWork.serviceRequestRepository.GetAsync(requestId);

			if (request is null)
				throw new NotFoundExeption(nameof(request), nameof(requestId));

			object Status = new { status = request.BusnissStatus.ToString() };

			return Status;
		}



		#endregion

	}
}
