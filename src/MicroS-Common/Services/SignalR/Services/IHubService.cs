using MicroS_Common.Dispatchers.Operations.Events;
using System.Threading.Tasks;

namespace MicroS_Common.Services.SignalR.Services
{
    public interface IHubService
    {
        Task PublishOperationPendingAsync(OperationPending @event);
        Task PublishOperationCompletedAsync(OperationCompleted @event);
        Task PublishOperationRejectedAsync(OperationRejected @event);
    }
}
