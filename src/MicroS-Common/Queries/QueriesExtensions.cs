using MicroS_Common.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
#if DEBUG

#endif
namespace MicroS_Common
{
    internal class PropertyWithFormatAttribute
    {
        public PropertyInfo Property { get; set; }
        public FormatAttribute Attribute { get; set; }

        public void Format<T>(T query) where T : IQuery
        {
            var value = Property.GetGetMethod().Invoke(query, null) as string;
            if (string.IsNullOrEmpty(value)) return;
            string formatted = Attribute.Format(value.ToString());
            Property.GetSetMethod().Invoke(query, new object[] { formatted });
        }
    }
    public class QueryService
    {
        private readonly ILogger<QueryService> _logger;
        public QueryService(ILogger<QueryService> logger)
        {
            this._logger = logger;
        }
        Dictionary<Type, List<PropertyWithFormatAttribute>> _cache = new Dictionary<Type, List<PropertyWithFormatAttribute>>();
        internal void Register<T>(T query) where T : IQuery
        {
            var properties = query.GetType().GetProperties().Where(p => Attribute.IsDefined(p, typeof(FormatAttribute)));
            var props = from p in query.GetType().GetProperties()
                        let attr = p.GetCustomAttributes(typeof(FormatAttribute), true)
                        where attr.Length == 1
                        select new PropertyWithFormatAttribute() { Property = p, Attribute = attr.First() as FormatAttribute };

            if (props.Count() > 0)
            {
                _logger.LogInformation($"Register Query {typeof(T)}");
                _cache[query.GetType()] = props.ToList();

            }
        }
        public void Format<T>(T query) where T : IQuery
        {
            if (!_cache.ContainsKey(query.GetType())) return;
            foreach (var prop in _cache[query.GetType()])
            {

                prop.Format(query);

            }
        }
    }
    public static class QueriesExtensions
    {
        public static void AddQueries(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddSingleton<QueryService>();

            foreach (var @assembly in assemblies)
            {
                var types = assembly.GetTypes().Where(t => typeof(IQuery).IsAssignableFrom(t));
                foreach (var @type in types)
                {
                    services.AddTransient(typeof(IQuery), @type);

                }

            }
        }
        public static void UseQueries(this IApplicationBuilder app)
        {
            var service = app.ApplicationServices.GetRequiredService<QueryService>();
            var queries = app.ApplicationServices.GetServices<IQuery>();
            foreach (var query in queries)
            {
                service.Register(query);
            }

        }

    }
}
