using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
        public CachedAttribute(int timelive)
        {
            _timeToLiveSeconds = timelive;
        }
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var redisOptions = context.HttpContext.RequestServices.GetRequiredService(typeof(RedisOptions)) as RedisOptions;
            if (!redisOptions.Enabled || !redisOptions.UseCache)
            {
                await next();
                return;
            }
#if DEBUG
          //  if (context.HttpContext.Request.Path.Value == "/spid/resultat_equipe_classement")
          //      Debugger.Break();
#endif
            var logger = context.HttpContext.RequestServices.GetRequiredService(typeof(ILogger<CachedAttribute>)) as ILogger<CachedAttribute>;
            var cacheService = context.HttpContext.RequestServices.GetRequiredService(typeof(IResponseCacheService)) as IResponseCacheService;
            var cacheKey = GenerateCacheKeyfromRequest(context.HttpContext.Request);
            var cachedResponse = await cacheService.GetCachedResponseAsync(cacheKey);
            if (!string.IsNullOrEmpty(cachedResponse))
            {
                var cacheValue = JsonSerializer.Deserialize<CacheValue>(cachedResponse);
                logger.LogInformation($"Get {context.HttpContext.Request.Path.Value} from cache");
                var contentResult = new ContentResult()
                {
                    Content = JsonSerializer.Serialize<object>(cacheValue.Value),
                    ContentType = "application/json; charset=utf-8",
                    StatusCode = 200
                };
                context.Result = contentResult;
                context.HttpContext.Response.Headers["link"] = cacheValue.Link;
                context.HttpContext.Response.Headers["X-Total-Count"] = cacheValue.Total;
                return;
            }

            var executedContext = await next();
            if (executedContext.Result is OkObjectResult okresult)
            {
                int timeToLiveSeconds = _timeToLiveSeconds ?? redisOptions.TimeLive ?? DEFAULT_TIME_TO_LIVE;
                //await cacheService.CacheResponseAsync(cacheKey, okresult.Value, TimeSpan.FromSeconds(timeToLiveSeconds));
                var headerLink = context.HttpContext.Response.Headers["link"];
                var totalCount = context.HttpContext.Response.Headers["X-Total-Count"];
                logger.LogInformation($"Save {context.HttpContext.Request.Path.Value} to cache");

                await cacheService.CacheResponseAsync($"{cacheKey}", new CacheValue() { Value = okresult.Value, Link = headerLink, Total = totalCount }, TimeSpan.FromSeconds(timeToLiveSeconds));
            }
        }
        private class CacheValue
        {
            public object Value { get; set; }
            public string Link { get; set; }
            public string Total { get; set; }
        }
        private string GenerateCacheKeyfromRequest(HttpRequest request)
        {
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
