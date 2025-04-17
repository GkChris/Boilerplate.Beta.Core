using System.Diagnostics;
using Boilerplate.Beta.Core.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Boilerplate.Beta.Core.Application.Middlewares
{
	public class CustomLoggingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<CustomLoggingMiddleware> _logger;
		private readonly InfrastructureSettings _settings;

		public CustomLoggingMiddleware(
			RequestDelegate next,
			ILogger<CustomLoggingMiddleware> logger,
			IOptions<InfrastructureSettings> options)
		{
			_next = next;
			_logger = logger;
			_settings = options.Value;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			// Initialize logging variables
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			var request = context.Request;
			var response = context.Response;
			var statusCode = response.StatusCode;
			var responseMessage = response.Headers["Message"];
			var errorCode = response.Headers["ErrorCode"];
			var errorMessage = response.Headers["ErrorMessage"];
			var errorDetails = response.Headers["ErrorDetails"];

			var logError = false;
			string status;

			try
			{
				if (_settings.EnableCustomLoggingMiddleware)
				{
					_logger.LogInformation($"Handling request: {request.Method} {request.Path}");

					await _next(context);

					stopwatch.Stop();
					var responseTime = $"{stopwatch.ElapsedMilliseconds} ms";
					var time = DateTime.UtcNow.ToString("R");

					if (statusCode == 200)
					{
						status = $"{AnsiColors.Green}[Success]{AnsiColors.Reset}";
					}
					else if (statusCode == 404)
					{
						status = $"{AnsiColors.Yellow}[Warning]{AnsiColors.Reset}";
					}
					else
					{
						status = $"{AnsiColors.Red}[Error]{AnsiColors.Reset}";
						logError = true;
					}

					var logMessage = $"{status} | " +
									 $"{AnsiColors.Cyan}{request.Method}{AnsiColors.Reset} | " +
									 $"{AnsiColors.Amber}{request.Path}{AnsiColors.Reset} | " +
									 $"{AnsiColors.Magenta}{responseTime}{AnsiColors.Reset} | " +
									 $"[{AnsiColors.Gray}{time}{AnsiColors.Reset}]";


					if (logError)
					{
						logMessage += $" | {AnsiColors.Red}ErrorCode: {errorCode} | ErrorDetails: {errorDetails}{AnsiColors.Reset}";
					}

					if (!string.IsNullOrEmpty(responseMessage))
					{
						logMessage += $" | {AnsiColors.Yellow}{responseMessage}{AnsiColors.Reset}";
					}

					_logger.LogInformation(logMessage);
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An error occurred while processing the request");
				throw; 
			}
		}
	}

	public static class AnsiColors
	{
		public static string Red = "\u001b[31m";
		public static string Green = "\u001b[32m";
		public static string Yellow = "\u001b[33m";
		public static string Amber = "\u001b[38;5;214m";
		public static string Cyan = "\u001b[36m";
		public static string Magenta = "\u001b[35m";
		public static string Gray = "\u001b[90m";
		public static string Reset = "\u001b[0m";  // Reset to default color
	}
}
