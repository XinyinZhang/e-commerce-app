using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification()
        {
            
        }
        public BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
           
        }

        //field Criteria is an Expression,ex: x => x.Id == id
        public Expression<Func<T, bool>> Criteria {get;}

        //field Includes: a list of expression
        public List<Expression<Func<T, object>>> Includes {get;} =
        new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderBy {get; private set;}

        public Expression<Func<T, object>> OrderByDescending {get; private set;}

        public int Take {get; private set;}

        public int Skip {get; private set;}

        public bool IsPagingEnabled {get; private set;}

        //a method that allows us to add include statement to our Include field(a list of expressions)
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
        //return example: {x => x.ProductBrand, x => x.ProductType}
    
        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
            //OrderBy = {x => x.Name}
        }

        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }


    }
}