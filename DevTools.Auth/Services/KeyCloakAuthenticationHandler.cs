using DevTools.Auth.Abstractions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace DevTools.Auth.Services;

/// <summary>
/// Обработчик аутентификации Keycloak
/// </summary>
internal sealed class KeyCloakAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IJwtValidator _jwtValidator;
    private readonly ILogger<KeyCloakAuthenticationHandler> _logger;

    /// <summary>
    /// Инифиализирует новый экземпляр класса <see cref="KeyCloakAuthenticationHandler"/>
    /// </summary>
    /// <param name="options">Опции мониторинга</param>
    /// <param name="loggerFactory"><see cref="ILoggerFactory"/></param>
    /// <param name="encoder">Кодировщик URL</param>
    /// <param name="jwtValidator">Валидатор прав</param>
    /// <param name="logger"><see cref="ILogger{T}"/></param>
    /// <param name="systemClock"><see cref="ISystemClock"/></param>
    public KeyCloakAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory loggerFactory,
        UrlEncoder encoder,
        IJwtValidator jwtValidator,
        ILogger<KeyCloakAuthenticationHandler> logger,
        ISystemClock systemClock)
        : base(options, loggerFactory, encoder, systemClock)
    {
        _jwtValidator = jwtValidator;
        _logger = logger;
    }

    /// <summary>
    /// Обработчик аутентификации
    /// </summary>
    /// <returns>Результат аутентификации <see cref="AuthenticateResult"/></returns>
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        _logger.LogInformation("Попытка проверки токена.");

        if(!Context.Request.Headers.TryGetValue("Authorization", out StringValues authorizationHeaders))
            return AuthenticateResult.NoResult();

        _logger.LogDebug("Попытка проверки токена. " +
            "Значение токена: {AuthorizationHeaders}", authorizationHeaders.ToArray());

        if(!(await _jwtValidator.IsValidAsync(authorizationHeaders.ToString())))
        {
            _logger.LogError("Неудачная авторизация. " +
                "Значение токена: {AuthorizationHeaders}", authorizationHeaders.ToArray());
            return AuthenticateResult.Fail("Неудачная авторизация.");
        }

        _logger.LogInformation("Авторизация успешна.");
        ClaimsIdentity claimsIdentity = new("Custom");
        ClaimsPrincipal claimsPrincipal = new(claimsIdentity);

        return AuthenticateResult.Success(
            new AuthenticationTicket(claimsPrincipal, 
            new AuthenticationProperties(), 
            string.Empty));
    }

    /// <summary>
    /// Обработчик ошибки 401
    /// </summary>
    /// <param name="properties">Свойства аутентификации</param>
    /// <returns>Результат ошибки аутентификации <see cref="AuthenticateResult.Failure"/></returns>
    protected override async Task<AuthenticateResult> HandleChallengeAsync(AuthenticationProperties properties)
    {
        Response.StatusCode = 401;
        string message = "Предоставьте корректный токен.";
        _logger.LogError("Неудачная авторизация. Некорректный токен.");

        await Response.Body.WriteAsync(Encoding.UTF8.GetBytes(message));

        return AuthenticateResult.Fail(message);
    }

    /// <summary>
    /// Обработчик ошибки 403
    /// </summary>
    /// <param name="properties">Свойства аутентификации</param>
    /// <returns>Результат ошибки аутентификации <see cref="AuthenticateResult.Failure"/></returns>
    protected override async Task<AuthenticateResult> HandleForbiddenAsync(AuthenticationProperties properties)
    {
        Response.StatusCode = 403;
        string message = "Недостаточно прав.";
        _logger.LogError("Неудачная авторизация. Нет прав.");

        await Response.Body.WriteAsync(Encoding.UTF8.GetBytes(message));

        return AuthenticateResult.Fail(message);
    }
}
