using DevTools.Kafka.Abstractions;
using DevTools.Kafka.Abstractions.Consumer;
using DevTools.Kafka.Abstractions.Producer;
using DevTools.Kafka.Options;
using DevTools.Kafka.Services.Consumer;
using DevTools.Kafka.Services.Producer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevTools.Kafka.DI
{
    /// <summary>
    /// Расширения для управления зависимостями в <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Добавляет JSON потребитель Kafka в переданную коллекцию сервисов
        /// </summary>
        /// <typeparam name="TKey">Тип ключа сообщения</typeparam>
        /// <typeparam name="TPayload">Тип тела сообщения</typeparam>
        /// <typeparam name="TOptions">Тип опций потребителя</typeparam>
        /// <typeparam name="TMessageHandler">Тип обработчика сообщения</typeparam>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <returns>Переданный <see cref="IServiceCollection"/></returns>
        /// <remarks>Обработчик сообщения будет зарегистрирован, как Scoped сервис</remarks>
        public static IServiceCollection AddKafkaJsonConsumer<TKey, TPayload, TOptions, TMessageHandler>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TPayload : class
            where TOptions : class, IKafkaConsumerOptions
            where TMessageHandler : class, IKafkaMessageHandler<TKey, TPayload>
        {
            services.Configure<TOptions>(configuration.GetSection(KafkaConsumerOptionsBase.Path));
            services.AddScoped<IKafkaMessageHandler<TKey, TPayload>, TMessageHandler>();
            services.AddSingleton<KafkaJsonConsumer<TKey, TPayload, TOptions>>();

            services.AddHostedService(sp =>
            {
                var consumer = sp.GetRequiredService<KafkaJsonConsumer<TKey, TPayload, TOptions>>();
                return new KafkaConsumerHostingService<TKey, TPayload, TOptions, TMessageHandler>(consumer);
            });

            return services;
        }

        /// <summary>
        /// Добавляет Protobuf потребитель Kafka в переданную коллекцию сервисов
        /// </summary>
        /// <typeparam name="TKey">Тип ключа сообщения</typeparam>
        /// <typeparam name="TPayload">Тип тела сообщения</typeparam>
        /// <typeparam name="TOptions">Тип опций потребителя</typeparam>
        /// <typeparam name="TMessageHandler">Тип обработчика сообщения</typeparam>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <param name="payloadAdapter">Адаптер массива байт в тип Protobuf</param>
        /// <returns>Переданный <see cref="IServiceCollection"/></returns>
        /// <remarks>Обработчик сообщения будет зарегистрирован, как Scoped сервис</remarks>
        public static IServiceCollection AddKafkaProtobufConsumer<TKey, TPayload, TOptions, TMessageHandler>(
            this IServiceCollection services,
            IConfiguration configuration,
            Func<byte[], TPayload> payloadAdapter)
            where TPayload : class
            where TOptions : class, IKafkaConsumerOptions
            where TMessageHandler : class, IKafkaMessageHandler<TKey, TPayload>
        {
            services.Configure<TOptions>(configuration.GetSection(KafkaConsumerOptionsBase.Path));
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

        /// <summary>
        /// Добавляет отправитель Kafka в переданную коллекцию сервисов
        /// </summary>
        /// <typeparam name="TKey">Тип ключа сообщения</typeparam>
        /// <typeparam name="TPayload">Тип тела сообщения</typeparam>
        /// <typeparam name="TOptions">Тип опций потребителя</typeparam>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <returns>Переданный <see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddKafkaProducer<TKey, TPayload, TOptions>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TPayload: class
            where TOptions : class, IKafkaProducerOptions
        {
            services.Configure<TOptions>(configuration.GetSection(KafkaProducerOptionsBase.Path));
            services.AddSingleton<KafkaProducer<TKey, TPayload, TOptions>>();

            return services;
        }
    }
}
