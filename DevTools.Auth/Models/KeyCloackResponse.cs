using System.Text.Json.Serialization;

namespace DevTools.Auth.Models;

/// <summary>
/// Ответ от сервера Keycloak
/// </summary>
public class KeyCloackResponse
{
    /// <summary>
    /// Токен Keycloak
    /// </summary>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    /// Срок жизни токена
    /// </summary>
    [JsonPropertyName("expires_in")]
    public int ExpiresIn { get; set; }
}
