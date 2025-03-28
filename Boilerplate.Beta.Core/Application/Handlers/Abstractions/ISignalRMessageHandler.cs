namespace Boilerplate.Beta.Core.Application.Handlers.Abstractions
{
	public interface ISignalRMessageHandler
	{
		/// <summary>
		/// Handles the message sent by a client.
		/// </summary>
		/// <param name="clientId">The client identifier to which the message is directed.</param>
		/// <param name="message">The message to be processed and sent.</param>
		Task HandleMessageAsync(string clientId, string message);

		/// <summary>
		/// Checks if the handler can process the given message.
		/// </summary>
		/// <param name="message">The message to be checked.</param>
		/// <returns>True if the handler can process the message, otherwise false.</returns>
		bool CanHandleMessage(string message);
	}
}
