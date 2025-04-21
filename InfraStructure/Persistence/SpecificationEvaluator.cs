using DomainLayer.Contracts;
using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    static class SpecificationEvaluator
    {
        // Create Query
        // _dbContext.Products.Where(P=>P.id=id).Include(P=>P.ProductBrandd).Include(P=>P.ProductType)
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> InputQuery, ISpecifications<TEntity, TKey> specifications) where TEntity : BaseEntity<TKey>
        {
            var Query = InputQuery;
            if (specifications.Criteria is not null)
            {
                Query = Query.Where(predicate: specifications.Criteria);
            }
            if (specifications.IncludeExpression is not null && specifications.IncludeExpression.Count > 0)
            {
                //foreach (var exp in specifications. IncludeExpressions) 
                // Query Query. Include (exp);
                Query = specifications.IncludeExpression.Aggregate(Query, (CurrentQuery, IncludeExp) => CurrentQuery.Include(IncludeExp));

            }
                    return Query;
        }
    }
}

