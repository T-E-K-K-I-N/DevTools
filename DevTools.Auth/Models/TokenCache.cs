namespace DevTools.Auth.Models;

/// <summary>
/// Кешированный ответ от сервера Keycloak
/// </summary>
public class TokenCache
{
    /// <summary>
    /// Токен Keycloak
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Срок жизни токена
    /// </summary>
    public DateTime ExpDate { get; set; }
}

