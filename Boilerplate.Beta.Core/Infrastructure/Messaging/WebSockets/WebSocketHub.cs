using Microsoft.AspNetCore.SignalR;

namespace Boilerplate.Beta.Core.Infrastructure.Messaging.WebSockets
{
    public class WebSocketHub : Hub
    {
        public async Task SendMessageToAll(string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}