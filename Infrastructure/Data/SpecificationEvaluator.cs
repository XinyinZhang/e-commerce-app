using System.Linq;
using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,
        ISpecification<TEntity> spec)
        {
            //spec contains 2 fields
            //field 1: a Criteria expression: x => x.Id == id
            //field 2: a list of expression
                // {x => x.ProductBrand, x => x.ProductType}
            var query = inputQuery;
            //evaluate what's inside the ISpecification
            if (spec.Criteria != null){
                query = query.Where(spec.Criteria); //ex: p => p.ProductTypeId == id
                
            }
            if(spec.OrderBy != null){ //ex: OrderBy = {x => x.Name}
                query = query.OrderBy(spec.OrderBy);
            }
            if(spec.OrderByDescending != null){
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            //check if the client want to apply paging
            if(spec.IsPagingEnabled)
            {
                //skip number = spec.Skip elements
                //take number = spec.Take elements
                query = query.Skip(spec.Skip).Take(spec.Take);
            }


            //accumulate all includes into an expression
             // {x => x.ProductBrand, x => x.ProductType}
             //return: _context.Products.Include(p => p.ProductBrand).Include(p => p.ProductType)
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
            return query;

        }
    }
}