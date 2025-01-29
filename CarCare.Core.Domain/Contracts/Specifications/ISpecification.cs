using CarCare.Core.Domain.Common;
using System.Linq.Expressions;

namespace CarCare.Core.Domain.Contracts.Specifications
{
    public interface ISpecification<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public Expression<Func<TEntity, bool>>? Criteria { get; set; }
        public List<Expression<Func<TEntity, object>>> Includes { get; set; }


    }
}
