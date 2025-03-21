using Boilerplate.Beta.Core.Application.Handlers;
using Boilerplate.Beta.Core.Application.Services;
using Boilerplate.Beta.Core.Application.Services.Abstractions;
using Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka;
using Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Beta.Core.Infrastructure.Extensions
{
    public static class KafkaExtension
    {
        public static void AddKafka(this IServiceCollection services, IConfiguration configuration)
        {
            var bootstrapServers = configuration["Kafka:BootstrapServers"];
            var consumerGroup = configuration["Kafka:ConsumerGroup"];
            var topics = configuration.GetSection("Kafka:Topics").Get<string[]>();

            services.AddSingleton<IKafkaProducer>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<KafkaProducer>>();
                return new KafkaProducer(bootstrapServers, logger);
            });

            services.AddSingleton<IKafkaConsumer, KafkaConsumer>();
			services.AddSingleton<KafkaMessageHandlers>();
            services.AddScoped<IKafkaPublisherService, KafkaPublisherService>();
			services.AddHostedService<KafkaConsumerBackgroundService>();
        }

		public static void UseKafka(this IApplicationBuilder app)
		{
			
		}
	}
}
