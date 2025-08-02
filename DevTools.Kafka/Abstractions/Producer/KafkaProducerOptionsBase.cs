using DevTools.Kafka.Options;

namespace DevTools.Kafka.Abstractions.Producer
{
    public abstract record KafkaProducerOptionsBase : IKafkaProducerOptions
    {
        public const string Path = "Kafka:ProducerOptions";

        /// <summary>
        /// Сервер Kafka
        /// </summary>
        public string Server { get; init; } = null!;

        /// <summary>
        /// Название топика
        /// </summary>
        public string Topic { get; init; } = null!;

        /// <summary>
        /// Разделение по партициям
        /// </summary>
        public string Partitioner { get; init; } = null!;

        /// <summary>
        /// Номер партиции
        /// </summary>
        public int? Partition { get; init; } = null!;

        /// <summary>
        /// Создавать топик в случае его отсутствия 
        /// </summary>
        public bool? AutoCreateTopics { get; init; } = null!;

        /// <summary>
        /// Какие партиции должны подтвердить запись сообщения
        /// </summary>
        public string Acks { get; init; } = null!;
    }
}
