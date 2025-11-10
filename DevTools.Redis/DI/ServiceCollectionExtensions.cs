using DevTools.Redis.Abstractions;
using DevTools.Redis.Exceptions;
using DevTools.Redis.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevTools.Redis.DI;

/// <summary>
/// Расширения для управления зависимостями <see cref="IServiceCollection"/>
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавляет Redis в переданную коллекцию сервисов
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <param name="configuration"><see cref="IConfiguration"/></param>
    /// <returns>Переданный <see cref="IServiceCollection"/></returns>
    /// <exception cref="RedisConfigurationException">Ошибка конфигурации Redis</exception>
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<RedisOptionsBase>(configuration.GetSection(RedisOptionsBase.Path));
        services.AddStackExchangeRedisCache(options =>
        {
            var redisOptions = configuration
            .GetRequiredSection(RedisOptionsBase.Path)
            .Get<RedisOptionsBase>() ?? throw new RedisConfigurationException("Не удалось получить настройки Redis");

            options.InstanceName = redisOptions.KeysPrefix;
            options.ConfigurationOptions = new()
            {
                EndPoints =
                {
                    $"{redisOptions.Host}:{redisOptions.Port}"
                },
                User = redisOptions.Username,
                Password = redisOptions.Password,
                DefaultDatabase = redisOptions.Database
            };
        });

        return services;
    }

    /// <summary>
    /// Добавляет JSON кеш в переданную коллекцию сервисов
    /// </summary>
    /// <param name="services"><see cref="IServiceCollection"/></param>
    /// <returns>Переданный <see cref="IServiceCollection"/></returns>
    public static IServiceCollection AddJsonCache(this IServiceCollection services)
    {
        services.AddSingleton<IJsonCache, JsonCache<RedisOptionsBase>>();
        return services;
    }
}
