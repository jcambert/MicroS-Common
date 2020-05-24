using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;

namespace MicroS_Common.Services.SignalR
{
    public static class Extensions
    {
        
        public static string ToUserGroup(this Guid userId)=> $"users:{userId}";

        public static IServiceCollection AddSignalr(this IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            services.Configure<SignalrOptions>(configuration.GetSection(SignalrOptions.SECTION));
            var options = configuration.GetOptions<SignalrOptions>(SignalrOptions.SECTION);
            services.AddSingleton(options);

            if (options.Enabled)
            {
                var bld = services.AddSignalR(c=> {
                    
                    });
                /*if (!options.Backplane.Equals("redis", StringComparison.InvariantCultureIgnoreCase))
                {
                    return services;
                }
                var redisOptions = configuration.GetOptions<RedisOptions>("redis");
                services.AddRedis(redisOptions.ConnectionString);*/
            }
            return services;
        }
    }
}
