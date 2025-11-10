namespace DevTools.Redis.Abstractions;

/// <summary>
/// Кеш с JSON сериализацией
/// </summary>
public interface IJsonCache
{
    /// <summary>
    /// Устанавливает значение
    /// </summary>
    /// <typeparam name="TValue">Тип значения</typeparam>
    /// <param name="key">Ключ</param>
    /// <param name="value">Значение</param>
    /// <param name="expiration">Время истечения срока действия ключа в сек.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="Task"/></returns>
    Task SetAsync<TValue>(
        string key,
        TValue value,
        TimeSpan? expiration,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Получает значение
    /// </summary>
    /// <typeparam name="TValue">Тип значения</typeparam>
    /// <param name="key">Ключ</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns>Значение или null</returns>
    Task<TValue?> GetAsync<TValue>(string key, CancellationToken cancellationToken = default);

    /// <summary>
    /// Получает значение или создает, если его нет
    /// </summary>
    /// <typeparam name="TValue">Тип значения</typeparam>
    /// <param name="key">Ключ</param>
    /// <param name="factory">Фабрика создания значения</param>
    /// <param name="expiration">Время истечения срока действия ключа в сек.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns>Значение</returns>
    Task<TValue> GetOrCreateAsync<TValue>(
        string key,
        Func<ValueTask<TValue>> factory,
        TimeSpan? expiration,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Удаляет значение
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="Task"/></returns>
    Task RemoveAsync(string key, CancellationToken cancellationToken = default);
}
