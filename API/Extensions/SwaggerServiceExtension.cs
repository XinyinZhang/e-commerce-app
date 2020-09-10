using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API.Extensions
{
    public static class SwaggerServiceExtension
    {
        //extend IServiceCollection to move all the swagger configuration 
        // from startup to it's own extension class in here
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            
            services.AddSwaggerGen(c => 
            {
              /* SwaggerGen:  a Swagger generator that builds SwaggerDocument 
              objects directly from your routes, controllers, and models. 
              It's typically combined with the Swagger endpoint middleware to 
              automatically expose Swagger JSON.
              */  
                c.SwaggerDoc("v1", new OpenApiInfo {
                    Title = "SkiNet API", Version = "v1"});
            });
            return services;
        }

        //create an extension for the swag middleware
        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
        {
            //enable the middleware for serving generated Swagger as a JSON endpoint
            app.UseSwagger();
            
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => 
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SkiNet API v1");
            });
            return app;
        }

    }
}