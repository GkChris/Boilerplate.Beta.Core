using Boilerplate.Beta.Core.Application.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Beta.Core.Application.Controllers.TestControllers
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

        [HttpPost("send-message-to-client")]
        public async Task<IActionResult> SendMessageToClientAsync([FromBody] SignalRTestRequest request)
        {
            if (CanHandleMessage(request.Message))
            {
                await _signalRPublisherService.SendMessageToClientAsync(request.ClientId, request.Message);
            }
            return Ok();
        }

        [HttpPost("send-message-to-all")]
        public async Task<IActionResult> SendMessageToAllAsync([FromBody] string message)
        {
            if (CanHandleMessage(message))
            {
                await _signalRPublisherService.SendMessageToAllAsync(message);
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
