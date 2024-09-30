using Microsoft.Extensions.DependencyInjection;
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
            options.Address = "127.0.0.1";
        });

        var provider = services.BuildServiceProvider();
        var client = provider.GetService<XApiClient>();

        Assert.NotNull(client);
    }

    [Fact]
    public void AddXApiClient_ShouldRegisterXApiClientViaKey()
    {
        var services = new ServiceCollection();

        services.AddXApiClient(options =>
        {
            options.Address = "127.0.0.1";
        }, "key");

        var provider = services.BuildServiceProvider();
        var client = provider.GetKeyedService<XApiClient>("key");

        Assert.NotNull(client);
    }
}