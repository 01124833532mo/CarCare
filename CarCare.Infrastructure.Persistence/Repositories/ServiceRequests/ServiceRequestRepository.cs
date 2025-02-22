using CarCare.Core.Domain.Contracts.Persistence.ServiceRequests;
using CarCare.Core.Domain.Entities.Identity;
using CarCare.Core.Domain.Entities.Orders;
using CarCare.Core.Domain.Entities.ServiceTypes;
using CarCare.Core.Domain.Entities.Vehicles;
using CarCare.Infrastructure.Persistence._Data;
using CarCare.Infrastructure.Persistence.Generic_Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Infrastructure.Persistence.Repositories.ServiceRequests
{
	public class ServiceRequestRepository : GenericRepository<ServiceRequest, int>, IServiceRequestRepository
	{
		private readonly CarCarIdentityDbContext _dbContext;

		public ServiceRequestRepository(CarCarIdentityDbContext dbContext)
			: base(dbContext)
		{

			_dbContext = dbContext;
		}

		public async Task<IEnumerable<ApplicationUser>> GetAvailableTechniciansAsync(int serviceTypeId)
		{
			return await _dbContext.Users
				.Where(t => t.IsActive == true && t.Type == Types.Technical)
				.Where(t => t.ServiceType!.Id == serviceTypeId).ToListAsync();
		}

		public async Task<IEnumerable<ApplicationUser>> GetNearestTechincal(int serviceTypeId, double userLatitude, double userLongitude)
		{
			var techs = await _dbContext.Users
				.Where(t => t.IsActive == true && t.Type == Types.Technical)
				.Where(t => t.ServiceType!.Id == serviceTypeId).ToListAsync();

			var sortedTechs = techs
				.AsEnumerable()
				.Select(t => new
				{
					technical = t,
					Distance = CalculateDistance(userLatitude, userLongitude, t.TechLatitude!.Value, t.TechLongitude!.Value)
				})
				.OrderBy(t => t.Distance)
				.Select(t => t.technical)
				.ToList();

			return sortedTechs;

		}


		private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
		{
			const double R = 6371;

			double dLat = (lat2 - lat1) * Math.PI / 180.0;
			double dLon = (lon2 - lon1) * Math.PI / 180.0;

			double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
					   Math.Cos(lat1 * Math.PI / 180.0) * Math.Cos(lat2 * Math.PI / 180.0) *
					   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

			double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

			return R * c;
		}

	}
}
