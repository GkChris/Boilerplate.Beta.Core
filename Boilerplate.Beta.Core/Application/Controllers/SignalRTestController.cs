using Boilerplate.Beta.Core.Application.Services.Abstractions;
using Boilerplate.Beta.Core.Infrastructure.Messaging.WebSockets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Boilerplate.Beta.Core.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class SignalRTestController : ControllerBase
	{
		private readonly IHubContext<ChatHub> _hubContext;

		public SignalRTestController(IHubContext<ChatHub> hubContext)
		{
			_hubContext = hubContext;
		}

		[HttpPost("send-message")]
		public async Task<IActionResult> SendMessageAsync([FromBody] MessageRequest request)
		{
			await _hubContext.Clients.All.SendAsync("ReceiveMessage", request.Message);
			return Ok(new { Status = "Message sent", Message = request.Message });
		}
	}

	public class MessageRequest
	{
		public string Message { get; set; }
	}
}
