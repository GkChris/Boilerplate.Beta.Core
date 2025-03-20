using System.Net.WebSockets;
using System.Threading.Tasks;

namespace Boilerplate.Beta.Core.Infrastructure.Messaging.WebSockets.Abstractions
{
    public interface IWebSocketHub
    {
        Task HandleClientMessages(string clientId, WebSocket socket);
    }
}