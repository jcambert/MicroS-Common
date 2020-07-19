using Autofac;
using MicroS_Common.Dispatchers.Operations.Events;
using MicroS_Common.Domain;
using MicroS_Common.Messages;
using MicroS_Common.Mvc;
using MicroS_Common.RabbitMq;
using MicroS_Common.Types;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using VaultSharp.V1.SecretsEngines.TOTP;

namespace MicroS_Common.Dispatchers
{
    public static class Extensions {

        private static ILoggerFactory Factory = LoggerFactory.Create(bld=> { });
        private static ILogger Logger = Factory.CreateLogger(typeof(Extensions));
        public static void AddDispatchers(this ContainerBuilder builder)
        {
            builder.RegisterType<CommandDispatcher>().As<ICommandDispatcher>();
            builder.RegisterType<Dispatcher>().As<IDispatcher>();
            builder.RegisterType<QueryDispatcher>().As<IQueryDispatcher>();
        }
        public static IBusSubscriber SubscribeOnRejected(this IBusSubscriber bus, params Assembly[] assemblies)
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
                bus.SubscribeCommandByType(item.Type, onError: (c, e) =>
                {
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

        public static IBusSubscriber SubscribeAllMessages(this IBusSubscriber subscriber, bool excludeOperationsMessages = false, params Assembly[] assemblies)
            => subscriber
            .SubscribeAllCommands(excludeOperationsMessages, assemblies)
            .SubscribeAllEvents(excludeOperationsMessages, assemblies);

        public static IBusSubscriber SubscribeAllCommands(this IBusSubscriber subscriber, bool excludeOperationsMessages = false, params Assembly[] assemblies)
            => subscriber.SubscribeAllMessages<ICommand>(nameof(IBusSubscriber.SubscribeCommand), excludeOperationsMessages, assemblies);

        public static IBusSubscriber SubscribeAllEvents(this IBusSubscriber subscriber, bool excludeOperationsMessages = false, params Assembly[] assemblies)
            => subscriber.SubscribeAllMessages<IEvent>(nameof(IBusSubscriber.SubscribeEvent), excludeOperationsMessages, assemblies);

        private static Func<BaseCommand<TKey>, MicroSException, IRejectedEvent> CreateOnRejected<TKey>(Type t)
        {

            Type RejectedType = t.GetCustomAttribute<OnRejectedAttribute>()?.Type;
            if (RejectedType == null) return null;
            try
            {
            var ctor = RejectedType.GetConstructor(new[] { typeof(Guid), typeof(string), typeof(string) });
            return (BaseCommand<TKey> c, MicroSException e) => (IRejectedEvent)ctor.Invoke(new object[] { c.Id, e.Message, e.Code });

            }catch(Exception e)
            {
#if DEBUG
                Debugger.Break();
#endif
            }
            return null;
        }



        private static IBusSubscriber SubscribeAllMessages<TMessage>
            (this IBusSubscriber subscriber, string subscribeMethod, bool excludeOperationsMessages = false, params Assembly[] assemblies)
        {
            var ass = new List<Assembly>(assemblies);
            //ass.Insert(0, MessagesAssembly);
            ass.ForEach(assembly =>
            {
                var messageTypes = assembly
                .GetTypes()
                .Where(t => t.IsClass && !(t.IsAbstract) && typeof(TMessage).IsAssignableFrom(t))
                .Where(t => excludeOperationsMessages && !ExcludedMessages.Contains(t))
                .ToList();

                var m0 = subscriber.GetType().GetMethod(subscribeMethod);
                var m1 = typeof(Extensions).GetMethod("CreateOnRejected",BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                foreach (var mt in messageTypes)
                {
                   // if (mt.Name == "CreateTypeDePropriete")
                     //   Debugger.Break();
                    try
                    {
                        var keyType=mt.GetProperty("Id").GetGetMethod().ReturnType;
                        var m2 = m0.MakeGenericMethod(mt);
                        var p0 = mt.GetCustomAttribute<MessageNamespaceAttribute>()?.Namespace;
                        var m3 = m1.MakeGenericMethod(keyType);
                        var p1 = m3.Invoke(null,new object[] { mt});
                        var p = new object[] { p0, null, p1 };
                        m2.Invoke(subscriber, p);

                    }catch(Exception e)
                    {
                        Logger.LogError(e.Message);
#if DEBUG
                        Debugger.Break();
#endif
                    }
                }

                /*messageTypes.ForEach(mt => 
                    subscriber.GetType()
                    .GetMethod(subscribeMethod)
                    .MakeGenericMethod(mt)
                    .Invoke(subscriber,
                        new object[] {
                            mt.GetCustomAttribute<MessageNamespaceAttribute>()?.Namespace,
                            null,
                            CreateOnRejected(mt) }));*/
            });


            return subscriber;
        }
    }
}
