namespace DevTools.Kafka.Abstractions
{
    /// <summary>
    /// Потребитель Kafka
    /// </summary>
    public interface IKafkaConsumer
    {
        /// <summary>
        /// Начинает потребление сообщений
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"></param>
        /// <returns><see cref="Task"></returns>
        Task StartConsumingAsync(CancellationToken cancellationToken = default);
    }
}
