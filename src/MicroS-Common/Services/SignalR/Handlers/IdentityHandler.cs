using MicroS_Common.Handlers;
using MicroS_Common.RabbitMq;
using MicroS_Common.Services.Identity.Messages.Events;
using MicroS_Common.Services.SignalR.Services;
using System.Threading.Tasks;

namespace MicroS_Common.Services.SignalR.Handlers
{
   /* public class IdentityHandler : SignalRBaseHandler, IEventHandler<SignedIn>
    {
        public IdentityHandler(IHubService hubService) : base(hubService)
        {
        }

        public async Task HandleAsync(SignedIn @event, ICorrelationContext context)
        => await HubService.PublishIdentitySignedInAsync(@event,context.UserId);
    }*/
}
