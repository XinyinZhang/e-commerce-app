
using System.Reflection;
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

        public DbSet<ProductBrand> ProductBrands { get; set; }

        public DbSet<ProductType> ProductTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            //OnModelCreating is called by the framework when your context is 
            //first created and responsible for build the model and its mapping in memory
            //we can override this method to add our own configurations
            base.OnModelCreating(modelBuilder);
            //If we have lots of EntityConfiguration classes, then we can assign all of them using
            //ApplyConfigurationFromAssembly, instead of assign one by one
            //ApplyConfigurationFromAssembly scan the given assembly  for all types that implement IEntityTypeConfiguration, 
            //and registers each one automatically. 
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}