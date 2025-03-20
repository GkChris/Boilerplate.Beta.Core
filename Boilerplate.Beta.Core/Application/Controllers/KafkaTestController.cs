using Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Beta.Core.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class KafkaTestController : ControllerBase
	{
		private readonly IKafkaProducer _kafkaProducer;

		public KafkaTestController(IKafkaProducer kafkaProducer)
		{
			_kafkaProducer = kafkaProducer;
		}

		[HttpPost("send-message")]
		public async Task<IActionResult> SendMessageAsync([FromBody] string message)
		{
			await _kafkaProducer.PublishMessageAsync("topic", message);
			return Ok(new { Status = "Message sent to Kafka", Message = message });
		}
	}
}