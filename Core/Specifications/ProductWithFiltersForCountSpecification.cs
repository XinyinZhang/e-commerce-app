using Core.Entities;

namespace Core.Specifications
{
    // the specification that only filter the product by brandId and typeId(do not do any sorting,paging or other modification)
    //used by productsController to count the number of items after the filter has been applied
    //ex: 符合 brandId = 1， typeId = 3的product备选有多少个
    //if do not apply any filter(do not set brandId & typeId) --> count = 18 --> since there are 18 products in total
    
    public class ProductWithFiltersForCountSpecification : BaseSpecification<Product>
    {
        // A specification that specify the collection of products to return should have the
        //specific BrandId and TypeId
        public ProductWithFiltersForCountSpecification(ProductSpecParams productParams) : 
        base(x => 
        (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
        (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) && 
        (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId)
        )
        {
        }
    }
}