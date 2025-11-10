namespace DevTools.Redis.Exceptions;

/// <summary>
/// Ошибка конфигурации Redis
/// </summary>
public sealed class RedisConfigurationException : Exception
{
    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="RedisConfigurationException"/>
    /// </summary>
    /// <param name="message">Сообщение</param>
    public RedisConfigurationException(string? message)
        : base(message)
    { 
    }
}
