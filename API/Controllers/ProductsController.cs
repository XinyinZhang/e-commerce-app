
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Core.Entities;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")] //specify URL pattern for a controller

    public class ProductsController : ControllerBase
    //this controller class derive from ControllerBase class that
    //provides many properties and methods that are useful for handling HTTP requests
    {
        private readonly StoreContext _context;
        public ProductsController(StoreContext context)
        //inject StoreContext into ProductController so that
        //we can use the method within StoreContext
        {
            _context = context;

        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            //make it async so that it will not block
            //the threads that is running on until the result is grabbed from database
            //instead of waiting for lists to come back, it pass the request to a delegate, that
            //delegate is going to handle the request, in the meantime this thread can go and
            //handle other things; 
        
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {

            return await _context.Products.FindAsync(id);
        }
    }
}