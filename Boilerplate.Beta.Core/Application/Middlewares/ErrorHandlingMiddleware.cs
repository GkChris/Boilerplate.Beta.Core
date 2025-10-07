using Boilerplate.Beta.Core.Application.Mappers;
using Boilerplate.Beta.Core.Application.Models.DTOs;
using Boilerplate.Beta.Core.Application.Shared.Constants;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Boilerplate.Beta.Core.Application.Middlewares
{
    public class ErrorHandlingMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ErrorHandlingMiddleware> _logger;
        private readonly IWebHostEnvironment _env;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IWebHostEnvironment env)
		{
			_next = next;
			_logger = logger;
			_env = env;
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

				var isDevelopment = _env.IsEnvironment(EnvInternalNames.LocalDevelopment) || _env.IsEnvironment(EnvInternalNames.DockerDevelopment);

                var response = new ApiErrorResponse
				{
					Error = new ApiErrorDetail
					{
						Type = typeName,
						Message = friendlyMessage,
						StackTrace = isDevelopment
                            ? ex.StackTrace?.ToString() 
							: null,
						InnerException = isDevelopment
							? ex.InnerException?.Message?.ToString()
							: null
					}
				};

                var options = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    WriteIndented = isDevelopment
                };

				var json = JsonSerializer.Serialize(response, options);

				await context.Response.WriteAsync(json);
            }
        }
	}
}