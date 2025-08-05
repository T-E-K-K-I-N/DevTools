namespace DevTools.Kafka.Options
{
    public interface IKafkaProducerOptions
    {
        /// <summary>
        /// Сервер Kafka
        /// </summary>
        public string Server { get; }

        /// <summary>
        /// Название топика
        /// </summary>
        public string Topic { get; }

        /// <summary>
        /// Разделение по партициям
        /// </summary>
        public string Partitioner { get; }

        /// <summary>
        /// Номер партиции
        /// </summary>
        public int? Partition { get; }

        /// <summary>
        /// Создавать топик в случае его отсутствия 
        /// </summary>
        public bool? AutoCreateTopics { get; }

        /// <summary>
        /// Какие партиции должны подтвердить запись сообщения
        /// </summary>
        public string Acks { get; }
    }
}
