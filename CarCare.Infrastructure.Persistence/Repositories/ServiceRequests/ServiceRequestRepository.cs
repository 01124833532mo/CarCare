using CarCare.Core.Domain.Contracts.Persistence.ServiceRequests;
using CarCare.Core.Domain.Entities.Identity;
using CarCare.Core.Domain.Entities.Orders;
using CarCare.Infrastructure.Persistence._Data;
using CarCare.Infrastructure.Persistence.Generic_Repository;
using CarCare.Shared.ErrorModoule.Exeptions;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<(ApplicationUser Technician, double Distance)>> GetAvailableTechniciansAsync(int serviceTypeId, double userLongitude, double userLatitude)
        {
            var technicians = await _dbContext.Users
                .Where(t => t.IsActive == true && t.Type == Types.Technical && t.ServiceType!.Id == serviceTypeId)
                .ToListAsync();

            var techniciansWithDistance = technicians
                .Select(t => new
                {
                    Technician = t,
                    Distance = CalculateDistance(userLatitude, userLongitude, t.TechLatitude, t.TechLongitude)
                })
                .OrderByDescending(t => t.Technician.TechRate)
                .ThenBy(t => t.Distance)
                .Select(t => (t.Technician, t.Distance));

            return techniciansWithDistance.ToList();
        }

        private double CalculateDistance(double userLatitude, double userLongitude, double? techLatitude, double? techLongitude)
        {
            if (techLatitude == null || techLongitude == null)
            {
                throw new BadRequestExeption("Technician Do not Have Location ");
            }

            var R = 6371;
            var dLat = ToRadians(techLatitude.Value - userLatitude);
            var dLon = ToRadians(techLongitude.Value - userLongitude);
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(ToRadians(userLatitude)) * Math.Cos(ToRadians(techLatitude.Value)) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            var distance = R * c;
            return distance;
        }
        private double ToRadians(double? angle)
        {
            return Math.PI * (angle ?? 0) / 180.0;
        }

        public async Task<IEnumerable<(ApplicationUser Technical, double Distance)>> GetNearestTechincal(int serviceTypeId, double userLatitude, double userLongitude)
        {
            var techs = await _dbContext.Users
                .Where(t => t.IsActive == true && t.Type == Types.Technical)
                .Where(t => t.ServiceType!.Id == serviceTypeId)
                .ToListAsync();

            var sortedTechs = techs
                .AsEnumerable()
                .Select(t => new
                {
                    Technical = t, // Match the interface tuple element name
                    Distance = CalculateDistance(userLatitude, userLongitude, t.TechLatitude!.Value, t.TechLongitude!.Value)
                })
                .OrderBy(t => t.Distance)
                .Select(t => (t.Technical, t.Distance)) // Match the interface tuple element name
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
