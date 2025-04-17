using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Beta.Core.Application.Middlewares
{
	public class ErrorHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ErrorHandlingMiddleware> _logger;

		public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				_logger.LogDebug("Exception caught in ErrorHandlingMiddleware: {Message}", ex.Message);
				throw;
			}
		}
	}
}