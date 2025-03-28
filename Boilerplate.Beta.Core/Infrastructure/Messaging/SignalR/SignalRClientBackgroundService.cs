using Boilerplate.Beta.Core.Application.Handlers.Abstractions;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Beta.Core.Infrastructure.Messaging.SignalR
{
	public class SignalRClientBackgroundService : BackgroundService
	{
		private readonly ILogger<SignalRClientBackgroundService> _logger;
		private readonly ISignalRMessageHandler _messageHandler; // Inject handler
		private HubConnection _connection;

		public SignalRClientBackgroundService(
			ILogger<SignalRClientBackgroundService> logger,
			ISignalRMessageHandler messageHandler) // Inject handler
		{
			_logger = logger;
			_messageHandler = messageHandler;
			_connection = new HubConnectionBuilder()
				.WithUrl("http://localhost:5000/chatHub") // Ensure the correct URL
				.WithAutomaticReconnect()
				.Build();
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_connection.On<string, string>("ReceiveMessage", async (clientId, message) =>
			{
				_logger.LogInformation($"📩 Backend received a message from {clientId}: {message}");
				await _messageHandler.HandleMessageAsync(clientId, message); // Call handler
			});

			while (!stoppingToken.IsCancellationRequested)
			{
				try
				{
					if (_connection.State == HubConnectionState.Disconnected)
					{
						_logger.LogInformation("Connecting to SignalR...");
						await _connection.StartAsync(stoppingToken);
						_logger.LogInformation("✅ Connected to SignalR!");
					}
				}
				catch (Exception ex)
				{
					_logger.LogError($"❌ Connection failed: {ex.Message}");
				}

				await Task.Delay(5000, stoppingToken); // Retry every 5 seconds
			}
		}

		public override async Task StopAsync(CancellationToken stoppingToken)
		{
			await _connection.StopAsync();
			await _connection.DisposeAsync();
			_logger.LogInformation("❌ SignalR client disconnected.");
		}
	}
}
