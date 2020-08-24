using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;
        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

       
        public async Task<Product> GetProductByIdAsync(int id)
        {
          return await _context.Products
                .Include(p => p.ProductBrand)
                .Include(p => p.ProductType)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync()
        {
            //make it async so that it will not block
            //the threads that is running on until the result is grabbed from database
            //instead of waiting for lists to come back, it pass the request to a delegate, that
            //delegate is going to handle the request, in the meantime this thread can go and
            //handle other things; 
        
            //Include: eager loading is used to include a query that tell Entity Framework 
            //we want to load a navigation property alongside the entity that we're returning
            return await _context.Products
                   .Include(p => p.ProductBrand)
                   .Include(p => p.ProductType)
                   .ToListAsync();
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }
    }
}