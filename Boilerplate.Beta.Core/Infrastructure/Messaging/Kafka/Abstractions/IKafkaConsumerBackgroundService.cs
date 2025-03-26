namespace Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka.Abstractions
{
    public interface IKafkaConsumerBackgroundService
    {
        /// <summary>
        /// Starts consuming messages in the background.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>
        Task StartConsumingMessagesAsync();
    }
}
