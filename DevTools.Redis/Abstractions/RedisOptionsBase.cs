using DevTools.Redis.Options;

namespace DevTools.Redis.Abstractions;

/// <inheritdoc />
public abstract record RedisOptionsBase : IRedisOptions
{
    public const string Path = "Redis";

    /// <summary>
    /// Хост
    /// </summary>
    public string Host { get; init; } = null!;

    /// <summary>
    /// Порт
    /// </summary>
    public int Port { get; init; } = 6379;

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string? Username { get; init; } = null!;

    /// <summary>
    /// Пароль
    /// </summary>
    public string? Password { get; init; } = null!;

    /// <summary>
    /// Номер БД
    /// </summary>
    public int? Database { get; init; } = null!;

    /// <summary>
    /// Префикс всех ключей
    /// </summary>
    public string? KeysPrefix { get; init; } = null!;

    /// <summary>
    /// Время истечения срока действия ключа по умолчанию в сек.
    /// </summary>
    public double? DefaultExpiratiobSeconds { get; init; } = null!;
}
