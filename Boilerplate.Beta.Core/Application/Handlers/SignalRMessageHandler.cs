using Boilerplate.Beta.Core.Application.Handlers.Abstractions;
using Boilerplate.Beta.Core.Infrastructure.Messaging.WebSockets;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Beta.Core.Application.Handlers
{
	public class SignalRMessageHandler : ISignalRMessageHandler
	{
		private readonly ILogger<SignalRMessageHandler> _logger;
		private readonly IHubContext<ChatHub> _hubContext;

		public SignalRMessageHandler(ILogger<SignalRMessageHandler> logger, IHubContext<ChatHub> hubContext)
		{
			_logger = logger;
			_hubContext = hubContext;
		}

		public async Task HandleMessageAsync(string clientId, string message)
		{
			_logger.LogInformation("Handling chat message from {ClientId}: {Message}", clientId, message);

			if (CanHandleMessage(message))
			{
				await SendMessageToClient(clientId, message);
			}
			else
			{
				_logger.LogWarning("No handler found for message: {Message}", message);
			}
		}

		public bool CanHandleMessage(string message)
		{
			return message.StartsWith("chat:", StringComparison.OrdinalIgnoreCase);
		}

		private async Task SendMessageToClient(string clientId, string message)
		{
			try
			{
				await _hubContext.Clients.Client(clientId).SendAsync("ReceiveMessage", message);
				_logger.LogInformation("Sent message to {ClientId}: {Message}", clientId, message);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error sending message to client {ClientId}: {Message}", clientId, message);
			}
		}
	}
}
