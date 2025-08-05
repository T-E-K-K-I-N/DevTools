using FluentResults;

namespace DevTools.Auth.Abstractions;
    
/// <summary>
/// Класс, получающий серверный токен OpenAPIConnect
/// </summary>
public interface IOpenApiConnectServerTokenReceiver
{
    /// <summary>
    /// Получает публичный серверный токен
    /// </summary>
    /// <returns><see cref="Result"/> с токеном или ошибкой</returns>
    Task<Result<string>> GetOpenApiConnectServerTokenAsync();
}
