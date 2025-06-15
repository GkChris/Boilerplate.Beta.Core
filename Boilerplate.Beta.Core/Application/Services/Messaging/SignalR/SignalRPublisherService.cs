using Boilerplate.Beta.Core.Application.Services.Abstractions.Messaging.SignalR;
using Boilerplate.Beta.Core.Infrastructure.Messaging.SignalR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Beta.Core.Application.Services.Messaging.SignalR
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

        public async Task SendMessageToAllAsync(string message)
        {
            _logger.LogInformation("Sending message to all clients: {Message}", message);
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
        }

        public async Task SendMessageToClientAsync(string targetClientId, string message)
        {
            _logger.LogInformation("Sending message to client {TargetClientId}: {Message}", targetClientId, message);
            await _hubContext.Clients.Client(targetClientId).SendAsync("ReceiveMessage", message);
        }
    }
}