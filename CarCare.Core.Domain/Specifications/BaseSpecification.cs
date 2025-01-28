using CarCare.Core.Domain.Common;
using CarCare.Core.Domain.Contracts.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Core.Domain.Specifications
{
	public class BaseSpecification<TEntity, TKey> : ISpecification<TEntity, TKey>
		where TEntity : BaseAuditableEntity<TKey>
		where TKey : IEquatable<TKey>
	{
		public Expression<Func<TEntity, bool>>? Criteria { get; set; }
		public List<Expression<Func<TEntity, object>>> Includes { get; set; } = new();

		private protected virtual void AddIncludes()
		{

		}

		public BaseSpecification()
		{

		}

		public BaseSpecification(Expression<Func<TEntity, bool>>? criteria)
		{
			Criteria = criteria;
		}

		public BaseSpecification(TKey id)
		{
			Criteria = E => E.Id.Equals(id);
		}

	}
}
