using DevTools.Auth.Options;

namespace DevTools.Auth.Abstractions;

/// <summary>
/// Настройки аутентификации
/// </summary>
public abstract record AuthOptionsBase : IAuthOptions
{
    public const string Path = "AuthOptions";

    /// <summary>
    /// Ссылка на Keycloak
    /// </summary>
    public string Url { get; init; } = null!;

    /// <summary>
    /// Realm в Keyclock
    /// </summary>
    public string Realm { get; init; } = null!;

    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public string ClientId { get; init; } = null!;

    /// <summary>
    /// Секрет клиента
    /// </summary>
    public string ClientSecret { get; init; } = null!;

    /// <summary>
    /// Не отключать авторизацию
    /// </summary>
    public bool? UseGwtAuthorization { get; init; } = null!;
}
