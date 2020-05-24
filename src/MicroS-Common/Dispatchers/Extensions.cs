using Autofac;
using MicroS_Common.Dispatchers.Operations.Events;
using MicroS_Common.Domain;
using MicroS_Common.Messages;
using MicroS_Common.RabbitMq;
using System;
using System.Collections.Generic;
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
        public static IBusSubscriber  SubscribeOnRejected(this IBusSubscriber bus, params Assembly[] assemblies)
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
            return bus;
        }

        private static readonly ISet<Type> ExcludedMessages = new HashSet<Type>(new[]
        {
            typeof(OperationPending),
            typeof(OperationCompleted),
            typeof(OperationRejected)
        });

        public static IBusSubscriber SubscribeAllMessages(this IBusSubscriber subscriber,bool excludeOperationsMessages=false, params Assembly[] assemblies)
            => subscriber
            .SubscribeAllCommands(excludeOperationsMessages,assemblies)
            .SubscribeAllEvents(excludeOperationsMessages,assemblies);

        private static IBusSubscriber SubscribeAllCommands(this IBusSubscriber subscriber, bool excludeOperationsMessages = false, params Assembly[] assemblies)
            => subscriber.SubscribeAllMessages<ICommand>(nameof(IBusSubscriber.SubscribeCommand), excludeOperationsMessages, assemblies);

        private static IBusSubscriber SubscribeAllEvents(this IBusSubscriber subscriber, bool excludeOperationsMessages = false, params Assembly[] assemblies)
            => subscriber.SubscribeAllMessages<IEvent>(nameof(IBusSubscriber.SubscribeEvent), excludeOperationsMessages, assemblies);

        private static IBusSubscriber SubscribeAllMessages<TMessage>
            (this IBusSubscriber subscriber, string subscribeMethod, bool excludeOperationsMessages = false, params Assembly[] assemblies)
        {
            var ass = new List<Assembly>(assemblies);
            //ass.Insert(0, MessagesAssembly);
            ass.ForEach(assembly =>
            {
                var messageTypes = assembly
                .GetTypes()
                .Where(t => t.IsClass && typeof(TMessage).IsAssignableFrom(t))
                .Where(t => excludeOperationsMessages && !ExcludedMessages.Contains(t))
                .ToList();

                messageTypes.ForEach(mt => subscriber.GetType()
                    .GetMethod(subscribeMethod)
                    .MakeGenericMethod(mt)
                    .Invoke(subscriber,
                        new object[] { mt.GetCustomAttribute<MessageNamespaceAttribute>()?.Namespace, null, null }));
            });


            return subscriber;
        }
    }
}
