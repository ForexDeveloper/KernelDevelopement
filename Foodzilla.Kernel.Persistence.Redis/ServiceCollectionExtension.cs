namespace Foodzilla.Kernel.Persistence.Redis;

using Microsoft.Extensions.Configuration;
using Foodzilla.Kernel.Commons.Credentials;
using Microsoft.Extensions.DependencyInjection;
using Foodzilla.Kernel.Persistence.Redis.Provider;

public static class ServiceCollectionExtension
{
    /// <summary>
    /// Use this method when each MicroService represents its own connection
    /// </summary>
    /// <param name="services"></param>
    /// <param name="credential"></param>
    public static void AddRedisConnectionProvider(this IServiceCollection services, RedisCredential credential)
    {
        var configuration = Connection.GetConfiguration(credential);

        services.AddHealthChecks().AddRedis(configuration);

        services.AddStackExchangeRedisCache(option =>
        {
            option.Configuration = configuration;
            option.ConfigurationOptions.AbortOnConnectFail = false;
        });
    }

    /// <summary>
    /// Use this method when kernel sets connection for each MicroService
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    public static void AddRedisConnectionProvider(this IServiceCollection services, IConfiguration configuration)
    {
        var credential = configuration.GetSection(nameof(RedisCredential)).Get<RedisCredential>();

        var connectionConfiguration = Connection.GetConfiguration(credential);

        services.AddHealthChecks().AddRedis(connectionConfiguration);

        services.AddStackExchangeRedisCache(option =>
        {
            option.Configuration = connectionConfiguration;
            option.ConfigurationOptions.AbortOnConnectFail = false;
        });
    }
}