using DevTools.Kafka.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DevTools.Kafka.Services
{
    /// <summary>
    /// Потребитель Protobuf сообщений Kafka
    /// </summary>
    /// <typeparam name="TKey">Тип ключа сообщения</typeparam>
    /// <typeparam name="TPayload">Protobuf тип тела сообщения</typeparam>
    /// <typeparam name="TOptions">Тип опций</typeparam>
    internal sealed class KafkaProtobufConsumer<TKey, TPayload, TOptions>
        : KafkaConsumerBase<TKey, byte[], TPayload, TOptions>
        where TPayload : class
        where TOptions : class, IKafkaConsumerOptions
    {
        private readonly Func<byte[], TPayload> _payloadAdapter;

        /// <summary>
        /// Инициализирует новый экземпляр класса <see cref="KafkaProtobufConsumer{TKey, TPayload, TOptions}"/>
        /// </summary>
        /// <param name="logger"><see cref="ILogger{T}"/></param>
        /// <param name="serviceScopeFactory"><see cref="IServiceScopeFactory"/></param>
        /// <param name="options"><see cref="IKafkaConsumerOptions"/></param>
        /// <param name="payloadAdapter">Адаптер преобразования тела сообщения в байтах в Protobuf</param>
        public KafkaProtobufConsumer(
            ILogger<KafkaProtobufConsumer<TKey, TPayload, TOptions>> logger,
            IServiceScopeFactory serviceScopeFactory,
            IOptions<TOptions> options,
            Func<byte[], TPayload> payloadAdapter)
            : base (logger, serviceScopeFactory, options) 
        {
            _payloadAdapter = payloadAdapter;
        }

        /// <inheritdoc />
        protected override TPayload ConvertMessagePayload(byte[] basePayload) => _payloadAdapter(basePayload);
    }
}
