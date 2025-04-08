using Boilerplate.Beta.Core.Application.Services.Abstractions;
using Boilerplate.Beta.Core.Infrastructure.Messaging.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Beta.Core.Application.Services
{
	public class SignalRPublisherService : ISignalRPublisherService
	{
		private readonly IHubContext<ChatHub> _hubContext;
		private readonly ILogger<SignalRPublisherService> _logger;

		public SignalRPublisherService(IHubContext<ChatHub> hubContext, ILogger<SignalRPublisherService> logger)
		{
			_hubContext = hubContext;
			_logger = logger;
		}

		public async Task SendMessageToAllAsync(string clientId, string message)
		{
			_logger.LogInformation("Sending message to all SignalR clients: {Message}", message);
			await _hubContext.Clients.All.SendAsync("ReceiveMessage", clientId, message);
		}

		public async Task SendMessageToClientAsync(string clientId, string message)
		{
			_logger.LogInformation("Sending message to SignalR client {ClientId}: {Message}", clientId, message);
			await _hubContext.Clients.Client(clientId).SendAsync("ReceiveMessage", clientId, message);
		}
	}
}
