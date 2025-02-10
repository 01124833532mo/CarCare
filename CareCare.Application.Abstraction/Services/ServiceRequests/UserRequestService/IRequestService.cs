using CarCare.Core.Domain.Entities.Orders;
using CareCare.Core.Application.Abstraction.Models.Auth.UserDtos;
using CareCare.Core.Application.Abstraction.Models.ServiceRequest.UserRequests;
using System.Security.Claims;

namespace CareCare.Core.Application.Abstraction.Services.ServiceRequests.UserRequestService
{
	public interface IRequestService
	{

		Task<ReturnRequestDto> CreateRequest(CreateRequestDto requestDto, List<string>? technicalId);
		Task<ReturnRequestDto> ReturnRequest(int requestId);
		Task<IEnumerable<ReturnRequestDto>> GetAllRequeststoUserForUser(ClaimsPrincipal claimsPrincipal);
		Task<IEnumerable<ReturnRequestDto>> GetAllRequeststoUserForAdmin(string UserId);
		Task<ReturnRequestDto> GetNearestTechnical(double UserLatitude, double UserLongitude);
		Task<IEnumerable<ReturnTechRequestDto>> GetActiveTechincals(int serviceTypeId);

		Task<string> ReceivedRequest(ClaimsPrincipal claimsPrincipal, int requestId, Status status);

		Task<string> TechincalBeActive(ClaimsPrincipal claimsPrincipal);

		Task<string> TechincalBeInActive(ClaimsPrincipal claimsPrincipal);


	}
}
