using CarCare.Core.Domain.Contracts.Persistence.ServiceRequests;
using CarCare.Core.Domain.Entities.Identity;
using CarCare.Core.Domain.Entities.Orders;
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
			return await _dbContext.Users.Where(t => t.IsActive == true && t.Type == Types.Technical).Where(t => t.ServiceType!.Id == serviceTypeId).ToListAsync();
		}

	}
}
