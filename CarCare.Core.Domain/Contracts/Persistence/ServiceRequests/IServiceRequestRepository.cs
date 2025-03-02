using CarCare.Core.Domain.Entities.Identity;
using CarCare.Core.Domain.Entities.Orders;

namespace CarCare.Core.Domain.Contracts.Persistence.ServiceRequests
{
	public interface IServiceRequestRepository : IGenericRepository<ServiceRequest, int>
	{
		Task<IEnumerable<(ApplicationUser Technician, double Distance)>> GetAvailableTechniciansAsync(int serviceTypeId, double userLongitude, double userLatitude);
		Task<IEnumerable<(ApplicationUser Technical, double Distance)>> GetNearestTechincal(int serviceTypeId, double userLatitude, double userLongitude);
		double CalculateDistance(double userLatitude, double userLongitude, double? techLatitude, double? techLongitude);

	}
}
