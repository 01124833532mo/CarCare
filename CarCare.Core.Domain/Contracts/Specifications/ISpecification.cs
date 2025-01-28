using CarCare.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CarCare.Core.Domain.Contracts.Specifications
{
	public interface ISpecification<TEntity, TKey>
		where TEntity : BaseAuditableEntity<TKey>
		where TKey : IEquatable<TKey>
	{
		public Expression<Func<TEntity, bool>>? Criteria { get; set; }
		public List<Expression<Func<TEntity, object>>> Includes { get; set; }


	}
}
