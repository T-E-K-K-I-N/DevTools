namespace DevTools.Auth.Options;

/// <summary>
/// Настройки аутентификации
/// </summary>
public interface IAuthOptions
{
    /// <summary>
    /// Ссылка на Keycloak
    /// </summary>
    public string Url { get; }

    /// <summary>
    /// Realm в Keyclock
    /// </summary>
    public string Realm { get; }

    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public string ClientId { get; }

    /// <summary>
    /// Секрет клиента
    /// </summary>
    public string ClientSecret { get; }

    /// <summary>
    /// Не отключать авторизацию
    /// </summary>
    public bool? UseGwtAuthorization { get; }
}