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
    /// The <see cref="Action{MemoryCacheOptions}"/> to configure the provided <see cref="MemoryCacheOptions"/>.
    /// </param>
    /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
    public static IServiceCollection AddXApiClient(this IServiceCollection services, Action<AddXApiClientOptions> setupAction)
    {
#if NET7_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(services);
#else
        _ = services ?? throw new ArgumentNullException(nameof(services));
#endif

        var options = new AddXApiClientOptions();
        setupAction(options);
        services.Configure(setupAction);


        services.TryAdd(ServiceDescriptor.Singleton<IClient, Connector>());
        var streamingApiConnector = StreamingApiConnector.Create(options.Host, options.StreamingPort, options.StreamingListener);
        services.TryAdd(ServiceDescriptor.Singleton(streamingApiConnector));
        services.TryAdd(ServiceDescriptor.Singleton(new ApiConnector()));

        services.TryAdd(ServiceDescriptor.Singleton<IXApiClientAsync, XApiClient>(new XApiClient()));


        //services.AddOptions();
        //services.TryAdd(ServiceDescriptor.Singleton<IDistributedCache, MemoryDistributedCache>());
        //services.Configure(setupAction); ??

        return services;
    }
}