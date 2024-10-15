using System.Threading;

namespace Xtb.XApi.UnitTests;

public class XApiClientTest
{
    [Fact]
    public async Task Create()
    {
        var client = XApiClient.Create("81.2.190.163", 5112, 5113);
        await client.ConnectAsync();
        await client.LoginAsync(new Credentials("login", "password"));
        var openTrades = await client.GetTradesAsync(true);

        Assert.NotNull(client.ApiConnector);
        Assert.NotNull(client.ApiConnector.Endpoint);
        Assert.Equal("81.2.190.163", client.ApiConnector.Endpoint.Address.ToString());
        Assert.Null(client.AccountId);
    }
}