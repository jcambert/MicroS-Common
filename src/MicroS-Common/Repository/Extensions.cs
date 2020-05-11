using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Reflection;
namespace MicroS_Common.Repository
{
    public static class Extensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, params Assembly[] assemblies)
        {
            var typesWithMyAttribute_ =
                from a in assemblies
                from t in a.GetTypes()
                let attributes = t.GetCustomAttributes(typeof(RepositoryAttribute), true)
                where attributes != null && attributes.Length > 0
                select new { Type = t, Attributes = attributes.Cast<RepositoryAttribute>() };
            foreach (var item in typesWithMyAttribute_)
            {
                services.AddScoped(item.Attributes.First().Type, item.Type);
            }
            return services;
        }
    }
}
