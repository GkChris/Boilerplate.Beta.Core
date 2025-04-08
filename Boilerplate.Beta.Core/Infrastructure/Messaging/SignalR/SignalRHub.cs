using Boilerplate.Beta.Core.Infrastructure.Messaging.SignalR.Abstractions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Beta.Core.Infrastructure.Messaging.SignalR
{
	public class ChatHub : Hub, IChatHub
	{
		private readonly ILogger<ChatHub> _logger;

		public ChatHub(ILogger<ChatHub> logger)
		{
			_logger = logger;
		}

		public async Task SendMessageToClient(string clientId, string message)
		{
			_logger.LogInformation("Sending message to {ClientId}: {Message}", clientId, message);
			await Clients.Client(clientId).SendAsync("ReceiveMessage", clientId, message);
		}

		public async Task SendMessageToAllClients(string message)
		{
			_logger.LogInformation("Sending message to all clients: {Message}", message);
			await Clients.All.SendAsync("ReceiveMessage", message);
		}

		public override async Task OnConnectedAsync()
		{
			_logger.LogInformation("Client connected: {ClientId}", Context.ConnectionId);
			await base.OnConnectedAsync();
		}

		public override async Task OnDisconnectedAsync(Exception? exception)
		{
			_logger.LogInformation("Client disconnected: {ClientId}", Context.ConnectionId);
			await base.OnDisconnectedAsync(exception);
		}
	}
}