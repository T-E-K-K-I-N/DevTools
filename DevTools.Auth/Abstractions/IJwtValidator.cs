namespace DevTools.Auth.Abstractions;

/// <summary>
/// Класс проверки авторизации в Keycloak
/// </summary>
internal interface IJwtValidator
{
    /// <summary>
    /// Проверяет валидность аутентификации
    /// </summary>
    /// <param name="jwt">JWT токен авторизации</param>
    /// <returns>Является ли токен валидным</returns>
    Task<bool> IsValidAsync(string jwt);
}