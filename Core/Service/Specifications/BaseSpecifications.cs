using DomainLayer.Contracts;
using DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specifications
{
  abstract  public class BaseSpecifications<TEntity, TKey> : ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        protected BaseSpecifications(Expression<Func<TEntity,bool>>? CriteriaExpression)
        {
            Criteria = CriteriaExpression;
        }
      
        public Expression<Func<TEntity, bool>>? Criteria { get; private set; }

        #region Include
        public List<Expression<Func<TEntity, object>>> IncludeExpression { get; } = [];
        protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
      => IncludeExpression.Add(includeExpression);
        #endregion
        #region Sorting
        public Expression<Func<TEntity, object>>? OrderBy { get; private set; }

        public Expression<Func<TEntity, object>>? OrderByDescending { get; private set; }
        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExp)
            => OrderBy = orderByExp;
        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescExp) 
            => OrderByDescending = orderByDescExp;
        #endregion

    }
}
