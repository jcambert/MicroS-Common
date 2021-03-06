﻿using MicroS_Common.Consul;
using MicroS_Common.Fabio;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RestEase;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace MicroS_Common.RestEase
{
    public static class Extensions
    {
        public static IServiceCollection RegisterAllServiceForwarders(this IServiceCollection services, params Assembly[] assemblies)
        {
            var typesWithMyAttribute_ =
                from a in assemblies
                from t in a.GetTypes()
                let attributes = t.GetCustomAttributes(typeof(ServiceForwarderAttribute), true)
                where attributes != null && attributes.Length > 0
                select new { Type = t, Attributes = attributes.Cast<ServiceForwarderAttribute>() };
            foreach (var item in typesWithMyAttribute_)
            {
                services.RegisterServiceForwarder(item.Type, item.Attributes.First().Name);
            }
            return services;
        }

        public static IServiceCollection RegisterServiceForwarder(this IServiceCollection services,Type type, string serviceName)
        {
            var clientName = type.ToString();
            var options = ConfigureOptions(services);
            switch (options.LoadBalancer?.ToLowerInvariant())
            {
                case "consul":
                    ConfigureConsulClient(services, clientName, serviceName);
                    break;
                case "fabio":
                    ConfigureFabioClient(services, clientName, serviceName);
                    break;
                default:
                    ConfigureDefaultClient(services, clientName, serviceName, options);
                    break;
            }

            services.ConfigureForwarder(type, clientName) ;
            return services;
        }
        public static IServiceCollection RegisterServiceForwarder<T>(this IServiceCollection services, string serviceName)
            where T : class
        {
            return services.RegisterServiceForwarder(typeof(T), serviceName);
            /*var clientName = typeof(T).ToString();
            var options = ConfigureOptions(services);
            switch (options.LoadBalancer?.ToLowerInvariant())
            {
                case "consul":
                    ConfigureConsulClient(services, clientName, serviceName);
                    break;
                case "fabio":
                    ConfigureFabioClient(services, clientName, serviceName);
                    break;
                default:
                    ConfigureDefaultClient(services, clientName, serviceName, options);
                    break;
            }

            ConfigureForwarder<T>(services, clientName);
            return services;*/
        }

        private static RestEaseOptions ConfigureOptions(IServiceCollection services)
        {
            IConfiguration configuration;
            using (var serviceProvider = services.BuildServiceProvider())
            {
                configuration = serviceProvider.GetService<IConfiguration>();
            }

            services.Configure<RestEaseOptions>(configuration.GetSection("restEase"));

            return configuration.GetOptions<RestEaseOptions>("restEase");
        }

        private static void ConfigureConsulClient(IServiceCollection services, string clientName,
            string serviceName)
        {
            services.AddHttpClient(clientName)
                .AddHttpMessageHandler(c =>
                    new ConsulServiceDiscoveryMessageHandler(c.GetService<IConsulServicesRegistry>(),
                        c.GetService<IOptions<ConsulOptions>>(), serviceName, overrideRequestUri: true));
        }

        private static void ConfigureFabioClient(IServiceCollection services, string clientName,
            string serviceName)
        {
            services.AddHttpClient(clientName)
                .AddHttpMessageHandler(c =>
                    new FabioMessageHandler(c.GetService<IOptions<FabioOptions>>(), serviceName));
        }

        private static void ConfigureDefaultClient(IServiceCollection services, string clientName,
            string serviceName, RestEaseOptions options)
        {
            services.AddHttpClient(clientName, client =>
            {
                var service = options.Services.SingleOrDefault(s => s.Name.Equals(serviceName,
                    StringComparison.InvariantCultureIgnoreCase));
                if (service == null)
                {
                    throw new RestEaseServiceNotFoundException($"RestEase service: '{serviceName}' was not found.",
                        serviceName);
                }

                client.BaseAddress = new UriBuilder
                {
                    Scheme = service.Scheme,
                    Host = service.Host,
                    Port = service.Port
                }.Uri;
            });
        }

        private static void ConfigureForwarder(this IServiceCollection services, Type type, string clientName)
        {
            services.AddTransient(type,c => {
                return new RestClient(c.GetService<IHttpClientFactory>().CreateClient(clientName))
                {
                    RequestQueryParamSerializer = new QueryParamSerializer()
                }.For(type); 
            });
                
        }
        private static void ConfigureForwarder<T>(this IServiceCollection services, string clientName) where T : class
        {
            services.AddTransient<T>(c => new RestClient(c.GetService<IHttpClientFactory>().CreateClient(clientName))
            {
                RequestQueryParamSerializer = new QueryParamSerializer()
            }.For<T>());
        }
    }
}
