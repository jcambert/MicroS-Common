﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroS_Common.Fabio
{
    public static class Extensions
    {
        public static IServiceCollection AddFabio(this IServiceCollection services)
        {
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                services.Configure<FabioOptions>(configuration.GetSection("fabio"));
            }

            services.AddTransient<FabioMessageHandler>();
            services.AddHttpClient<IFabioHttpClient, FabioHttpClient>()
                .AddHttpMessageHandler<FabioMessageHandler>();

            return services;
        }
    }
}
