using Microsoft.IdentityModel.Tokens;

namespace DevTools.Auth.Abstractions;

/// <summary>
/// Класс, получающий серверный ключ OpenAPIConnect
/// </summary>
public interface IOpenApiConnectServerKeyReceiver
{
    /// <summary>
    /// Получить публичный серверный ключ
    /// </summary>
    /// <returns><see cref="JsonWebKeySet"/></returns>
    Task<JsonWebKeySet> GetOpenApiConnectServerPublicKeyAsync();
}
