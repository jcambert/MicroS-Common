using Elasticsearch.Net.Specification.TasksApi;
using MicroS_Common.Dispatchers.Operations.Events;
using MicroS_Common.Services.Identity.Messages.Events;
using System;
using System.Threading.Tasks;

namespace MicroS_Common.Services.SignalR.Services
{
    public interface IHubService
    {
        Task PublishOperationPendingAsync(OperationPending @event);
        Task PublishOperationCompletedAsync(OperationCompleted @event);
        Task PublishOperationRejectedAsync(OperationRejected @event);

        //Task PublishIdentitySignedInAsync(SignedIn @event,Guid UserId);
    }
}
