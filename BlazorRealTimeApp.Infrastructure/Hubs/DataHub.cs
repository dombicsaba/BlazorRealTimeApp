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
        private static ConcurrentDictionary<string, string> ConnectedClients = new();


        public DataHub(IDbContextFactory<ApplicationDbContext> contextFactory, ILogger<DataHub> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var clientId = httpContext?.Request.Headers["ClientIdentifier"].ToString();

            if (!string.IsNullOrEmpty(clientId))
            {
                lock (ConnectedClients)
                {
                    ConnectedClients[clientId] = Context.ConnectionId;
                    _logger.LogInformation("DataHub.OnConnectedAsync() | Client connected: {ClientId}", clientId);
                }
            }

            foreach (var client in ConnectedClients) 
            {
                _logger.LogInformation("DataHub.OnConnectedAsync() | Connected client: {Client.Key}", client.Key);
            }

                await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var clientToRemove = ConnectedClients.FirstOrDefault(x => x.Value == Context.ConnectionId).Key;
            if (!string.IsNullOrEmpty(clientToRemove))
            {
                lock (ConnectedClients)                
                {
                    ConnectedClients.TryRemove(clientToRemove, out _);
                    _logger.LogInformation("DataHub.OnDisconnectedAsync() | Client disconnected: {clientToRemove}, Exception: {Exception}", clientToRemove, exception);
                }
            }

            if (ConnectedClients.Count == 0)
                _logger.LogInformation("DataHub.OnDisconnectedAsync() | Remaining client: No clients connected.");

            foreach (var client in ConnectedClients) 
            {
                _logger.LogInformation("DataHub.OnDisconnectedAsync() | Remaining client: {Client.Key}", client.Key);
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendUpdate(string message)
        {
            _logger.LogInformation("DataHub.SendUpdate() | Sending update to all clients: {Message}", message);

            //using var context = _contextFactory.CreateDbContext();
            //var articles = await context.Articles.ToListAsync();

            foreach (var client in ConnectedClients)
            {
                _logger.LogInformation("DataHub.SendUpdate() | Notifying client: {Client.Key}", client.Key);
            }

            await Clients.All.SendAsync("ReceiveUpdate", message);
        }
    }
}
