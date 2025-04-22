namespace Boilerplate.Beta.Core.Application.Exceptions
{
	/// <summary>
	/// Base class for all custom application exceptions.
	/// Carries an HTTP status code and a friendly message.
	/// </summary>
	public abstract class CustomException : Exception
	{
		public int StatusCode { get; }
		public string FriendlyMessage { get; }

		protected CustomException(string message, string friendlyMessage, int statusCode)
			: base(message)
		{
			FriendlyMessage = friendlyMessage;
			StatusCode = statusCode;
		}

		protected CustomException(string friendlyMessage, int statusCode)
			: base(friendlyMessage)
		{
			FriendlyMessage = friendlyMessage;
			StatusCode = statusCode;
		}
	}
}