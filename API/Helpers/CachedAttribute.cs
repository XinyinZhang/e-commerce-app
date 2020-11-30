using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int _timeToLiveSeconds;
        public CachedAttribute(int timeToLiveSeconds)
        {
            _timeToLiveSeconds = timeToLiveSeconds;
        }
        // before request hit the controller, we will execute this method: to check
        // if the response of this specific request is already inside cache memory
        // if so, fetch it from cache memory and return directly(instead of go to Controller again
        // asking for the same result)
        public async Task OnActionExecutionAsync(ActionExecutingContext context,
        ActionExecutionDelegate next)
        {
            // get the reference to cache service
            var cacheService = context.HttpContext.RequestServices
                                .GetRequiredService<IResponseCacheService>();
            // generate cacheKey based on the request itself
            var cacheKey = GenerateCacheKeyFromRequest(context.HttpContext.Request);
            // check if we already store the cache inside Redis database(memory)
            var cacheResponse = await cacheService.GetCashedResponseAsync(cacheKey);

            if(!string.IsNullOrEmpty(cacheResponse)) { 
                // if this is already saved in to memory, wrap into http response type
                // and directly return to client <--- request don't need to bother Controller
                var contentResult = new ContentResult
                {
                    Content = cacheResponse,
                    ContentType = "application/json",
                    StatusCode = 200
                };
                context.Result = contentResult;
                return;
            }
            // if we don't have this response inside our memory, we move the request to controller
            // (to let controller handle the request)
            var executedContext = await next(); // move the request to controller

            if (executedContext.Result is OkObjectResult okObjectResult)
            {
                // put the result into the cache memory, so that the next time someone ask for
                // the same result, we can directly return it from cache
                await cacheService.CacheResponseAsync(cacheKey, okObjectResult.Value, TimeSpan.FromSeconds(_timeToLiveSeconds));
            }


        }

        private static string GenerateCacheKeyFromRequest(HttpRequest request)
        {
            // we want to organize the query string of the request into a specific order
            // so that we always have the same key for the same response
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach (var (key, value) in request.Query.OrderBy(x => x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }

            return keyBuilder.ToString();

        }
    }
}