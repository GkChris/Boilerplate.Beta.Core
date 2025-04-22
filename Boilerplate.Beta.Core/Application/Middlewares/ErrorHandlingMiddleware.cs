using System.Text.Json;
using Boilerplate.Beta.Core.Application.Mappers;
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
				var (statusCode, message, friendlyMessage, typeName) = ExceptionMapper.MapToHttpResponse(ex);

				context.Response.StatusCode = statusCode;
				context.Response.ContentType = "application/json";

				var response = new
				{
					error = new
					{
						type = typeName,
						message = friendlyMessage
					}
				};

				await context.Response.WriteAsync(JsonSerializer.Serialize(response));
			}
		}
	}
}