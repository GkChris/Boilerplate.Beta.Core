using Boilerplate.Beta.Core.Application.Handlers;
using Boilerplate.Beta.Core.Application.Services;
using Boilerplate.Beta.Core.Application.Services.Abstractions;
using Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka;
using Boilerplate.Beta.Core.Infrastructure.Messaging.Kafka.Abstractions;
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

            services.AddSingleton<IKafkaProducer>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<KafkaProducer>>();
                return new KafkaProducer(bootstrapServers, logger);
            });

            services.AddSingleton<IKafkaConsumer, KafkaConsumer>();
            services.AddScoped<KafkaMessageHandlers>();
            services.AddScoped<IKafkaPublisherService, KafkaPublisherService>();
            services.AddHostedService<KafkaConsumerBackgroundService>();
        }
    }
}
