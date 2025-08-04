using DevTools.Auth.Abstractions;
using DevTools.Auth.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DevTools.Auth.Services;

/// <summary>
/// Класс проверки авторизации Keyсloak
/// </summary>
/// <typeparam name="TOptions">Тип опций</typeparam>
internal sealed class JwtValidator<TOptions> : IJwtValidator
    where TOptions : class, IAuthOptions
{
    private readonly TOptions _options;
    private readonly IOpenApiConnectServerKeyReceiver _openApiConnectServerKeyReceiver;
    private readonly ILogger<JwtValidator<TOptions>> _logger;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="JwtValidator1{TOptions}"/>
    /// </summary>
    /// <param name="options"><see cref="IAuthOptions"/></param>
    /// <param name="openApiConnectServerKeyReceiver">Ключ OpenApi</param>
    /// <param name="logger"><see cref="ILogger{T}"/></param>
    public JwtValidator(
        IOptions<TOptions> options,
        IOpenApiConnectServerKeyReceiver openApiConnectServerKeyReceiver,
        ILogger<JwtValidator<TOptions>> logger)
    {
        _options = options.Value;
        _openApiConnectServerKeyReceiver = openApiConnectServerKeyReceiver;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<bool> IsValidAsync(string jwt)
    {
        bool isValid = false;

        if (_options.UseGwtAuthorization != null && _options.UseGwtAuthorization == false)
            return true;

        _logger.LogInformation("Попытка авторизации KeyCloak");
        _logger.LogDebug("Url: {url}", _options.Url);
        _logger.LogDebug("ClientId: {clientId}", _options.ClientId);

        try
        {
            JsonWebTokenHandler jsonWebTokenHandler = new();
            TokenValidationParameters validationParameters = new()
            {
                ValidateLifetime = true,
                ValidAudience = _options.ClientId,
                ValidIssuer = $"{_options.Url}/auth/realms/{_options.Realm}",
                IssuerSigningKey = (await _openApiConnectServerKeyReceiver.GetOpenApiConnectServerPublicKeyAsync())
                                    .Keys
                                    .FirstOrDefault()
            };

            _logger.LogDebug("validationparameters.ValidIssuer : {validIssuer}",
                validationParameters.ValidIssuer);
            _logger.LogDebug("validationparameters.IssuerSigningKey : {issuerSigningKey}",
                validationParameters.IssuerSigningKey);

            var tokenValidationResult = await jsonWebTokenHandler.ValidateTokenAsync(
                jwt.Replace("Bearer ", string.Empty),
                validationParameters);

            isValid = tokenValidationResult.IsValid;

            if (tokenValidationResult.Exception != null)
                _logger.LogWarning("Исключение в KeyCloak: {exception}", tokenValidationResult.Exception.Message);

            _logger.LogInformation("При попытке авторизации KeyCloak ошибок не возникло.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка авторизации.");
        }

        return isValid;
    }
}
