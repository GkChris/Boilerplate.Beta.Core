using Boilerplate.Beta.Core.Infrastructure.Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Boilerplate.Beta.Core.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class WebSocketTestController : ControllerBase
	{
		private readonly IHubContext<WebSocketHub> _hubContext;

		public WebSocketTestController(IHubContext<WebSocketHub> hubContext)
		{
			_hubContext = hubContext;
		}

		[HttpPost("send-message")]
		public async Task<IActionResult> SendMessageAsync([FromBody] string message)
		{
			await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
			return Ok(new { Status = "Message sent to WebSocket clients", Message = message });
		}

		[HttpGet("ping")]
		public IActionResult Ping()
		{
			return Ok(new { Status = "WebSocket is connected" });
		}
	}
}
