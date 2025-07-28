using DevTools.Kafka.Exceptions;
using DevTools.Kafka.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
namespace DevTools.Kafka.Services
{
    /// <summary>
    /// Потребитель JSON сообщений Kafka
    /// </summary>
    /// <typeparam name="TKey">Тип ключа сообщения</typeparam>
    /// <typeparam name="TPayload">JSON тип тела сообщения</typeparam>
    /// <typeparam name="TOptions">Тип опций</typeparam>
    internal sealed class KafkaJsonConsumer<TKey, TPayload, TOptions>
        : KafkaConsumerBase<TKey, string, TPayload, TOptions>
        where TPayload : class
        where TOptions : class, IKafkaConsumerOptions
    {
        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="KafkaJsonConsumer{TKey, TPayload, TOptions}"/>
        /// </summary>
        /// <param name="logger"><see cref="ILogger{T}"/></param>
        /// <param name="serviceScopeFactory"><see cref="IServiceScopeFactory"/></param>
        /// <param name="options"><see cref="IKafkaConsumerOptions"/></param>
        public KafkaJsonConsumer(
            ILogger<KafkaJsonConsumer<TKey, TPayload, TOptions>> logger,
            IServiceScopeFactory serviceScopeFactory,
            IOptions<TOptions> options)
            : base (logger, serviceScopeFactory, options) 
        {
        }

        protected override TPayload ConvertMessagePayload(string basePayload) =>
            JsonSerializer.Deserialize<TPayload>(basePayload) 
            ?? throw new InvalidMessagePayloadException("Некорректное тело сообщения.");
    }
}
