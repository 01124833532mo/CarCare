using CarCare.Core.Domain.Entities.Identity;
using CarCare.Core.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Core.Domain.Contracts.Persistence.ServiceRequests
{
	public interface IServiceRequestRepository : IGenericRepository<ServiceRequest, int>
	{
		Task<IEnumerable<ApplicationUser>> GetAvailableTechniciansAsync(int serviceTypeId);
		Task<IEnumerable<ApplicationUser>> GetNearestTechincal(int serviceTypeId, double userLatitude, double userLongitude);
	}
}
