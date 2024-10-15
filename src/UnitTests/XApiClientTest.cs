namespace Xtb.XApi.UnitTests;

public class XApiClientTest
{
    [Fact]
    public void Create()
    {
        var client = XApiClient.Create("81.2.190.163", 5112, 5113);

        Assert.NotNull(client.ApiConnector);
        Assert.NotNull(client.ApiConnector.Endpoint);
        Assert.Equal("81.2.190.163", client.ApiConnector.Endpoint.Address.ToString());
        Assert.Null(client.AccountId);
    }
}