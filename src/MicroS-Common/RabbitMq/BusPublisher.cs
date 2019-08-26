using System.Threading.Tasks;
using MicroS_Common.Messages;
using RawRabbit;
using RawRabbit.Enrichers.MessageContext;

namespace MicroS_Common.RabbitMq
{
    /// <summary>
    /// @see IBusPublish
    /// </summary>
    public sealed class BusPublisher : IBusPublisher
    {
        private readonly IBusClient _busClient;

        public BusPublisher(IBusClient busClient)
        {
            this._busClient = busClient;
        }

        public async Task PublishAsync<TEvent>(TEvent @event, ICorrelationContext context) where TEvent : IEvent
            => await _busClient.PublishAsync(@event, ctx => ctx.UseMessageContext(context));

        public async Task SendAsync<TCommand>(TCommand command, ICorrelationContext context) where TCommand : ICommand
             => await _busClient.PublishAsync(command, ctx => ctx.UseMessageContext(context));
    }
}