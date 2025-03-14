using CarCare.Core.Domain.Entities.Orders;
using CareCare.Core.Application.Abstraction.Models.ServiceRequest;
using CareCare.Core.Application.Abstraction.Models.ServiceRequest.UserRequests;
using System.Security.Claims;

namespace CareCare.Core.Application.Abstraction.Services.ServiceRequests.UserRequestService
{
	public interface IRequestService
	{

		Task<ReturnRequestDto> CreateRequestAutomatic(CreateRequestDto requestDto);
		Task<ReturnRequestDto> CreateRequestManually(CreateRequestDto requestDto);
		Task<ReturnRequestDto> UpdateTechnicalinRequest(int requestid, string techid, int sercieid);
		Task<string> DeleteRequest(int requestid);
		Task<ReturnRequestDto> ReturnRequest(int requestId);
		Task<IEnumerable<ReturnRequestDto>> GetAllRequeststoUserForUser(ClaimsPrincipal claimsPrincipal);
		Task<IEnumerable<ReturnRequestDto>> GetAllRequeststoUserForAdmin(string UserId);
		Task<IEnumerable<ReturnTechRequestDto>> GetNearestTechnicals(int serviceTypeId, double UserLatitude, double UserLongitude);
		Task<IEnumerable<ReturnTechRequestDto>> GetActiveTechincals(int serviceTypeId, double UserLatitude, double UserLongitude);


		Task<string> ReceivedRequest(int requestId, BusnissStatus status);

		Task<string> AcceptRequest(int requestId);
		Task<string> RejectRequest(int requestId);
		Task<string> CompleteRequest(int requestId);

		Task<IEnumerable<ReturnRequestDto>> GetAllRequestsToTechnical(ClaimsPrincipal claimsPrincipal, BusnissStatus? status);
		Task<IEnumerable<ReturnRequestDto>> GetAllPendingRequestsToTechnical(ClaimsPrincipal claimsPrincipal, string? sort);
		Task<string> TechincalBeActive(ClaimsPrincipal claimsPrincipal);

		Task<string> TechincalBeInActive(ClaimsPrincipal claimsPrincipal);

		Task CheckStatusAutomatic(int requestId);
		Task<object> CheckStatusManual(int requestId);
	}
}
