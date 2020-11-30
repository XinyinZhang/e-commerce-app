using System;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface IResponseCacheService
    {
         // used to actually cash any responses; when we receive some data
         // from database, we're going to cash that response into memory(redis)
         // response: what we save to redis and return to client
         // timeTOLive: amount of time to live inside redis
         Task CacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);
         // return the cashe response with specific cashKey
         Task<string> GetCashedResponseAsync(string cacheKey);
         

    }
}