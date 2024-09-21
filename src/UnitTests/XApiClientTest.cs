using NSubstitute;

namespace Xtb.XApi.UnitTests;

public class XApiClientTest
{
    [Fact]
    public void Connect()
    {
        var connectedHandler = Substitute.For<EventHandler<EndpointEventArgs>>();

        var client = new XApiClient(null);
        client.Connected += connectedHandler;

        client.Connect();

        Assert.True(client.ApiConnector.IsConnected);
        connectedHandler.Received(1).Invoke(Arg.Any<object>(), Arg.Any<EndpointEventArgs>());
    }

    [Fact]
    public void Connect_IfAlreadyConnected()
    {
        var connectedHandler = Substitute.For<EventHandler<EndpointEventArgs>>();

        var client = new XApiClient(null);
        client.Connected += connectedHandler;

        client.Connect();
        client.Connect();

        Assert.True(client.ApiConnector.IsConnected);
        //connectedHandler.Received(2).Invoke(Arg.Any<object>(), Arg.Any<EndpointEventArgs>());
    }
}