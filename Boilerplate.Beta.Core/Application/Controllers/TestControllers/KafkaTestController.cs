using Boilerplate.Beta.Core.Application.Handlers.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Boilerplate.Beta.Core.Application.Controllers.TestControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KafkaTestController : ControllerBase
    {
        private readonly IKafkaMessagePublisher _kafkaPublisherService;

        public KafkaTestController(IKafkaMessagePublisher kafkaPublisherService)
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