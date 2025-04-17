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
			if (!_settings.EnableCustomLoggingMiddleware)
			{
				await _next(context);
				return;
			}

			var stopwatch = Stopwatch.StartNew();
			var request = context.Request;
			string status;
			bool logError = false;
			string errorDetails = null;

			try
			{
				await _next(context);
				stopwatch.Stop();

				(status, logError) = EvaluateStatus(context);
			}
			catch (Exception ex)
			{
				stopwatch.Stop();

				status = $"{AnsiColors.Red}[Failure]{AnsiColors.Reset}";
				errorDetails = ex.Message;

				context.Response.StatusCode = 500;

				LogException(context, request, stopwatch.ElapsedMilliseconds, status, errorDetails);
				throw;
			}

			LogRequest(context, request, stopwatch.ElapsedMilliseconds, status, logError);
		}

		private (string Status, bool LogError) EvaluateStatus(HttpContext context)
		{
			return context.Response.StatusCode switch
			{
				200 => ($"{AnsiColors.Green}[Success]{AnsiColors.Reset}", false),
				404 => ($"{AnsiColors.Yellow}[Warning]{AnsiColors.Reset}", false),
				_ => ($"{AnsiColors.Red}[Error]{AnsiColors.Reset}", true)
			};
		}

		private void LogRequest(HttpContext context, HttpRequest request, long elapsedMs, string status, bool logError)
		{
			var time = DateTime.UtcNow.ToString("R");
			var responseTime = $"{elapsedMs} ms";

			var logMessage = $"{status} | " +
							 $"{AnsiColors.Cyan}{request.Method}{AnsiColors.Reset} | " +
							 $"{AnsiColors.Amber}{request.Path}{AnsiColors.Reset} | " +
							 $"{AnsiColors.Magenta}{responseTime}{AnsiColors.Reset} | " +
							 $"[{AnsiColors.Gray}{time}{AnsiColors.Reset}]";

			if (logError)
			{
				logMessage += $" | {AnsiColors.Red}StatusCode: {context.Response.StatusCode}{AnsiColors.Reset}";
			}

			_logger.LogInformation(logMessage);
		}

		private void LogException(HttpContext context, HttpRequest request, long elapsedMs, string status, string errorDetails)
		{
			var time = DateTime.UtcNow.ToString("R");
			var responseTime = $"{elapsedMs} ms";

			var logMessage = $"{status} | " +
							 $"{AnsiColors.Cyan}{request.Method}{AnsiColors.Reset} | " +
							 $"{AnsiColors.Amber}{request.Path}{AnsiColors.Reset} | " +
							 $"{AnsiColors.Magenta}{responseTime}{AnsiColors.Reset} | " +
							 $"[{AnsiColors.Gray}{time}{AnsiColors.Reset}] | " +
							 $"{AnsiColors.Red}Exception: {errorDetails}{AnsiColors.Reset}";

			_logger.LogError(logMessage);
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
		public static string Reset = "\u001b[0m";
	}
}