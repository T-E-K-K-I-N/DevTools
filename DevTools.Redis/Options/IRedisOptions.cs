namespace DevTools.Redis.Options;

/// <summary>
/// Опции Redis
/// </summary>
public interface IRedisOptions
{
    /// <summary>
    /// Хост
    /// </summary>
    string Host { get; }

    /// <summary>
    /// Порт
    /// </summary>
    int Port { get; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    string? Username { get; }

    /// <summary>
    /// Пароль
    /// </summary>
    string? Password { get; }

    /// <summary>
    /// Номер БД
    /// </summary>
    int? Database {  get; }

    /// <summary>
    /// Префикс всех ключей
    /// </summary>
    string? KeysPrefix { get; }

    /// <summary>
    /// Время истечения срока действия ключа по умолчанию в сек.
    /// </summary>
    double? DefaultExpiratiobSeconds { get; }
}
