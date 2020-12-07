using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class ExceptionMiddleware //middleware class
    {
        private readonly RequestDelegate _next; //RequestDelegate: a function that can process
                                                //an HTTP request
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;//Provides information about the hosting environment 
                                                //an application is running in.
        public ExceptionMiddleware(RequestDelegate next, 
        ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {

            _env = env;
            _logger = logger;
            _next = next;
        }

        /* exception handling method, use to replace the build-in exception
         middleware in startup class */
         public async Task InvokeAsync(HttpContext context)
         {
             try
             {
                 //if no exception, then pass the request to the next middleware
                 await _next(context);
             }
             catch (Exception ex){
                 _logger.LogError(ex, ex.Message);
                 //write our own response into the context response so that
                 //we can send it to the client
                 context.Response.ContentType = "application/json"; //send response as json type
                 //set the statusCode to internalServerError status code
                 context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                 //check if in development mode -> send response
                 var response = _env.IsDevelopment()
                    ? new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, 
                    ex.StackTrace.ToString())
                    : new ApiException((int)HttpStatusCode.InternalServerError, ex.Message,
                    ex.StackTrace.ToString());
                    
                    //to make JsonResponse consistent with other response --> set to camelCase
                    var options = new JsonSerializerOptions{PropertyNamingPolicy = 
                    JsonNamingPolicy.CamelCase};

                    //serialize response to JSON format
                    var jsonResponse = JsonSerializer.Serialize(response, options);
                    
                    await context.Response.WriteAsync(jsonResponse); //write jsonResponse into response body

                 
             }
         }
        
}
}