using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.OrderAggregate;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Data
{
    //purpose: read all data from Json file into database
    //this method will be called by API/Program.cs to seed the data
    // into the database when application starts
    public class StoreContextSeed
    {
        //using static method allows us to call it directly, without creating an instance of the class
        public static async Task SeedAsync(StoreContext context, ILoggerFactory loggerFactory)
        {
            try
            {
                var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                //check if we got any productBrand in our database
                if (!context.ProductBrands.Any())
                {
                    //if there are no ProductBrand in our database, we need to seed
                    //the data into database
                    //1. read data from Json file
                    var brandsData = 
                    File.ReadAllText(path + @"/Data/SeedData/brands.json");
                    //2. deserialize Json --> a list of productBrand object
                    var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsData);
                    

                    foreach (var item in brands)
                    {   //3. add each item of brands to the ProductBrands DbSet in memory
                        context.ProductBrands.Add(item);
                    }

                    //4. save all changes to the database
                    await context.SaveChangesAsync();

                }

                //do the same to seed all ProductType data into database
                if(!context.ProductTypes.Any())
                {
                    var TypeData = 
                    File.ReadAllText(path + @"/Data/SeedData/types.json");
                    var types = JsonSerializer.Deserialize<List<ProductType>>(TypeData);
                    foreach(var item in types)
                    {
                        context.ProductTypes.Add(item);
                    }
                    await context.SaveChangesAsync();
                }

                if(!context.Products.Any())
                {
                    var ProductData = 
                    File.ReadAllText(path + @"/Data/SeedData/products.json");
                    var products = JsonSerializer.Deserialize<List<Product>>(ProductData);
                    foreach(var item in products)
                    {
                        context.Products.Add(item);
                    }
                    await context.SaveChangesAsync();
                }
                // also do the same for seeding deliveryMethod datas
                if (!context.DeliveryMethods.Any())
                {
                    var dmData =
                        File.ReadAllText(path + @"/Data/SeedData/delivery.json");

                    var methods = JsonSerializer.Deserialize<List<DeliveryMethod>>(dmData);

                    foreach (var item in methods)
                    {
                        context.DeliveryMethods.Add(item);
                    }

                    await context.SaveChangesAsync();
                }

            }
            catch(Exception ex)
            {
                var logger = loggerFactory.CreateLogger<StoreContextSeed>();
                logger.LogError(ex.Message);
            }

        }
    }
}