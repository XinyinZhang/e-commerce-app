using System.Linq;
using API.Errors;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace API.Extensions
{
    public static class ApplicationServicesExtensions
    {
        //extend IServiceCollection(bring some configurations from startup to
        //here -- clean up startup)
        public static IServiceCollection AddApplicationServices (this IServiceCollection services)
        {
            //lifetime: every HTTP request
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    /* extract all validation errors and put their error messages into an array*/
                    var errors = actionContext.ModelState /* ModeState stores a collection of values
                    submitted to the server, and to store the validation errors associated with those values*/
                    .Where(e => e.Value.Errors.Count > 0) // fetch all ModelState objects with validation errors
                    .SelectMany(x => x.Value.Errors) //obtain their error messages and put it into an array
                    .Select(x => x.ErrorMessage).ToArray(); 

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors 
                    };
                    return new BadRequestObjectResult(errorResponse);
                    
                };
            });
            return services;

        }
    }
}