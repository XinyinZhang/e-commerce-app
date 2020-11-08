using System;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API
{
    public class Program
    {
        public static async Task Main(string[] args) //starting the project
        {
            var host = CreateHostBuilder(args).Build();
            /* because we are outside of our service container in the startup class, 
            we don't have the control over the life time of when we create the 
            instance of our context

            using statement:
            any code runs inside of this is going to be disposed of as soon as we 
            finish with the methods inside it, we don't need to worry about
             cleaning up by ourselves*/
            using (var scope = host.Services.CreateScope()){
                var services = scope.ServiceProvider;
        //goal: when start our project, create the database using any migrations we have
            //loggerFactory:
                //用来create logger class的instance
            // logger class: 用来log any error message into our console
                var loggerFactory = services.GetRequiredService<ILoggerFactory>();
                try
                {
                    var context = services.GetRequiredService<StoreContext>();
                    // MigrateAsync(): asynchronously 将context里的任何migrations apply to database，
                    // 如果database不存在，create一个database
                    await context.Database.MigrateAsync();
                    //seed the data into database when application starts
                    await StoreContextSeed.SeedAsync(context, loggerFactory);


                    // seed the identity data into database once the application starts
                    // 1. get a userManager service
                    var userManager = services.GetRequiredService<UserManager<AppUser>>();
                    var identityContext = services.GetRequiredService<AppIdentityDbContext>();
                    // 2. asynchronously将identityContext里的任何migrations apply到
                    // database中，如果database不存在，create an identity database
                    await identityContext.Database.MigrateAsync();
                    // seed the identity data into database once the application starts,
                    // pass in 新create好的userManager to help store the data into database
                    await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
                }
                catch(Exception ex)
                {
                    var logger = loggerFactory.CreateLogger<Program>();
                    logger.LogError(ex, "An error occured during migration");

                }
                

            }
            host.Run();

            
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
