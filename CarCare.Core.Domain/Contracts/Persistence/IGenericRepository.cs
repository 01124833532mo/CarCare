using CarCare.Core.Domain.Common;
using CarCare.Core.Domain.Contracts.Specifications;

namespace CarCare.Core.Domain.Contracts.Persistence
{
    public interface IGenericRepository<TEntity, TKey>
        where TEntity : BaseEntity<TKey> where TKey : IEquatable<TKey>
    {
        Task<IEnumerable<TEntity>> GetAllAsync(bool WithTraching = false);
        Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity, TKey> Spec, bool WithTraching = false);

        Task<TEntity?> GetAsync(TKey id);

        Task<TEntity?> GetWithSpecAsync(ISpecification<TEntity, TKey> spec, TKey id);

        Task<int> GetCountAsync(ISpecification<TEntity, TKey> spec);



        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

    }
}
