using DevTools.Auth.Abstractions;
using DevTools.Auth.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevTools.Auth.DI;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавляет аутентификацию в переданную коллекцию сервисов
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <param name="schemaName">Название схемы</param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddAuth(
        this IServiceCollection services, 
        IConfiguration configuration, 
        string schemaName)
    {
        services.AddAuthentication(schemaName)
            .AddScheme<AuthenticationSchemeOptions, KeyCloakAuthenticationHandler>(
            schemaName, 
            opt => { });

        return services;
    }

    /// <summary>
    /// Добавляет сервис по получению токена в коллецию сервисов
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <returns><see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddKeyCloakTokenReceiver(
        this IServiceCollection services, 
        IConfiguration configuration)
    {
        services.Configure<AuthOptionsBase>(configuration.GetSection(AuthOptionsBase.Path));
        services.AddSingleton<IOpenApiConnectServerTokenReceiver, OpenApiConnectServerTokenReceiver<AuthOptionsBase>>();

        return services;
    }
}
