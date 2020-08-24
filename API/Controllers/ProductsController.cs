
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Core.Entities;
using Core.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] //specify URL pattern for a controller

    public class ProductsController : ControllerBase
    //this controller class derive from ControllerBase class that
    //provides many properties and methods that are useful for handling HTTP requests
    {
        private readonly IProductRepository _repo;

        public ProductsController(IProductRepository repo)
        {
            _repo = repo;


        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            //make it async so that it will not block
            //the threads that is running on until the result is grabbed from database
            //instead of waiting for lists to come back, it pass the request to a delegate, that
            //delegate is going to handle the request, in the meantime this thread can go and
            //handle other things; 

            var products = await _repo.GetProductsAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {

            return await _repo.GetProductByIdAsync(id);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            
            var productBrands = await _repo.GetProductBrandsAsync();
            return Ok(productBrands);
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes()
        {
            
            var productTypes = await _repo.GetProductTypesAsync();
            return Ok(productTypes);
        }




    }
}