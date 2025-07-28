using DevTools.Kafka.Abstractions;
using DevTools.Kafka.Options;
using DevTools.Kafka.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTools.Kafka.DI
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddKafkaJsonConsumer<TKey, TPayload, TOptions, TMessageHandler>(
            this IServiceCollection services,
            IConfiguration configuration,
            string configurationKey)
            where TPayload : class
            where TOptions : class, IKafkaConsumerOptions
            where TMessageHandler : class, IKafkaMessageHandler<TKey, TPayload>
        {
            services.Configure<TOptions>(configuration.GetSection(configurationKey));
            services.AddScoped<IKafkaMessageHandler<TKey, TPayload>, TMessageHandler>();
            services.AddSingleton<KafkaJsonConsumer<TKey, TPayload, TOptions>>();

            services.AddHostedService(sp =>
            {
                var consumer = sp.GetRequiredService<KafkaJsonConsumer<TKey, TPayload, TOptions>>();
                return new KafkaConsumerHostingService<TKey, TPayload, TOptions, TMessageHandler>(consumer);
            });

            return services;
        }

        public static IServiceCollection AddKafkaProtobufConsumer<TKey, TPayload, TOptions, TMessageHandler>(
            this IServiceCollection services,
            IConfiguration configuration,
            string configurationKey,
            Func<byte[], TPayload> payloadAdapter)
            where TPayload : class
            where TOptions : class, IKafkaConsumerOptions
            where TMessageHandler : class, IKafkaMessageHandler<TKey, TPayload>
        {
            services.Configure<TOptions>(configuration.GetSection(configurationKey));
            services.AddSingleton(payloadAdapter);
            services.AddScoped<IKafkaMessageHandler<TKey, TPayload>, TMessageHandler>();
            services.AddSingleton<KafkaProtobufConsumer<TKey, TPayload, TOptions>>();

            services.AddHostedService(sp =>
            {
                var consumer = sp.GetRequiredService<KafkaProtobufConsumer<TKey, TPayload, TOptions>>();
                return new KafkaConsumerHostingService<TKey, TPayload, TOptions, TMessageHandler>(consumer);
            });

            return services;
        }
    }
}
