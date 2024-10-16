using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Xtb.XApi.Extensions.DependencyInjection;

/// <summary>
/// Configuration extension methods for <see cref="Xtb.XApi.XApiClient"/>.
/// </summary>
public static class XApiServiceExtensions
{
    /// <summary>
    /// Adds a ... to the <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="setupAction">
    /// The <see cref="Action{AddXApiClientOptions}"/> to configure the provided <see cref="AddXApiClientOptions"/>.
    /// </param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddXApiClient(this IServiceCollection services, Action<XApiClientOptions> setupAction)
    {
#if NET7_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(services);
#else
        _ = services ?? throw new ArgumentNullException(nameof(services));
#endif

        var options = new XApiClientOptions();
        setupAction(options);
        services.Configure(setupAction);

        var xapiClient = XApiClient.Create(options.Address, options.MainPort, options.StreamingPort, options.StreamingListener);
        services.TryAdd(ServiceDescriptor.Singleton(xapiClient));
        services.TryAdd(ServiceDescriptor.Singleton<IXApiClient>(provider => xapiClient));
        services.TryAdd(ServiceDescriptor.Singleton<IXApiClientSync>(provider => xapiClient));
        services.TryAdd(ServiceDescriptor.Singleton<IXApiClientAsync>(provider => xapiClient));

        return services;
    }

    /// <summary>
    /// Adds a ... to the <see cref="IServiceCollection" />.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
    /// <param name="setupAction">
    /// The <see cref="Action{AddXApiClientOptions}"/> to configure the provided <see cref="XApiClientOptions"/>.
    /// </param>
    /// <param name="key">The service key. </param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddXApiClient(this IServiceCollection services, string key, Action<XApiClientOptions> setupAction)
    {
#if NET7_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(services);
#else
        _ = services ?? throw new ArgumentNullException(nameof(services));
#endif

        var options = new XApiClientOptions();
        setupAction(options);
        services.Configure(setupAction);

        var xapiClient = XApiClient.Create(options.Address, options.MainPort, options.StreamingPort, options.StreamingListener);
        services.TryAddKeyedSingleton(key, ServiceDescriptor.Singleton(xapiClient));
        services.TryAddKeyedSingleton(key, ServiceDescriptor.Singleton<IXApiClientAsync>(provider => xapiClient));
        services.TryAdd(ServiceDescriptor.KeyedSingleton<IXApiClientSync>(key, xapiClient));
        services.TryAdd(ServiceDescriptor.KeyedSingleton<IXApiClientAsync>(key, xapiClient));

        return services;
    }
}