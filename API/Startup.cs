using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using AutoMapper;
using API.Helpers;
using API.Middleware;
using API.Extensions;

namespace API
{
    public class Startup
    {
        private readonly IConfiguration _config;
        public Startup(IConfiguration config)
        {
            _config = config;
            
        }



        // This method gets called by the runtime. Use this method to add services to the container.
        //any services that we want to add to our application and we want to make
        //available to other parts of our application, we add it inside this method
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddControllers();
            //add StoreContext as a service to handle data transferring between application and DB
            services.AddDbContext<StoreContext>(x 
            => x.UseSqlite(_config.GetConnectionString("DefaultConnection")));

            services.AddApplicationServices();
            //add AutoMapper service
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddSwaggerDocumentation();
            //enable CORS so that it can be used by the application
            services.AddCors(opt => 
            {
                opt.AddPolicy("CorsPolicy", policy => 
                {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                    //Angular client的localhost: port 4200
                    //this will allow any methods/headers comes from localhost:4200 to access the data
                });
            });
          
 
        }

        // This method gets called by the runtime. 
        //Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //use our own exception middleware
            app.UseMiddleware<ExceptionMiddleware>();

            //when a request comes to API server but we don't have an endpoint for this
            //request(404 error is generated),the statusCode middleware catches it, and
            //re-executes the pipeline using /error/404 
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            //{0}: a placeholder which will be replaced with the actual code integer(e.g: 404)
            //when the pipeline is re-executed

            app.UseHttpsRedirection(); //redirect HTTP requests to HTTPS

            app.UseRouting();

            app.UseStaticFiles(); //configure a middleware to enable static file serving
                                 //(image sending) for this pipeline

            app.UseCors("CorsPolicy");   //configure a middleware to enable Cors policy

            app.UseAuthorization(); //configure a middleware that will be able to
                                    //authenticate and authorize the incoming request
            
            app.UseSwaggerDocumentation();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
