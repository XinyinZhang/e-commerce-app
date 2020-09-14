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

         //expression inside Orderby
         Expression<Func<T, object>> OrderBy{get;}
         
         //expression inside OrderbyDescending
         Expression<Func<T, object>> OrderByDescending{get;}

         int Take{get;} //take a certain amount of products
         int Skip{get;} //skip a certain amount of products
         //ex: if we have 10 products --> take the first 5 and skip null
         bool IsPagingEnabled{get;} 
         
    }
}