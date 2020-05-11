using Autofac;
using MicroS_Common.Domain;
using MicroS_Common.Messages;
using MicroS_Common.RabbitMq;
using System;
using System.Linq;
using System.Reflection;

namespace MicroS_Common.Dispatchers
{
    public static class Extensions
    {
        public static void AddDispatchers(this ContainerBuilder builder)
        {
            builder.RegisterType<CommandDispatcher>().As<ICommandDispatcher>();
            builder.RegisterType<Dispatcher>().As<IDispatcher>();
            builder.RegisterType<QueryDispatcher>().As<IQueryDispatcher>();
        }
        public static void SubscribeOnRejected(this IBusSubscriber bus, params Assembly[] assemblies)
        {
            var typesWithMyAttribute =
                from a in assemblies
                from t in a.GetTypes()
                let attributes = t.GetCustomAttributes(typeof(OnRejectedAttribute), true)
                where attributes != null && attributes.Length > 0
                select new { Type = t, Attributes = attributes.Cast<OnRejectedAttribute>() };
            foreach (var item in typesWithMyAttribute)
            {
                var type = item.Attributes.First().Type;
                bus.SubscribeCommandByType(item.Type, onError: (c, e) => {
                    var o = (BaseRejectedEvent)c;
                    return (IRejectedEvent)Activator.CreateInstance(type, o.Id, o.Reason, o.Code);
                });

            }
        }
    }
}
