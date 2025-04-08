using Boilerplate.Beta.Core.Application.Handlers.Abstractions;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Beta.Core.Application.Handlers
{
	public class SignalRMessageHandler : ISignalRMessageHandler
	{
		private readonly ILogger<SignalRMessageHandler> _logger;

		public SignalRMessageHandler(ILogger<SignalRMessageHandler> logger)
		{
			_logger = logger;
		}

		public async Task HandleMessageAsync(string clientId, string message)
		{
			_logger.LogInformation("Handling chat message from {ClientId}: {Message}", clientId, message);

			if (CanHandleMessage(message))
			{
				
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
	}
}
