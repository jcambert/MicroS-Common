using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MicroS_Common.Redis
{
    public static class Extensions
    {
        private static readonly string SectionName = "redis";

        public static IServiceCollection AddRedis(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            services.Configure<RedisOptions>(configuration.GetSection(SectionName));
            var options = configuration.GetOptions<RedisOptions>(SectionName);
            services.AddSingleton(options);
            if (options.Enabled)
            {
                services.AddDistributedRedisCache(o =>
                {
                    o.Configuration = options.ConnectionString;
                    o.InstanceName = options.Instance;
                });
                services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            }
            return services;
        }
    }
}
