using Confluent.Kafka;
using DevTools.Kafka.Abstractions.Producer;
using DevTools.Kafka.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DevTools.Kafka.Services
{
    /// <summary>
    /// Отправитель сообщений Kafka
    /// </summary>
    /// <typeparam name="TKey">Тип ключа сообщения</typeparam>
    /// <typeparam name="TPayload">Тип тела сообщения</typeparam>
    /// <typeparam name="TOptions">Тип опций</typeparam>
    internal class KafkaProducer<TKey, TPayload, TOptions> : IKafkaProducer<TKey, TPayload>
        where TPayload : class
        where TOptions : class, IKafkaProducerOptions
    {
        private readonly ILogger<KafkaProducer<TKey, TPayload, TOptions>> _logger;
        private readonly IKafkaProducerOptions _options;
        private readonly IProducer<TKey, TPayload> _producer;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="KafkaProducer{TKey, TPayload, TOptions}"/>
        /// </summary>
        /// <param name="logger"><see cref="ILogger{T}"/></param>
        /// <param name="options"><see cref="IKafkaProducerOptions"/></param>
        protected KafkaProducer(
            ILogger<KafkaProducer<TKey, TPayload, TOptions>> logger, 
            IOptions<TOptions> options)
        {
            _logger = logger;
            _options = options.Value;

            ProducerConfig producerConfig = new()
            {
                BootstrapServers = options.Value.Server,
                AllowAutoCreateTopics = options.Value.AutoCreateTopics
            };

            if (Enum.TryParse(options.Value.Partitioner, out Partitioner partitioner))
                producerConfig.Partitioner = partitioner;
            else
                producerConfig.Partitioner = Partitioner.Random;

            if (Enum.TryParse(options.Value.Acks, out Acks acks))
                producerConfig.Acks = acks;
            else
                producerConfig.Acks = Acks.All;

            _producer = new ProducerBuilder<TKey, TPayload>(producerConfig)
                .SetValueSerializer(new KafkaMessageSerializer<TPayload>())
                .Build();
        }

        /// <inheritdoc />
        public async Task ProduceAsync(TKey key, TPayload message, CancellationToken cancellationToken = default)
        {
            try
            {
                DeliveryResult<TKey, TPayload> result;

                if (_options.Partition != null)
                {
                    TopicPartition topicPartition = new(_options.Topic, new Partition((int)_options.Partition));
                    result = await _producer.ProduceAsync(
                        topicPartition, 
                        new Message<TKey, TPayload>() { Key = key, Value = message },
                        cancellationToken);
                }
                else
                {
                    result = await _producer.ProduceAsync(
                        _options.Topic,
                        new Message<TKey, TPayload>() { Key = key, Value = message },
                        cancellationToken);
                }

                _logger.LogInformation(
                    "Сообщение отправлено. Ключ: {Key}, Значение: {Value}, Оффсет: {Offset}, Партиция: {Partition}",
                    result.Message.Key,
                    result.Message.Value,
                    result.Offset,
                    result.Partition);
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogInformation(
                    ex,
                    "Отправка сообщения в Kafka остановлено {KafkaServer} {KafkaTopic} {KafkaPartition}.",
                    _options.Server,
                    _options.Topic,
                    _options.Partition);
            }
            catch (ProduceException<TKey, TPayload> ex)
            {
                _logger.LogError(
                    "Отправка сообщения в Kafka завершилась с ошибкой: {Reason}. {KafkaServer} {KafkaTopic} {KafkaPartition}.", 
                    ex.Error.Reason,
                    _options.Server,
                    _options.Topic,
                    _options.Partition);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Отправка сообщения в Kafka завершилась с ошибкой {KafkaServer} {KafkaTopic} {KafkaPartition}.",
                    _options.Server,
                    _options.Topic,
                    _options.Partition);
            }
        }

        /// <summary>
        /// Освобождает ресурсы после того, как все сообщения будут отправлены в Kafka
        /// </summary>
        public void Dispose()
        {
            _producer.Flush();
            _producer.Dispose();
        }
    }
}
