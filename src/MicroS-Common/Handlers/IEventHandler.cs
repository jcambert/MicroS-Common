using MicroS_Common.Messages;
using MicroS_Common.RabbitMq;
using System.Threading.Tasks;

namespace MicroS_Common.Handlers
{
    public interface IEventHandler<in TEvent> where TEvent : IEvent
    {
        Task HandleAsync(TEvent @event, ICorrelationContext context);
    }
}
