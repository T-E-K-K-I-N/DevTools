using DevTools.Auth.Abstractions;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;

namespace DevTools.Auth.Services;

/// <summary>
/// Перехватчик отправки http-запросов для добавления в них заголовков с авторизацией 
/// </summary>
public class JwtTokenHeaderHandler : DelegatingHandler
{
    private readonly IOpenApiConnectServerTokenReceiver _tokenReceiver;
    private readonly ILogger<JwtTokenHeaderHandler> _logger;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="JwtTokenHeaderHandler"/>
    /// </summary>
    /// <param name="tokenReceiver">Сервис, позволяющий получить токен авторизации</param>
    /// <param name="logger"><see cref="ILogger{T}"/></param>
    public JwtTokenHeaderHandler(IOpenApiConnectServerTokenReceiver tokenReceiver, 
        ILogger<JwtTokenHeaderHandler> logger)
    {
        _tokenReceiver = tokenReceiver;
        _logger = logger;
    }

    /// <summary>
    /// Перехватчик процесса отправки http-запроса.
    /// Подставляет в запрос Authorization заголовок с Bearer авторизацией
    /// </summary>
    /// <param name="request">http запрос</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="HttpResponseMessage"/></returns>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Попытка получения JWT токена из KeyCloak.");
        var jwtToken = await _tokenReceiver.GetOpenApiConnectServerTokenAsync();

        if (jwtToken?.IsSuccess == true)
        {
            _logger.LogInformation("Получен JWT токен из KeyCloak.");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken.Value);
        }
        else
        {
            _logger.LogError("Ошибка получения JWT токена из KeyCloak. Причина: {0}", jwtToken?.Errors);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
