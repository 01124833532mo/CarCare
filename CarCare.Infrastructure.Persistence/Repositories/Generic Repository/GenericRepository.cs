using CarCare.Core.Domain.Common;
using CarCare.Core.Domain.Contracts.Persistence;
using CarCare.Core.Domain.Contracts.Specifications;
using CarCare.Infrastructure.Persistence._Data;
using Microsoft.EntityFrameworkCore;
//using Twilio.TwiML.Voice;

namespace CarCare.Infrastructure.Persistence.Generic_Repository
{
	public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey>
		where TEntity : BaseEntity<TKey> where TKey : IEquatable<TKey>

	{
		private readonly CarCarIdentityDbContext _dbContext;

		public GenericRepository(CarCarIdentityDbContext dbContext)
		{
			_dbContext = dbContext;
		}


		public async Task<IEnumerable<TEntity>> GetAllAsync(bool WithTraching = false)
		{
			return WithTraching ? await _dbContext.Set<TEntity>().ToListAsync() : await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
		}
		public async Task<IEnumerable<TEntity>> GetAllWithSpecAsync(ISpecification<TEntity, TKey> Spec, bool WithTraching = false)
		{
			return await ApplySpecifications(Spec).ToListAsync();
		}



		public async Task<TEntity?> GetWithSpecAsync(ISpecification<TEntity, TKey> spec, TKey id)
		{
			return await ApplySpecifications(spec).FirstOrDefaultAsync();
		}

		public async Task<TEntity?> GetAsync(TKey id)
		{
			return await _dbContext.Set<TEntity>().FindAsync(id);


		}


		public async Task AddAsync(TEntity entity)
		{
			await _dbContext.Set<TEntity>().AddAsync(entity);
		}

		public void Delete(TEntity entity)
		{
			_dbContext.Set<TEntity>().Remove(entity);
		}


		public void Update(TEntity entity)
		{
			_dbContext.Set<TEntity>().Update(entity);
		}


		private IQueryable<TEntity> ApplySpecifications(ISpecification<TEntity, TKey> Spec)
		{
			return SpecificationEvaluator<TEntity, TKey>.GetQuery(_dbContext.Set<TEntity>(), Spec);
		}

	}
}
