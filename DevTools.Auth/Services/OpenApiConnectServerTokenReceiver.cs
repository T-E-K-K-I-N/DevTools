using DevTools.Auth.Abstractions;
using DevTools.Auth.Models;
using DevTools.Auth.Options;
using FluentResults;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace DevTools.Auth.Services;

/// <summary>
/// Класс, получающий серверный токен OpenAPIConnect
/// </summary>
/// <typeparam name="TOptions">Тип опций</typeparam>
internal sealed class OpenApiConnectServerTokenReceiver<TOptions> : IOpenApiConnectServerTokenReceiver
    where TOptions : class, IAuthOptions
{
    private readonly TOptions _options;
    private readonly IMemoryCache _memoryCache;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<OpenApiConnectServerTokenReceiver<TOptions>> _logger;

    public OpenApiConnectServerTokenReceiver(
        IOptions<TOptions> options,
        IMemoryCache memoryCache,
        IHttpClientFactory httpClientFactory,
        ILogger<OpenApiConnectServerTokenReceiver<TOptions>> logger)
    {
        _options = options.Value;
        _memoryCache = memoryCache;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<Result<string>> GetOpenApiConnectServerTokenAsync()
    {
        _logger.LogInformation("Запрос Token.");

        if(_memoryCache.TryGetValue<TokenCache>(
            nameof(OpenApiConnectServerTokenReceiver<TOptions>), 
            out var jsonWebTokenCache) &&
            jsonWebTokenCache?.ExpDate > DateTime.Now)
        {
            _logger.LogInformation("Получен JWT токен из кеша.");
            return jsonWebTokenCache.AccessToken;
        }

        using HttpClient client = _httpClientFactory.CreateClient();
        string url = $"{_options.Url}/auth/realms/{_options.Realm}/protocol/openid-connect/token";

        Dictionary<string, string> formData = new()
        {
            { "grant_type", "client_credentials" },
            { "client_id", _options.ClientId },
            { "client_secret", _options.ClientSecret }
        };

        _logger.LogInformation("Токен запрошен из {0}", _options.Url);
        FormUrlEncodedContent postContent = new(formData);
        var result = await client.PostAsync(url, postContent);
        result.EnsureSuccessStatusCode();
        var jsonResponse = await result.Content.ReadFromJsonAsync<KeyCloackResponse>();

        if(jsonResponse?.AccessToken == null)
        {
            _logger.LogError("Ошибка получения JWT токена из {0}", _options.Url);
            var responseContent = await result.Content.ReadAsStringAsync();
            return Result.Fail(new Error("Ошибка при попытке получения токена KeyCloak. " +
                $"Результат запроса: {responseContent}."));
        }

        _logger.LogInformation("Токен получен из {0}", _options.Url);
        TokenCache jsonWebToken = new()
        {
            AccessToken = jsonResponse.AccessToken,
            ExpDate = DateTime.Now.AddSeconds(jsonResponse.ExpiresIn)
        };

        _memoryCache.Set(nameof(OpenApiConnectServerTokenReceiver<TOptions>), jsonWebToken);

        return jsonResponse.AccessToken;
    }
}
