using CarCare.Core.Domain.Common;
using CarCare.Core.Domain.Contracts.Specifications;
using CarCare.Core.Domain.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Infrastructure.Persistence.Generic_Repository
{
	internal static class SpecificationEvaluator<TEntity, TKey>
		where TEntity : BaseAuditableEntity<TKey>
		where TKey : IEquatable<TKey>
	{

		public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery, ISpecification<TEntity, TKey> specs)
		{
			var query = inputQuery; //dbContext.Set<TEntity>

			if (specs.Criteria is not null)
				query = query.Where(specs.Criteria); //dbContext.Set<TEntity>.Where(E => E.id == id)

			query = specs.Includes.Aggregate(query, (currentQuery, Include) => currentQuery.Include(Include));
			//dbContext.Set<TEntity>.Where(E => E.id == id).Include(E=>E.Entity)
			//dbContext.Set<TEntity>.Where(E => E.id == id).Include(E=>E.Entity).Include(E=>E.Entity)


			return query;
		}

	}
}
