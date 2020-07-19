using MicroS_Common.Dispatchers.Operations.Events;
using MicroS_Common.Services.Identity.Messages.Events;
using System;
using System.Threading.Tasks;

namespace MicroS_Common.Services.SignalR.Services
{
    public class HubService : IHubService
    {
        private readonly IHubWrapper _hubContextWrapper;

        public HubService(IHubWrapper hubContextWrapper)
        {
            _hubContextWrapper = hubContextWrapper;
        }



        public async Task PublishOperationPendingAsync(OperationPending @event)
            => await _hubContextWrapper.PublishToUserAsync(@event.UserId,
                OperationBase.OPERATION_PENDING,
                new
                {
                    id = @event.Id,
                    name = @event.Name,
                    resource = @event.Resource
                }
            );

        public async Task PublishOperationCompletedAsync(OperationCompleted @event)
            => await _hubContextWrapper.PublishToUserAsync(@event.UserId,
                OperationBase.OPERATION_COMPLETED,
                new
                {
                    id = @event.Id,
                    name = @event.Name,
                    resource = @event.Resource
                }
            );

        public async Task PublishOperationRejectedAsync(OperationRejected @event)
            => await _hubContextWrapper.PublishToUserAsync(@event.UserId,
                OperationBase.OPERATION_REJECTED,
                new
                {
                    id = @event.Id,
                    name = @event.Name,
                    resource = @event.Resource,
                    code = @event.Code,
                    reason = @event.Message
                }
            );

      /*  public async Task PublishIdentitySignedInAsync(SignedIn @event,Guid UserId)
        {
            await _hubContextWrapper.PublishToUserAsync(
                UserId,
                IdentityBase.SIGNED_IN,
                new
                {
                    id = @event.UserId
                });
        }*/
    }
}
