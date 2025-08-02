using Confluent.Kafka;

namespace DevTools.Kafka.Abstractions.Producer
{
    /// <summary>
    /// Отправитель Kafka
    /// </summary>
    /// <typeparam name="TKey">Тип ключа сообщения</typeparam>
    /// <typeparam name="TPayload">Тип тела сообщения</typeparam>
    public interface IKafkaProducer<TKey, TPayload> : IDisposable
    {
        /// <summary>
        /// Отправляет сообщение в Kafka
        /// </summary>
        /// <param name="key">Ключ сообщения</param>
        /// <param name="message">Тело сообщения</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>
        Task ProduceAsync(TKey key, TPayload message, CancellationToken cancellationToken = default);
    }
}
