using NSubstitute;
using System.Net;

namespace Xtb.XApi.UnitTests;

public class ConnectorTest
{
    [Fact]
    public void SendMessage_WhenDisconnected_Exception()
    {
        var client = new Connector(new IPEndPoint(IPAddress.Loopback, 5921));

        Assert.Throws<APICommunicationException>(() => client.SendMessage("abc"));
    }

    [Fact]
    public void SendMessageAsync_WhenDisconnected_Exception()
    {
        var client = new Connector(new IPEndPoint(IPAddress.Loopback, 5921));

        Assert.ThrowsAsync<APICommunicationException>(async () => await client.SendMessageAsync("abc"));
    }

    [Fact]
    public void Disconnect_WhenNotConnected_ShouldNotInvokeDisconnectedEvent()
    {
        var client = new Connector(new IPEndPoint(IPAddress.Loopback, 5921));
        var disconnectedHandler = Substitute.For<EventHandler>();
        client.Disconnected += disconnectedHandler;

        client.Disconnect();

        disconnectedHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
    }
}