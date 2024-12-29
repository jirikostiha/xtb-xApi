using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Xtb.XApi.Extensions.DependencyInjection;

namespace Xtb.XApi.UnitTests;

public class XApiServiceExtensionsTest
{
    [Fact]
    public void AddXApiClient_ShouldRegisterXApiClient()
    {
        var services = new ServiceCollection();

        services.AddXApiClient(options =>
        {
            options.Address = IPAddress.Loopback.ToString();
        });

        var provider = services.BuildServiceProvider();
        var client = provider.GetService<XApiClient>();

        Assert.NotNull(client);
        Assert.NotNull(client.ApiConnector);
        Assert.NotNull(client.ApiConnector.Endpoint);
        Assert.Equal(IPAddress.Loopback, client.ApiConnector.Endpoint.Address);
        Assert.Null(client.AccountId);

        var iclient = provider.GetService<IXApiClient>();
        Assert.NotNull(iclient);

        var aclient = provider.GetService<IXApiClientAsync>();
        Assert.NotNull(aclient);

        var sclient = provider.GetService<IXApiClientSync>();
        Assert.NotNull(sclient);
    }

    [Fact]
    public void AddXApiClient_ShouldRegisterXApiClientViaKey()
    {
        var services = new ServiceCollection();

        services.AddXApiClient("key", options =>
        {
            options.Address = IPAddress.Loopback.ToString();
        });

        var provider = services.BuildServiceProvider();
        var client = provider.GetKeyedService<XApiClient>("key");

        Assert.NotNull(client);
        Assert.NotNull(client.ApiConnector);
        Assert.NotNull(client.ApiConnector.Endpoint);
        Assert.Equal(IPAddress.Loopback, client.ApiConnector.Endpoint.Address);
        Assert.Null(client.AccountId);

        var iclient = provider.GetKeyedService<IXApiClient>("key");
        Assert.NotNull(iclient);

        var aclient = provider.GetKeyedService<IXApiClientAsync>("key");
        Assert.NotNull(aclient);

        var sclient = provider.GetKeyedService<IXApiClientSync>("key");
        Assert.NotNull(sclient);
    }
}