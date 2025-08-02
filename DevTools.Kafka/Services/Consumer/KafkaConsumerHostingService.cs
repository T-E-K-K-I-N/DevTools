using DevTools.Kafka.Abstractions;
using DevTools.Kafka.Abstractions.Consumer;
using DevTools.Kafka.Options;
using Microsoft.Extensions.Hosting;

namespace DevTools.Kafka.Services
{
    /// <summary>
    /// Фоновый сервис для хостинга потребителя Kafka
    /// </summary>
    /// <typeparam name="TKey">Тип ключа сообщения</typeparam>
    /// <typeparam name="TPayload">Тип тела сообщения</typeparam>
    /// <typeparam name="TOptions">Тип опций потребителя</typeparam>
    /// <typeparam name="TMessageHandler">Тип обработчика сообщения</typeparam>
    /// <param name="kafkaConsumer"></param>
    internal sealed class KafkaConsumerHostingService<TKey, TPayload, TOptions, TMessageHandler>(
        IKafkaConsumer kafkaConsumer) : BackgroundService
        where TPayload : class
        where TOptions : class, IKafkaConsumerOptions
        where TMessageHandler : class, IKafkaMessageHandler<TKey, TPayload>
    {

        /// <inheritdoc />
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Factory.StartNew(
                () => kafkaConsumer.StartConsumingAsync(stoppingToken),
                stoppingToken,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Current);
        }
    }
}
