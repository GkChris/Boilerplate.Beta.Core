using Boilerplate.Beta.Core.Application.Messaging.Kafka;
using Boilerplate.Beta.Core.Infrastructure.Messaging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Boilerplate.Beta.Core.Infrastructure.Extensions
{
    public static class KafkaExtensions
    {
        public static void AddKafka(this IServiceCollection services, IConfiguration configuration)
        {
            var bootstrapServers = configuration["Kafka:BootstrapServers"];
            var consumerGroup = configuration["Kafka:ConsumerGroup"];
            var topic = configuration["Kafka:Topic"];

            services.AddSingleton<IKafkaProducer>(new KafkaProducer(bootstrapServers));
            services.AddSingleton<IKafkaConsumer>(new KafkaConsumer(bootstrapServers, topic, consumerGroup));

            services.AddHostedService<KafkaConsumerBackgroundService>();
        }

		public static void UseKafka(this IApplicationBuilder app)
		{
			
		}
	}
}
