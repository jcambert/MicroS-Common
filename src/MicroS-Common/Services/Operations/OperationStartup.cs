using Autofac;
using Chronicle;
using MicroS_Common.Dispatchers;
using MicroS_Common.Handlers;
using MicroS_Common.RabbitMq;
using MicroS_Common.Services.Operations.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace MicroS_Common.Services.Operations
{
    public abstract class ServiceStartup : BaseStartup
    {
        public ServiceStartup(IConfiguration configuration) : base(configuration)
        {
        }
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddChronicle();

        }
        public override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);
            builder.RegisterGeneric(typeof(GenericEventHandler<>)).As(typeof(IEventHandler<>));
            builder.RegisterGeneric(typeof(GenericCommandHandler<>)).As(typeof(ICommandHandler<>));
        }

        protected override void SubscribeEventAndMessageBus(IBusSubscriber bus)
        {
            base.SubscribeEventAndMessageBus(bus);
            bus.SubscribeAllMessages(true, Assembly.GetEntryAssembly(),Assembly.GetExecutingAssembly());
        }
        protected override Type DomainType => null;
    }
}
