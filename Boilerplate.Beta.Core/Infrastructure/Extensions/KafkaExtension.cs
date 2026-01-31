using Boilerplate.Beta.Core.Application.Handlers;
using Boilerplate.Beta.Core.Application.Handlers.Abstractions;
using Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka;
using Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Boilerplate.Beta.Core.Infrastructure.Extensions
{
    public static class KafkaExtension
    {
        public static void AddKafkaBus(this IServiceCollection services, IConfiguration configuration)
        {
			services.AddKafkaPublisher(configuration);
			services.AddKafkaConsumer(configuration);
		}

		public static void AddKafkaPublisher(this IServiceCollection services, IConfiguration configuration)
		{
			var bootstrapServers = configuration["Kafka:BootstrapServers"];

			services.AddSingleton<IKafkaProducer>(provider =>
			{
				var logger = provider.GetRequiredService<ILogger<KafkaProducer>>();
				return new KafkaProducer(bootstrapServers, logger);
			});

			services.AddScoped<IKafkaMessagePublisher, KafkaMessagePublisher>();
		}

		public static void AddKafkaConsumer(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton<IKafkaConsumer, KafkaConsumer>();
			services.AddScoped<KafkaMessageConsumer>();
			services.AddHostedService<KafkaConsumerBackgroundService>();
		}
	}
}
