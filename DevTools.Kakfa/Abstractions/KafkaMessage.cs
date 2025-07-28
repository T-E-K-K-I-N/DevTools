namespace DevTools.Kakfa.Abstractions
{
    /// <summary>
    /// Сообщение из Kafka
    /// </summary>
    /// <typeparam name="TKey">Тип ключа</typeparam>
    /// <typeparam name="TPayload">Тип сообщения</typeparam>
    /// <param name="Key">Ключ сообщения</param>
    /// <param name="Payload">Тело сообщения</param>
    /// <param name="Topic">Название топика</param>
    /// <param name="Partition">Номер партиции</param>
    /// <param name="Offset">Оффсет сообщения</param>
    /// <param name="PartitionOffset">Оффсет сообщения в партиции</param>
    public sealed record KafkaMessage<TKey, TPayload>(
        TKey Key,
        TPayload Payload,
        string Topic,
        int Partition,
        long Offset,
        long PartitionOffset);
}
