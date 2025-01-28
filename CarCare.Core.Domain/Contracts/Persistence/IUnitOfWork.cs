using CarCare.Core.Domain.Common;
using CarCare.Core.Domain.Contracts.Persistence.Vehicles;

namespace CarCare.Core.Domain.Contracts.Persistence
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>()
            where TEntity : BaseEntity<TKey> where TKey : IEquatable<TKey>;

        IVehicleRepository VehicleRepository { get; }
        public Task<int> CompleteAsync();

    }
}
