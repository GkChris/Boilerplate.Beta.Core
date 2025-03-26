namespace Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka.Abstractions
{
    public interface IKafkaConsumer
    {
        /// <summary>
        /// Subscribes to a Kafka topic with a specific message handler.
        /// </summary>
        /// <param name="topic">The topic to subscribe to.</param>
        /// <param name="messageHandler">The function to handle received messages.</param>
        void Subscribe(string topic, Func<string, Task> messageHandler);

        /// <summary>
        /// Unsubscribes from a Kafka topic.
        /// </summary>
        /// <param name="topic">The topic to unsubscribe from.</param>
        void Unsubscribe(string topic);

        /// <summary>
        /// Starts consuming messages asynchronously until cancellation is requested.
        /// </summary>
        /// <param name="cancellationToken">Token to signal when consumption should stop.</param>
        /// <returns>A task representing the message consumption loop.</returns>
        Task StartConsumingAsync(CancellationToken cancellationToken);
    }
}
