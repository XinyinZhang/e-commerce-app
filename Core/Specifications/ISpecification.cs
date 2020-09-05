using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Specifications
{
    public interface ISpecification<T>
    {
        //Criteria: the Criteria of the thing we will get
        //ex: the product of typeid 1
         Expression<Func<T, bool>> Criteria{get;}
         
         List<Expression<Func<T, object>>> Includes{get;}
         
    }
}