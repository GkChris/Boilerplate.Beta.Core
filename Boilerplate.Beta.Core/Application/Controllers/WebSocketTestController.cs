using Boilerplate.Beta.Core.Application.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Beta.Core.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class WebSocketTestController : ControllerBase
	{
		private readonly IWebSocketPublisherService _websocketPublisherService;

		public WebSocketTestController(IWebSocketPublisherService websocketPublisherService)
		{
			_websocketPublisherService = websocketPublisherService;
		}

		[HttpPost("send-message")]
		public async Task<IActionResult> SendMessageAsync([FromBody] string message)
		{
			await _websocketPublisherService.SendMessageToAllAsync(message);
			return Ok(new { Status = "Message sent to WebSocket clients", Message = message });
		}
	}
}
