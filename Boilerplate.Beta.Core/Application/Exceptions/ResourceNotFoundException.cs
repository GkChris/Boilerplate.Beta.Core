using Microsoft.AspNetCore.Http;

namespace Boilerplate.Beta.Core.Application.Exceptions
{
	public class ResourceNotFoundException : CustomException
	{
		public ResourceNotFoundException(string resource)
			: base($"Resource '{resource}' was not found.", "The requested resource was not found.", StatusCodes.Status404NotFound)
		{
		}
	}
}