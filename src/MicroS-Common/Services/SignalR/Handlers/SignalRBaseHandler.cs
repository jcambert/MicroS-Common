using MicroS_Common.Services.SignalR.Services;

namespace MicroS_Common.Services.SignalR.Handlers
{
    public abstract class SignalRBaseHandler
    {

        public SignalRBaseHandler(IHubService hubService)
       => (HubService) = (hubService);
        protected IHubService HubService { get; }
    }
}
