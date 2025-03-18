using Microsoft.AspNetCore.SignalR;

namespace BlazorRealTimeApp.WebUI.Server.Hubs
{
    public class DataHub : Hub
    {
        public async Task SendUpdate(string message)
        {
            await Clients.All.SendAsync("ReceiveUpdate", message);
        }
    }
}
