
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class StoreContext : DbContext
    //with entity framework, we do not direct query our database, instead
    //we use DPcontext method to query our database
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }
        //products: name of the table
        public DbSet<Product> Products { get; set; }
    }
}