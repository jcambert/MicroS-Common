using Chronicle;
using MicroS_Common.Dispatchers;
using MicroS_Common.RabbitMq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        protected override void SubscribeEventAndMessageBus(IBusSubscriber bus)
        {
            base.SubscribeEventAndMessageBus(bus);
            bus.SubscribeAllMessages(true, DomainType.Assembly);
        }
    }
}
