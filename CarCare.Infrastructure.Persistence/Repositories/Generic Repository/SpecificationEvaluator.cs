using CarCare.Core.Domain.Common;
using CarCare.Core.Domain.Contracts.Specifications;
using Microsoft.EntityFrameworkCore;

namespace CarCare.Infrastructure.Persistence.Generic_Repository
{
    internal static class SpecificationEvaluator<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>
    {

        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity, TKey> specs)
        {
            var query = inputQuery; //dbContext.Set<TEntity>

            if (specs.Criteria is not null)
                query = query.Where(specs.Criteria); //dbContext.Set<TEntity>.Where(E => E.id == id)

            if (specs.OrderByDesending is not null)
                query = query.OrderByDescending(specs.OrderByDesending);

            if (specs.OrderBy is not null)
                query = query.OrderBy(specs.OrderBy);

            if (specs.IsPaginationEnabled)
                query = query.Skip(specs.Skip).Take(specs.Take);


            query = specs.Includes.Aggregate(query, (currentQuery, Include) => currentQuery.Include(Include));
            //dbContext.Set<TEntity>.Where(E => E.id == id).Include(E=>E.Entity)
            //dbContext.Set<TEntity>.Where(E => E.id == id).Include(E=>E.Entity).Include(E=>E.Entity)


            return query;
        }

    }
}
