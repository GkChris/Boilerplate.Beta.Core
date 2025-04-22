using System.Security;
using Boilerplate.Beta.Core.Application.Exceptions;
using Microsoft.AspNetCore.Http;

namespace Boilerplate.Beta.Core.Application.Mappers
{
	public static class ExceptionMapper
	{
		public static (int StatusCode, string Message, string FriendlyMessage, string TypeName) MapToHttpResponse(Exception ex)
		{
			return ex switch
			{
				// CUSTOM
				CustomException custom => (
					custom.StatusCode,
					custom.Message ?? custom.FriendlyMessage,
					custom.FriendlyMessage ?? "A handled error occurred.",
					custom.GetType().Name
				),

				// CLIENT ERRORS
				ArgumentNullException e => (
					StatusCodes.Status400BadRequest,
					e.Message,
					"Missing required parameter.",
					nameof(ArgumentNullException)
				),
				ArgumentOutOfRangeException e => (
					StatusCodes.Status400BadRequest,
					e.Message,
					"Parameter is out of allowed range.",
					nameof(ArgumentOutOfRangeException)
				),
				ArgumentException e => (
					StatusCodes.Status400BadRequest,
					e.Message,
					"Invalid argument provided.",
					nameof(ArgumentException)
				),
				FormatException e => (
					StatusCodes.Status400BadRequest,
					e.Message,
					"Invalid input format.",
					nameof(FormatException)
				),
				InvalidOperationException e => (
					StatusCodes.Status400BadRequest,
					e.Message,
					"Invalid operation attempted.",
					nameof(InvalidOperationException)
				),
				KeyNotFoundException e => (
					StatusCodes.Status404NotFound,
					e.Message,
					"The specified key was not found.",
					nameof(KeyNotFoundException)
				),
				FileNotFoundException e => (
					StatusCodes.Status404NotFound,
					e.Message,
					"The requested file was not found.",
					nameof(FileNotFoundException)
				),
				DirectoryNotFoundException e => (
					StatusCodes.Status404NotFound,
					e.Message,
					"The requested directory was not found.",
					nameof(DirectoryNotFoundException)
				),
				OperationCanceledException e => (
					StatusCodes.Status408RequestTimeout,
					e.Message,
					"The operation was canceled or timed out.",
					nameof(OperationCanceledException)
				),
				TimeoutException e => (
					StatusCodes.Status408RequestTimeout,
					e.Message,
					"The operation timed out.",
					nameof(TimeoutException)
				),

				// AUTH & SECURITY
				UnauthorizedAccessException e => (
					StatusCodes.Status401Unauthorized,
					e.Message,
					"Unauthorized access.",
					nameof(UnauthorizedAccessException)
				),
				SecurityException e => (
					StatusCodes.Status403Forbidden,
					e.Message,
					"Access is forbidden.",
					nameof(SecurityException)
				),

				// NOT IMPLEMENTED / SERVER LIMITS
				NotImplementedException e => (
					StatusCodes.Status501NotImplemented,
					e.Message,
					"Feature not implemented.",
					nameof(NotImplementedException)
				),
				NotSupportedException e => (
					StatusCodes.Status405MethodNotAllowed,
					e.Message,
					"This operation is not supported.",
					nameof(NotSupportedException)
				),

				// SERVER ERRORS
				OutOfMemoryException e => (
					StatusCodes.Status500InternalServerError,
					e.Message,
					"The server ran out of memory.",
					nameof(OutOfMemoryException)
				),
				StackOverflowException e => (
					StatusCodes.Status500InternalServerError,
					e.Message,
					"A stack overflow occurred.",
					nameof(StackOverflowException)
				),
				SystemException e => (
					StatusCodes.Status500InternalServerError,
					e.Message,
					"A system error occurred.",
					nameof(SystemException)
				),

				// FALLBACK
				_ => (
					StatusCodes.Status500InternalServerError,
					ex.Message,
					"An unexpected error occurred.",
					ex.GetType().Name
				)
			};
		}
	}
}