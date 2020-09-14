
using System.Linq;
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

            //check if we are working with SQLite database
            if(Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
            {
                //convert all decimal type to double type, since Sqlite database cannot 
                // order by expression of type decimal
                foreach (var entityType in modelBuilder.Model.GetEntityTypes())
                {
                    var properties = entityType.ClrType.GetProperties()
                    .Where(p => p.PropertyType == typeof(decimal));
                    foreach (var property in properties)
                    {
                        modelBuilder.Entity(entityType.Name).Property(property.Name)
                        .HasConversion<double>();
                    }
                }

            }


        }
    }
}