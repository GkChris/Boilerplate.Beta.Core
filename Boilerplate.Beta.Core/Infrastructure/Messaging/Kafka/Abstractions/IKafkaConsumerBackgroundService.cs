using System.Threading.Tasks;

namespace Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka.Abstractions
{
    public interface IKafkaConsumerBackgroundService
    {
        Task StartConsumingMessagesAsync();
    }
}