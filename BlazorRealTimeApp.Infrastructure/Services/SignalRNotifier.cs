using BlazorRealTimeApp.Application.Common.Interfaces;
using BlazorRealTimeApp.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRealTimeApp.Infrastructure.Services
{
    public class SignalRNotifier : IRealTimeNotifier
    {
        private readonly IHubContext<DataHub> _hubContext;

        public SignalRNotifier(IHubContext<DataHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task NotifyArticlesUpdated(string message)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveUpdate", message);
        }
    }
}
