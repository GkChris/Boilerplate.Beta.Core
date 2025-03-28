using Boilerplate.Beta.Core.Application.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Beta.Core.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class KafkaTestController : ControllerBase
	{
		private readonly IKafkaPublisherService _kafkaPublisherService;

		public KafkaTestController(IKafkaPublisherService kafkaPublisherService)
		{
			_kafkaPublisherService = kafkaPublisherService;
		}

		[HttpPost("send-message")]
		public async Task<IActionResult> SendMessageAsync([FromBody] string message)
		{
			await _kafkaPublisherService.PublishKafkaMessage("example-topic-1", message);
			return Ok(new { Status = "Message sent to Kafka", Message = message });
		}
	}
}