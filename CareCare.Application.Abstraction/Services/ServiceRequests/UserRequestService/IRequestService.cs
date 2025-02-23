using CarCare.Core.Domain.Entities.Orders;
using CareCare.Core.Application.Abstraction.Models.ServiceRequest.UserRequests;
using System.Security.Claims;

namespace CareCare.Core.Application.Abstraction.Services.ServiceRequests.UserRequestService
{
    public interface IRequestService
    {

        Task<ReturnRequestDto> CreateRequestAutomatic(CreateRequestDto requestDto);
        Task<ReturnRequestDto> CreateRequestManually(CreateRequestDto requestDto);
        Task<ReturnRequestDto> UpdateTechnicalinRequest(int requestid, string techid, int sercieid);
        Task<ReturnRequestDto> ReturnRequest(int requestId);
        Task<IEnumerable<ReturnRequestDto>> GetAllRequeststoUserForUser(ClaimsPrincipal claimsPrincipal);
        Task<IEnumerable<ReturnRequestDto>> GetAllRequeststoUserForAdmin(string UserId);
        Task<IEnumerable<ReturnTechRequestDto>> GetNearestTechnicals(int serviceTypeId, double UserLatitude, double UserLongitude);
        Task<IEnumerable<ReturnTechRequestDto>> GetActiveTechincals(int serviceTypeId, double UserLatitude, double UserLongitude);


        Task<string> ReceivedRequestAutomatic(ClaimsPrincipal claimsPrincipal, int requestId, Status status);

        Task<string> AcceptRequest(ClaimsPrincipal claimsPrincipal, int requestId);
        Task<string> RejectRequest(ClaimsPrincipal claimsPrincipal, int requestId);

        Task<IEnumerable<ReturnRequestDto>> GetAllRequestsToTechnical(ClaimsPrincipal claimsPrincipal);

        Task<string> TechincalBeActive(ClaimsPrincipal claimsPrincipal);

        Task<string> TechincalBeInActive(ClaimsPrincipal claimsPrincipal);


    }
}
