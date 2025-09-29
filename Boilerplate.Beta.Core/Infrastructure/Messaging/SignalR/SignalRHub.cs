using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Beta.Core.Infrastructure.Messaging.SignalR
{
	public class SignalRHub : Hub
	{
		private readonly ILogger<SignalRHub> _logger;

		public SignalRHub(ILogger<SignalRHub> logger)
		{
			_logger = logger;
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