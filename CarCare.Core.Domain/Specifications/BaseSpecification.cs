using CarCare.Core.Domain.Common;
using CarCare.Core.Domain.Contracts.Specifications;
using System.Linq.Expressions;

namespace CarCare.Core.Domain.Specifications
{
    public abstract class BaseSpecification<TEntity, TKey> : ISpecification<TEntity, TKey>
        where TEntity : BaseEntity<TKey>
        where TKey : IEquatable<TKey>
    {
        public Expression<Func<TEntity, bool>>? Criteria { get; set; }
        public List<Expression<Func<TEntity, object>>> Includes { get; set; } = new();
        public Expression<Func<TEntity, object>>? OrderBy { get; set; } = null;
        public Expression<Func<TEntity, object>>? OrderByDesending { get; set; } = null;
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnabled { get; set; }

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

        private protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpresstion)
        {
            OrderBy = orderByExpresstion;
        }

        private protected void AddOrderByDesc(Expression<Func<TEntity, object>> orderByDescExpresstion)
        {
            OrderByDesending = orderByDescExpresstion;
        }

    }
}
