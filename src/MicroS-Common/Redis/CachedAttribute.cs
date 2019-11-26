using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace MicroS_Common.Redis
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class CachedAttribute : Attribute, IAsyncActionFilter
    {
        public const int DEFAULT_TIME_TO_LIVE = 60;
        private readonly int? _timeToLiveSeconds;
        public CachedAttribute()
        {

        }
        public CachedAttribute( int timelive)
        {
            _timeToLiveSeconds = timelive;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
       {
            var redisOptions = context.HttpContext.RequestServices.GetRequiredService(typeof(RedisOptions)) as RedisOptions;
            if (!redisOptions.Enabled)
            {
                await next();
                return;
            }
            

            var cacheService= context.HttpContext.RequestServices.GetRequiredService(typeof(IResponseCacheService)) as IResponseCacheService;
            var cacheKey = GenerateCacheKeyfromRequest(context.HttpContext.Request);
            var cachedResponse = await cacheService.GetCachedResponseAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedResponse))
            {
                var contentResult = new ContentResult()
                {
                    Content=cachedResponse,
                    ContentType= "application/json; charset=utf-8",
                    StatusCode=200
                };
                context.Result = contentResult;
                return;
            }

            var executedContext = await next();
            if(executedContext.Result is OkObjectResult okresult)
            {
                int timeToLiveSeconds = _timeToLiveSeconds ?? redisOptions.TimeLive ?? DEFAULT_TIME_TO_LIVE;
                await cacheService.CacheResponseAsync(cacheKey, okresult.Value, TimeSpan.FromSeconds(timeToLiveSeconds));
            }
        }

        private string GenerateCacheKeyfromRequest(HttpRequest request)
        {
            var keyBuilder = new StringBuilder();
            keyBuilder.Append($"{request.Path}");
            foreach (var (key,value) in request.Query.OrderBy(x=>x.Key))
            {
                keyBuilder.Append($"|{key}-{value}");
            }
            return keyBuilder.ToString();
        }
    }
}
