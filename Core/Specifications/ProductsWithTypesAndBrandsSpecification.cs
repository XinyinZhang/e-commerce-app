using Core.Entities;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpecification : BaseSpecification<Product>
    {
        public ProductsWithTypesAndBrandsSpecification(ProductSpecParams productParams)
        : base(x =>                                     
        (string.IsNullOrEmpty(productParams.Search) || x.Name.ToLower().Contains(productParams.Search)) &&
        (!productParams.BrandId.HasValue || x.ProductBrandId == productParams.BrandId) && 
        (!productParams.TypeId.HasValue || x.ProductTypeId == productParams.TypeId)
        )
        {
            AddInclude(x => x.ProductBrand); //when return a product, also return its brand
            AddInclude(x => x.ProductType); //when return a product, also return its type
            AddOrderBy(x => x.Name);
                                //skip
            ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), 
            productParams.PageSize); 
            //take
            if(!string.IsNullOrEmpty(productParams.Sort))
            {
                switch(productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(n => n.Name);
                        break;
                }
                
                
            }
        }

        public ProductsWithTypesAndBrandsSpecification(int id) //return a single product with the specified id
        : base(x => x.Id == id)
        {
            AddInclude(x => x.ProductBrand);
            AddInclude(x => x.ProductType);
        }
    }
}