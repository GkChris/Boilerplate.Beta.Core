using Boilerplate.Beta.Core.Application.Services.Abstractions;
using Boilerplate.Beta.Core.Infrastructure.Messaging.SignalR.Abstractions;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Beta.Core.Application.Services
{
	public class SignalRPublisherService : ISignalRPublisherService
    {
        private readonly ISignalRPublisher _signalRPublisher;
        private readonly ILogger<SignalRPublisherService> _logger;

        public SignalRPublisherService(ISignalRPublisher signalRPublisher, ILogger<SignalRPublisherService> logger)
        {
            _signalRPublisher = signalRPublisher;
            _logger = logger;
        }

        public async Task SendMessageToAllAsync(string message)
        {
            _logger.LogInformation("Sending message to all clients: {Message}", message);
            await _signalRPublisher.SendMessageToAllClients(message);
        }

        public async Task SendMessageToClientAsync(string targetClientId, string message)
        {
            _logger.LogInformation("Sending message to client {TargetClientId}: {Message}", targetClientId, message);
            await _signalRPublisher.SendMessageToClient(targetClientId, message);
        }
    }
}