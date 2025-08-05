using DevTools.Auth.Abstractions;
using DevTools.Auth.Options;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DevTools.Auth.Services;

/// <summary>
/// Класс, получающий серверный ключ OpenAPIConnect
/// </summary>
/// <typeparam name="TOptions">Тип опций</typeparam>
internal sealed class OpenApiConnectServerKeyReceiver<TOptions> : IOpenApiConnectServerKeyReceiver
    where TOptions : class, IAuthOptions
{
    private readonly TOptions _options;
    private readonly IMemoryCache _memoryCache;
    private readonly IHttpClientFactory _httpClientFactory;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="OpenApiConnectServerKeyReceiver{TOptions}"/>
    /// </summary>
    /// <param name="options"><see cref="IAuthOptions"/></param>
    /// <param name="memoryCache">Кеш</param>
    /// <param name="httpClientFactory"><see cref="IHttpClientFactory"/></param>
    public OpenApiConnectServerKeyReceiver(
        IOptions<TOptions> options,
        IMemoryCache memoryCache,
        IHttpClientFactory httpClientFactory)
    {
        _options = options.Value;
        _memoryCache = memoryCache;
        _httpClientFactory = httpClientFactory;
    }

    
    /// <inheritdoc />
    public async Task<JsonWebKeySet> GetOpenApiConnectServerPublicKeyAsync()
    {
        if(_memoryCache.TryGetValue<JsonWebKeySet>(
            nameof(OpenApiConnectServerKeyReceiver<TOptions>), 
            out var jsonWebKey))
        {
            ArgumentNullException.ThrowIfNull(jsonWebKey);
            return jsonWebKey;
        }

        using HttpClient httpClient = _httpClientFactory.CreateClient();
        string url = $"{_options.Url}/auth/realms/{_options.Realm}/protocol/openid-connect/certs";

        var jsonCert = await httpClient.GetStringAsync(url);
        JsonWebKeySet jsonWebKeySet = new(jsonCert);

        _memoryCache.Set(nameof(OpenApiConnectServerKeyReceiver<TOptions>), jsonWebKeySet);

        return jsonWebKeySet;
    }
}

