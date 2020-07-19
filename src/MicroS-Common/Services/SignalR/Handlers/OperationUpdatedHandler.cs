using MicroS_Common.Dispatchers.Operations.Events;
using MicroS_Common.Handlers;
using MicroS_Common.RabbitMq;
using MicroS_Common.Services.SignalR.Services;
using System.Threading.Tasks;

namespace MicroS_Common.Services.SignalR.Handlers
{
    public class OperationUpdatedHandler : SignalRBaseHandler,IEventHandler<OperationPending>,
        IEventHandler<OperationCompleted>, IEventHandler<OperationRejected>
    {
        

        public OperationUpdatedHandler(IHubService hubService):base(hubService)
        {
            
        }

        public async Task HandleAsync(OperationPending @event, ICorrelationContext context)
            => await HubService.PublishOperationPendingAsync(@event);

        public async Task HandleAsync(OperationCompleted @event, ICorrelationContext context)
            => await HubService.PublishOperationCompletedAsync(@event);

        public async Task HandleAsync(OperationRejected @event, ICorrelationContext context)
            => await HubService.PublishOperationRejectedAsync(@event);
    }
}
