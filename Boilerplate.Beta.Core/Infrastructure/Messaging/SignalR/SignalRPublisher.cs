using Boilerplate.Beta.Core.Infrastructure.Messaging.SignalR.Abstractions;
using Microsoft.AspNetCore.SignalR;

namespace Boilerplate.Beta.Core.Infrastructure.Messaging.SignalR
{
	public class SignalRPublisher : ISignalRPublisher
	{
		private readonly IHubContext<SignalRHub> _hubContext;

		public SignalRPublisher(IHubContext<SignalRHub> hubContext)
		{
			_hubContext = hubContext;
		}

		public Task SendMessageToClient(string clientId, string message) =>
			_hubContext.Clients.Client(clientId).SendAsync("ReceiveMessage", message);

		public Task SendMessageToAllClients(string message) =>
			_hubContext.Clients.All.SendAsync("ReceiveMessage", message);
	}
}
