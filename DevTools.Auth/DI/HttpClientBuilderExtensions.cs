using DevTools.Auth.Abstractions;
using DevTools.Auth.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DevTools.Auth.DI;

/// <summary>
/// Расширение для управления зависимостями <see cref="IHttpClientBuilder"/>
/// </summary>
public static class HttpClientBuilderExtensions
{
    /// <summary>
    /// Добавляет аутентификацию токена к создаваемому клиенту
    /// </summary>
    /// <param name="httpClientBuilder"><see cref="IHttpClientBuilder"/></param>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <returns><see cref="IHttpClientBuilder"/></returns>
    public static IHttpClientBuilder AddAuthTikenProvider(this IHttpClientBuilder httpClientBuilder, IServiceCollection services)
    {
        var serverTokenReceiver = services.BuildServiceProvider().GetRequiredService<IOpenApiConnectServerTokenReceiver>();
        var logger = services.BuildServiceProvider().GetRequiredService<ILogger<JwtTokenHeaderHandler>>();

        logger.LogInformation("Logger type: {0}", logger.GetType().FullName);
        return httpClientBuilder.AddHttpMessageHandler(() => new JwtTokenHeaderHandler(serverTokenReceiver, logger));
    }
}
