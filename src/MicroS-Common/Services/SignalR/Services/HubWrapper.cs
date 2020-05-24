using MicroS_Common.Services.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace MicroS_Common.Services.SignalR.Services
{
    public class HubWrapper : IHubWrapper
    {
        private readonly IHubContext<MicroSHub> _hubContext;

        public HubWrapper(IHubContext<MicroSHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task PublishToUserAsync(Guid userId, string message, object data)
            => await _hubContext.Clients.Group(userId.ToUserGroup()).SendAsync(message, data);

        public async Task PublishToAllAsync(string message, object data)
            => await _hubContext.Clients.All.SendAsync(message, data);
    }
}
