namespace DevTools.Kakfa.Options
{
    /// <summary>
    /// Настройки консьюмера
    /// </summary>
    public interface IKafkaConsumerOptions
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
        /// Идентификатор группы потребителей
        /// </summary>
        public string GroupId { get; }
    }
}
