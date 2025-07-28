namespace DevTools.Kakfa.Abstractions
{
    /// <summary>
    /// Обработчик сообщения Kafka
    /// </summary>
    /// <typeparam name="TKey">Тип ключа сообщения</typeparam>
    /// <typeparam name="TPayload">Тип тела сообщения</typeparam>
    public interface IKafkaMessageHandler<TKey,TPayload>
    {
        /// <summary>
        /// Обрабатывает сообщение
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="Task"/></returns>
        Task HandleAsync(KafkaMessage<TKey, TPayload> message, CancellationToken cancellationToken = default);
    }
}
