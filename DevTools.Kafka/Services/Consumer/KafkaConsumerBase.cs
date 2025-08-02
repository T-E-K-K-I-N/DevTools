using Confluent.Kafka;
using DevTools.Kafka.Abstractions;
using DevTools.Kafka.Abstractions.Consumer;
using DevTools.Kafka.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DevTools.Kafka.Services.Consumer
{
    /// <summary>
    /// Базовый потребитель сообщений Kafka
    /// </summary>
    /// <typeparam name="TKey">Тип ключа сообщения</typeparam>
    /// <typeparam name="TBasePayload">Тип базового тела сообщения</typeparam>
    /// <typeparam name="TPayload">Тип тела сообщения</typeparam>
    /// <typeparam name="TOptions">Тип опций</typeparam>
    internal abstract class KafkaConsumerBase<TKey, TBasePayload, TPayload, TOptions> : IKafkaConsumer
        where TPayload : class
        where TOptions : class, IKafkaConsumerOptions
    {
        private readonly ILogger<KafkaConsumerBase<TKey, TBasePayload, TPayload, TOptions>> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IKafkaConsumerOptions _options;
        private readonly IConsumer<TKey, TBasePayload> _consumer;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="KafkaConsumerBase{TKey, TBasePayload, TPayload, TOptions}"/>
        /// </summary>
        /// <param name="logger"><see cref="ILogger{T}"/></param>
        /// <param name="serviceScopeFactory"><see cref="IServiceScopeFactory"/></param>
        /// <param name="options"><see cref="IKafkaConsumerOptions"/></param>
        protected KafkaConsumerBase(
            ILogger<KafkaConsumerBase<TKey, TBasePayload, TPayload, TOptions>> logger, 
            IServiceScopeFactory serviceScopeFactory, 
            IOptions<TOptions> options)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;
            _options = options.Value;

            ConsumerConfig consumerConfig = new()
            {
                GroupId = options.Value.GroupId,
                BootstrapServers = options.Value.Server,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _consumer = new ConsumerBuilder<TKey, TBasePayload>(consumerConfig)
                .Build();
        }

        /// <inheritdoc />
        public async Task StartConsumingAsync(CancellationToken cancellationToken = default)
        {
            _consumer.Subscribe(_options.Topic);

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    _logger.LogDebug("Потребление сообщений топика '{Topic}'", _options.Topic);
                    var consumeResult = _consumer.Consume(cancellationToken);

                    var message = new KafkaMessage<TKey, TPayload>(
                        consumeResult.Message.Key,
                        ConvertMessagePayload(consumeResult.Message.Value),
                        consumeResult.Topic,
                        consumeResult.Partition.Value,
                        consumeResult.Offset.Value,
                        consumeResult.TopicPartitionOffset.Offset.Value);

                    await ProcessMessageAsync(message, cancellationToken);
                }
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogInformation(
                    ex,
                    "Потребление сообщений Kafka остановлено {KafkaServer} {KafkaTopic} {KafkaGroupId}.",
                    _options.Server,
                    _options.Topic,
                    _options.GroupId);
            }
            catch (ConsumeException ex)
            {
                _logger.LogError(
                    "Потребление сообщений Kafka завершилось с ошибкой: {Reason}. {KafkaServer} {KafkaTopic} {KafkaGroupId}.",
                    ex.Error.Reason,
                    _options.Server,
                    _options.Topic,
                    _options.GroupId);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Потребление сообщений Kafka завершилось с ошибкой {KafkaServer} {KafkaTopic} {KafkaGroupId}.",
                    _options.Server,
                    _options.Topic,
                    _options.GroupId);
            }
            finally
            {
                _consumer.Close();
            }
        }

        /// <summary>
        /// Конвертирует базовое тело сообщения
        /// </summary>
        /// <param name="basePayload">Тип базового тела сообщения</param>
        /// <returns>Тело сообщения для обработчика</returns>
        protected abstract TPayload ConvertMessagePayload(TBasePayload basePayload);

        /// <summary>
        /// Обрабатывает сообщение
        /// </summary>
        /// <param name="message">Сообщение из Kafka</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>
        private async Task ProcessMessageAsync(
            KafkaMessage<TKey, TPayload> message,
            CancellationToken cancellationToken = default)
        {
            _logger.LogDebug(
                "Обработка сообщения {Offset} потребителя топика '{Topic}'",
                message.Offset,
                message.Topic);

            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var messageHandler = scope.ServiceProvider.GetRequiredService<IKafkaMessageHandler<TKey, TPayload>>();
                await messageHandler.HandleAsync(message, cancellationToken);
                _logger.LogInformation(
                    "Сообщение {Offset} потребителя топика '{Topic}' успешно обработано",
                    message.Offset,
                    message.Topic);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Сообщение {Offset} потребителя топика '{Topic}' обработано с ошибкой",
                    message.Offset,
                    message.Topic);
            }
        }
    }
}
