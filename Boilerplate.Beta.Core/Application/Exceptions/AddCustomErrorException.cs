using Microsoft.AspNetCore.Http;

namespace Boilerplate.Beta.Core.Application.Exceptions
{
	public class AddCustomErrorException : CustomException
	{
		public AddCustomErrorException(string addMessage)
			: base($"'{addMessage}'.", "This will be a future custom error.", StatusCodes.Status500InternalServerError)
		{
		}
	}
}