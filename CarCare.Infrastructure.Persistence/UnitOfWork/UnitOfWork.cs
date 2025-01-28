using CarCare.Core.Domain.Common;
using CarCare.Core.Domain.Contracts.Persistence;
using CarCare.Core.Domain.Contracts.Persistence.Vehicles;
using CarCare.Infrastructure.Persistence._Data;
using CarCare.Infrastructure.Persistence.Generic_Repository;
using CarCare.Infrastructure.Persistence.Repositories.Vehicles;
using System.Collections.Concurrent;

namespace CarCare.Infrastructure.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {

        private readonly ConcurrentDictionary<string, object> _repositories;
        private readonly CarCarIdentityDbContext _dbContext;
        private readonly Lazy<IVehicleRepository> _vehicleRepository;

        public UnitOfWork(CarCarIdentityDbContext dbContext)
        {
            _repositories = new ConcurrentDictionary<string, object>();
            _dbContext = dbContext;
            _vehicleRepository = new Lazy<IVehicleRepository>(() => new VehicleRepository(dbContext));
        }

        public IVehicleRepository VehicleRepository => _vehicleRepository.Value;

        public async Task<int> CompleteAsync()
        {
            return await _dbContext.SaveChangesAsync();

        }

        public ValueTask DisposeAsync()
        {
            return _dbContext.DisposeAsync();
        }

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : BaseEntity<TKey>
            where TKey : IEquatable<TKey>
        {

            return (IGenericRepository<TEntity, TKey>)_repositories.GetOrAdd(typeof(TEntity).Name, new GenericRepository<TEntity, TKey>(_dbContext));

        }
    }
}
