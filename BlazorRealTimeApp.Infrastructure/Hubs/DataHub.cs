using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorRealTimeApp.Infrastructure.Hubs
{
    public class DataHub : Hub
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly ILogger<DataHub> _logger;
        private static readonly ConcurrentDictionary<string, string> ConnectedClients = new();


        public DataHub(IDbContextFactory<ApplicationDbContext> contextFactory, ILogger<DataHub> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            ConnectedClients[Context.ConnectionId] = Context.ConnectionId;
            _logger.LogInformation("Client connected: {ConnectionId}", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            ConnectedClients.TryRemove(Context.ConnectionId, out _);
            _logger.LogInformation("Client disconnected: {ConnectionId}", Context.ConnectionId);
            if (exception != null)
            {
                _logger.LogError(exception, "Client disconnected with error");
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendUpdate(string message)
        {
            _logger.LogInformation("Sending update to all clients: {Message}", message);

            using var context = _contextFactory.CreateDbContext();
            var articles = await context.Articles.ToListAsync();

            foreach (var clientId in ConnectedClients.Keys)
            {
                _logger.LogInformation("Notifying client: {ClientId}", clientId);
            }

            await Clients.All.SendAsync("ReceiveUpdate", message);
        }


        //public async Task SendUpdate(string message)
        //{
        //    _logger.LogInformation("Sending update to {ClientCount} clients: {Message}", ConnectedClients.Count, message);

        //    foreach (var clientId in ConnectedClients.Keys)
        //    {
        //        _logger.LogInformation("Notifying client: {ClientId}", clientId);
        //    }

        //    await Clients.All.SendAsync("ReceiveUpdate", message);
        //}

        //public async Task SendUpdate(string message)
        //{
        //    _logger.LogInformation("Sending message: {Message}", message);
        //    await Clients.All.SendAsync("ReceiveUpdate", message);
        //}
    }
}
