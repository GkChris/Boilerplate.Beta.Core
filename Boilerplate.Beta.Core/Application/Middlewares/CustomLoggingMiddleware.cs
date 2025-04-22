using System.Diagnostics;
using Boilerplate.Beta.Core.Application.Mappers;
using Boilerplate.Beta.Core.Infrastructure;
using Microsoft.AspNetCore.Diagnostics;
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
			string statusLabel;
			bool isError;

			try
			{
				await _next(context);
				stopwatch.Stop();

				(statusLabel, isError) = EvaluateStatus(context);
				var message = ExtractMessage(context);

				LogRequest(context, request, stopwatch.ElapsedMilliseconds, statusLabel, context.Response.StatusCode, message);
			}
			catch (Exception ex)
			{
				stopwatch.Stop();

				var (statusCode, message, friendlyMessage, _) = ExceptionMapper.MapToHttpResponse(ex);
				context.Response.StatusCode = statusCode;

				(statusLabel, isError) = EvaluateStatus(context);

				LogException(context, request, stopwatch.ElapsedMilliseconds, statusLabel, context.Response.StatusCode, message, ex);
				throw;
			}
		}

		private (string StatusLabel, bool IsError) EvaluateStatus(HttpContext context) => context.Response.StatusCode switch
		{
			>= 200 and < 300 => ($"{AnsiColors.Green}[Success]{AnsiColors.Reset}", false),
			404 => ($"{AnsiColors.Yellow}[Warning]{AnsiColors.Reset}", false),
			>= 400 and < 500 => ($"{AnsiColors.Orange}[Client Error]{AnsiColors.Reset}", true),
			>= 500 => ($"{AnsiColors.Red}[Server Error]{AnsiColors.Reset}", true),
			_ => ($"{AnsiColors.Gray}[Unknown]{AnsiColors.Reset}", true)
		};

		private string ExtractMessage(HttpContext context)
		{
			var feature = context.Features.Get<IExceptionHandlerFeature>();
			if (feature?.Error != null)
			{
				return feature.Error.Message;
			}

			return context.Items.TryGetValue("Message", out var messageObj) ? messageObj?.ToString() : null;
		}

		private void LogRequest(HttpContext context, HttpRequest request, long elapsedMs, string status, int statusCode, string message)
		{
			var time = DateTime.UtcNow.ToString("R");
			var responseTime = $"{elapsedMs} ms";

			var logMessage = $"{status} | " +
							 $"{AnsiColors.BrightBlue}{statusCode}{AnsiColors.Reset} | " +
							 $"{AnsiColors.Cyan}{request.Method}{AnsiColors.Reset} | " +
							 $"{AnsiColors.Amber}{request.Path}{AnsiColors.Reset} | " +
							 $"{AnsiColors.Magenta}{responseTime}{AnsiColors.Reset} | " +
							 $"[{AnsiColors.Gray}{time}{AnsiColors.Reset}]";

			if (!string.IsNullOrWhiteSpace(message))
			{
				logMessage += $" | {AnsiColors.Yellow}{message}{AnsiColors.Reset}";
			}

			if (statusCode >= 400)
			{
				_logger.LogError(logMessage);
			}
			else
			{
				_logger.LogInformation(logMessage);
			}
		}

		private void LogException(HttpContext context, HttpRequest request, long elapsedMs, string status, int statusCode, string message, Exception ex)
		{
			var time = DateTime.UtcNow.ToString("R");
			var responseTime = $"{elapsedMs} ms";

			var logMessage = $"{status} | " +
							 $"{AnsiColors.BrightBlue}{statusCode}{AnsiColors.Reset} | " +
							 $"{AnsiColors.Cyan}{request.Method}{AnsiColors.Reset} | " +
							 $"{AnsiColors.Amber}{request.Path}{AnsiColors.Reset} | " +
							 $"{AnsiColors.Magenta}{responseTime}{AnsiColors.Reset} | " +
							 $"[{AnsiColors.Gray}{time}{AnsiColors.Reset}]";

			if (!string.IsNullOrWhiteSpace(message))
			{
				logMessage += $" | {AnsiColors.Yellow}{message}{AnsiColors.Reset}";
			}

			if (_settings.LogExceptionStackTrace)
			{
				_logger.LogError(ex, logMessage);
			}
			else
			{
				_logger.LogError(logMessage);
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
		public static string Blue = "\u001b[34m";
		public static string BrightBlue = "\u001b[94m";
		public static string Orange = "\u001b[38;5;214m"; 
		public static string Reset = "\u001b[0m";
	}
}
