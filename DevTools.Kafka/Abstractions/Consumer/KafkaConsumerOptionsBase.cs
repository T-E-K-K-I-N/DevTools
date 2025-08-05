using DevTools.Kafka.Options;

namespace DevTools.Kafka.Abstractions.Consumer
{
    /// <summary>
    /// Настройки консьюмера
    /// </summary>
    public abstract record KafkaConsumerOptionsBase : IKafkaConsumerOptions
    {
        public const string Path = "Kafka:ConsumerOptions";

        /// <summary>
        /// Сервер Kafka
        /// </summary>
        public string Server { get; init; } = null!;

        /// <summary>
        /// Название топика
        /// </summary>
        public string Topic { get; init; } = null!;

        /// <summary>
        /// Идентификатор группы потребителей
        /// </summary>
        public string GroupId { get; init; } = null!;
    }
}
