using Boilerplate.Beta.Core.Application.Messaging.Kafka;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Beta.Core.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class KafkaTestController : ControllerBase
	{
		private readonly IKafkaProducer _kafkaProducer;
		private readonly IKafkaConsumer _kafkaConsumer;

		public KafkaTestController(IKafkaProducer kafkaProducer, IKafkaConsumer kafkaConsumer)
		{
			_kafkaProducer = kafkaProducer;
			_kafkaConsumer = kafkaConsumer;
		}

		[HttpPost("send-message")]
		public async Task<IActionResult> SendMessageAsync([FromBody] string message)
		{
			await _kafkaProducer.PublishMessageAsync("topic", message);
			return Ok(new { Status = "Message sent to Kafka", Message = message });
		}
	}
}