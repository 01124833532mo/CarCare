using AutoMapper;
using CarCare.Core.Domain.Contracts.Persistence;
using CarCare.Core.Domain.Entities.Identity;
using CarCare.Core.Domain.Entities.Orders;
using CarCare.Shared.ErrorModoule.Exeptions;
using CareCare.Core.Application.Abstraction.Common.Contract.Infrastructure;
using CareCare.Core.Application.Abstraction.Models.ServiceRequest.UserRequests;
using CareCare.Core.Application.Abstraction.Services.ServiceRequests.UserRequestService;
using Microsoft.AspNetCore.Identity;
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

            request.ServicePrice = request.BasePrice + (request.ServiceQuantity * request.ServicePrice);

            request.TechId = activeTechnicals.FirstOrDefault()!.Technical.Id;

            await _unitOfWork.serviceRequestRepository.AddAsync(request);

            var complete = await _unitOfWork.CompleteAsync() > 0;

            if (!complete)
                throw new BadRequestExeption("There is an Error in Request");

            var Orderid = request.Id;

            var result = await paymentService.CreateOrUpdatePaymentIntent(Orderid);

            var returnedData = _mapper.Map<ReturnRequestDto>(request);
            returnedData.PaymentIntentId = result.PaymentIntentId;
            returnedData.ClientSecret = result.ClientSecret;

            _ = PendingRequest(request.Id);

            return returnedData;

        }
        public async Task<ReturnRequestDto> CreateRequestManually(CreateRequestDto requestDto)
        {
            var repo = _unitOfWork.serviceRequestRepository;

            var activeTechnicals = await repo.GetAvailableTechniciansAsync(requestDto.ServiceTypeId, requestDto.UserLatitude, requestDto.UserLongitude);

            if (activeTechnicals.Count() == 0)
                throw new BadRequestExeption("There is no Available Techincals");

            if (!activeTechnicals.Where(t => t.Technician.Id == requestDto.TechId).Any())
                throw new NotFoundExeption("No Technical Found For This Id ", nameof(requestDto.TechId));
            if (requestDto.ServiceQuantity is null)
                requestDto.ServiceQuantity = 1;

            var request = _mapper.Map<ServiceRequest>(requestDto);

            request.ServicePrice = request.BasePrice + (request.ServiceQuantity * request.ServicePrice);


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

        public async Task<ReturnRequestDto> UpdateTechnicalinRequest(int requestid, string techid, int sercieid)
        {
            var repo = _unitOfWork.GetRepository<ServiceRequest, int>();

            var technicians = await userManager.Users
                .Where(t => t.IsActive == true && t.Type == Types.Technical && t.ServiceType!.Id == sercieid)
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

            request!.TechId = techid;

            repo.Update(request);

            var complete = await _unitOfWork.CompleteAsync() > 0;

            if (!complete)
                throw new BadRequestExeption("There is an Error in Update Request");

            var returnedRequest = _mapper.Map<ReturnRequestDto>(request);

            return returnedRequest;




        }

        public async Task<string> DeleteRequest(int requestid)
        {
            var repo = _unitOfWork.GetRepository<ServiceRequest, int>();
            var request = await repo.GetAsync(requestid);

            if (request is null) throw new NotFoundExeption("Not request With This Id:", nameof(requestid));


            if (request.Status == Status.Pending || request.Status == Status.Canceled)
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

        public async Task<IEnumerable<ReturnRequestDto>> GetAllRequeststoUserForUser(ClaimsPrincipal claimsPrincipal)
        {
            var userId = claimsPrincipal.FindFirst(ClaimTypes.PrimarySid)?.Value;

            if (userId is null)
                throw new UnAuthorizedExeption("UnAuthorized , You Are Not Allowed");

            return await GetAllRequeststoUserForAdmin(userId);

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

        public async Task<string> ReceivedRequestAutomatic(ClaimsPrincipal claimsPrincipal, int requestId, Status status)
        {
            var repo = _unitOfWork.serviceRequestRepository;

            var request = await repo.GetAsync(requestId);

            if (request is null)
                throw new NotFoundExeption(nameof(request), requestId);

            var techIdclaims = claimsPrincipal.FindFirst(ClaimTypes.PrimarySid)!.Value;

            if (techIdclaims is null)
                throw new NotFoundExeption("No Technical For This Id", nameof(techIdclaims));


            var technicalId = request.TechId;

            var technical = await userManager.FindByIdAsync(technicalId);

            if (technical is null)
                throw new NotFoundExeption("No Technical For This Id", nameof(technicalId));


            if (technicalId != techIdclaims)
                throw new UnAuthorizedExeption("You are not allowed!!");


            request.Status = status;
            repo.Update(request);


            switch (status)
            {

                case Status.InProgress:

                    technical.IsActive = false;

                    break;

                case Status.Canceled:

                    technical.IsActive = false;

                    await userManager.UpdateAsync(technical);

                    var updatedrequest = _mapper.Map<CreateRequestDto>(request);

                    _unitOfWork.serviceRequestRepository.Delete(request);

                    await CreateRequestAutomatic(updatedrequest);

                    break;

            }

            var complete = await _unitOfWork.CompleteAsync() > 0;

            if (!complete)
                throw new BadRequestExeption("Request Can't Received");

            return $"Request status updated to {status}";

        }


        public async Task<string> AcceptRequest(ClaimsPrincipal claimsPrincipal, int requestId)
        {
            var result = await ReceivedRequestAutomatic(claimsPrincipal, requestId, Status.InProgress);
            return result;
        }

        public async Task<string> RejectRequest(ClaimsPrincipal claimsPrincipal, int requestId)
        {
            var result = await ReceivedRequestAutomatic(claimsPrincipal, requestId, Status.Canceled);
            return result;
        }

        private async Task PendingRequest(int requestId)
        {
            await Task.Delay(TimeSpan.FromSeconds(5));

            var request = await ReturnRequest(requestId);

            // if (request.Status == Status.Pending)
            // {

            await RejectRequest(null!, requestId);

            // }
        }

        public async Task<IEnumerable<ReturnRequestDto>> GetAllRequestsToTechnical(ClaimsPrincipal claimsPrincipal)
        {
            var techId = claimsPrincipal.FindFirst(ClaimTypes.PrimarySid)?.Value;

            if (techId is null)
                throw new UnAuthorizedExeption("UnAuthorized , You Are Not Allowed");

            var requests = await _unitOfWork.serviceRequestRepository.GetAllAsync();

            if (!requests.Any())
                throw new NotFoundExeption(nameof(requests), techId);

            var userRequests = requests.Where(r => r.TechId == techId);

            if (!userRequests.Any())
                throw new NotFoundExeption(nameof(userRequests), techId);

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







        #endregion

    }
}
