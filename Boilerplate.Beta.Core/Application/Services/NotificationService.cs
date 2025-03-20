using Boilerplate.Beta.Core.Infrastructure.Messaging.WebSockets.Abstractions;

namespace Boilerplate.Beta.Core.Application.Services
{
    public class NotificationService
	{
		private readonly IWebSocketManager _webSocketManager;

		public NotificationService(IWebSocketManager webSocketManager)
		{
			_webSocketManager = webSocketManager;
		}

		public async Task NotifyClients(string message)
		{
			await _webSocketManager.SendMessageToAllClients(message);
		}
	}

}
