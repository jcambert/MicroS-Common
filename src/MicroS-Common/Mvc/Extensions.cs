using Lockbox.Client.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace MicroS_Common.Mvc
{
    public static class Extensions
    {
        public static IMvcCoreBuilder AddCustomMvc(this IServiceCollection services, Action<MvcOptions> options = null)
        {
            using (var serviceProvider = services.BuildServiceProvider())
            {
                var configuration = serviceProvider.GetService<IConfiguration>();
                services.Configure<AppOptions>(configuration.GetSection(AppOptions.SECTION));
            }
            
            services.AddSingleton<IServiceId, ServiceId>();
            //services.AddTransient<IStartupInitializer, StartupInitializer>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddInitializers();
            return services
                .AddMvcCore(o =>
               {
                   o.EnableEndpointRouting = false;
                   options?.Invoke(o);
               })

                .AddNewtonsoftJson(o =>
                {
                    o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    o.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                    o.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                    o.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                    o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    o.SerializerSettings.Formatting = Formatting.Indented;
                    o.SerializerSettings.Converters.Add(new StringEnumConverter());
                    o.SerializerSettings.Converters.Add(new StringTrimConverter());
                })
                //.AddJsonFormatters()
                //.AddDefaultJsonOptions()
                .AddDataAnnotations()
                .AddApiExplorer()
                .AddAuthorization();
        }

        public static IServiceCollection AddInitializers(this IServiceCollection services, params Type[] initializers)
        {
            services.AddTransient<IStartupInitializer, StartupInitializer>(c =>
             {
                 var startupInitializer = new StartupInitializer();
                 var services = c.GetServices<IInitializer>();
                 /*var validInitializers = initializers.Where(t => typeof(IInitializer).IsAssignableFrom(t));
                 */foreach (var initializer in services)
                 {
                     startupInitializer.AddInitializer(initializer);
                 }

                 return startupInitializer;
             });
            return services;
        }

        /*  public static IMvcCoreBuilder AddDefaultJsonOptions(this IMvcCoreBuilder builder)
              => builder.AddJsonOptions (o =>
              {
                  o.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                  o.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                  o.SerializerSettings.DateParseHandling = DateParseHandling.DateTimeOffset;
                  o.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;
                  o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                  o.SerializerSettings.Formatting = Formatting.Indented;
                  o.SerializerSettings.Converters.Add(new StringEnumConverter());
              });
              */
        public static IApplicationBuilder UseErrorHandler(this IApplicationBuilder builder)
            => builder.UseMiddleware<ErrorHandlerMiddleware>();

        public static IApplicationBuilder UseAllForwardedHeaders(this IApplicationBuilder builder)
            => builder.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

        public static IApplicationBuilder UseServiceId(this IApplicationBuilder builder)
            => builder.Map("/id", c => c.Run(async ctx =>
            {
                using var scope = c.ApplicationServices.CreateScope();
                var id = scope.ServiceProvider.GetService<IServiceId>().Id;
                await ctx.Response.WriteAsync(id);
            }));

        public static IWebHostBuilder UseLockbox(this IWebHostBuilder builder)
            => builder.ConfigureAppConfiguration((ctx, cfg) =>
            {
                var useLockbox = Environment.GetEnvironmentVariable("USE_LOCKBOX");
                if (useLockbox?.ToLowerInvariant() == "true")
                {
                    cfg.AddLockbox();
                }
            });

        public static T Bind<T>(this T model, Expression<Func<T, object>> expression, object value, ILogger<T> logger = null)
            => model.Bind<T, object>(expression, value);

        public static T BindId<T>(this T model, Expression<Func<T, Guid>> expression, ILogger<T> logger = null)
            => model.Bind<T, Guid>(expression, Guid.NewGuid(), logger);

        private static TModel Bind<TModel, TProperty>(this TModel model, Expression<Func<TModel, TProperty>> expression,
            object value, ILogger<TModel> logger = null)
        {
            if (!(expression.Body is MemberExpression memberExpression))
            {
                memberExpression = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }

            var propertyName = memberExpression.Member.Name.ToLowerInvariant();
            var modelType = model.GetType();
            var fields = modelType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

            /*if (logger != null)
                fields.ToList().ForEach(f =>
                {
                    logger.LogError(f.Name);
                });*/
            var field = fields.SingleOrDefault(x => x.Name.ToLowerInvariant().StartsWith($"<{propertyName}>"));
            if (field == null)
            {
                if (logger != null) logger.LogError($"Cannot bind {propertyName}");
                return model;
            }

            field.SetValue(model, value);

            return model;
        }
    }
}