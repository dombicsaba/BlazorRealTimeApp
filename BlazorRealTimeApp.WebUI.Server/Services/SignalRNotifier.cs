using BlazorRealTimeApp.Application.Common.Interfaces;
using BlazorRealTimeApp.WebUI.Server.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace BlazorRealTimeApp.WebUI.Server.Services
{
    public class SignalRNotifier : IRealTimeNotifier
    {
        private readonly IHubContext<DataHub> _hubContext;

        public SignalRNotifier(IHubContext<DataHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyArticlesUpdated()
        {
            await _hubContext.Clients.All.SendAsync("ReceiveUpdate", "Articles table updated!");
        }
    }
}
