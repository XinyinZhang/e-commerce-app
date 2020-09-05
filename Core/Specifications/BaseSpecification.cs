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

        //a method that allows us to add include statement to our Include field(a list of expressions)
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
    //return example: {x => x.ProductBrand, x => x.ProductType}
    
        
    }
}