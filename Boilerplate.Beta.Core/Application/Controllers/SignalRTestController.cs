using Boilerplate.Beta.Core.Application.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Beta.Core.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class SignalRTestController : ControllerBase
	{
		private readonly ISignalRPublisherService _signalRPublisherService;

		public SignalRTestController(ISignalRPublisherService signalRPublisherService)
		{
			_signalRPublisherService = signalRPublisherService;
		}

		[HttpPost("send-message")]
		public async Task<IActionResult> SendMessageAsync([FromBody] SignalRTestRequest request)
		{
			if (CanHandleMessage(request.Message))
			{
				await _signalRPublisherService.SendMessageToClientAsync(request.ClientId, request.Message);
			}
			return Ok();
		}

		private bool CanHandleMessage(string message)
		{
			return message.StartsWith("chat:", StringComparison.OrdinalIgnoreCase);
		}
	}

	public class SignalRTestRequest
	{
		public string ClientId { get; set; } = default!;
		public string Message { get; set; } = default!;
	}
}
