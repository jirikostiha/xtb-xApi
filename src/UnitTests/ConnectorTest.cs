using NSubstitute;
using System.Net;

namespace Xtb.XApiClient.UnitTests;

public class ConnectorTest
{
    #region disconnection

    [Fact]
    public void SendMessage_WhenDisconnected_Exception()
    {
        var client = new Connector(new IPEndPoint(IPAddress.Loopback, 5921));

        Assert.Throws<APICommunicationException>(() => client.SendMessage("abc"));
    }

    [Fact]
    public async Task SendMessageAsync_WhenDisconnected_Exception()
    {
        var client = new Connector(new IPEndPoint(IPAddress.Loopback, 5921));

        await Assert.ThrowsAsync<APICommunicationException>(async () => await client.SendMessageAsync("abc"));
    }

    [Fact]
    public void ReadMessage_WhenDisconnected_Exception()
    {
        var client = new Connector(new IPEndPoint(IPAddress.Loopback, 5921));

        Assert.Throws<APICommunicationException>(client.ReadMessage);
    }

    [Fact]
    public async Task ReadMessageAsync_WhenDisconnected_Exception()
    {
        var client = new Connector(new IPEndPoint(IPAddress.Loopback, 5921));

        await Assert.ThrowsAsync<APICommunicationException>(async () => await client.ReadMessageAsync());
    }

    [Fact]
    public void SendMessageWaitResponse_WhenDisconnected_Exception()
    {
        var client = new Connector(new IPEndPoint(IPAddress.Loopback, 5921));

        Assert.Throws<APICommunicationException>(() => client.SendMessageWaitResponse("abc"));
    }

    [Fact]
    public async Task SendMessageWaitResponseAsync_WhenDisconnected_Exception()
    {
        var client = new Connector(new IPEndPoint(IPAddress.Loopback, 5921));

        await Assert.ThrowsAsync<APICommunicationException>(async () => await client.SendMessageWaitResponseAsync("abc"));
    }

    [Fact]
    public void Disconnect_WhenDisconnected_ShouldNotInvokeDisconnectedEvent()
    {
        var client = new Connector(new IPEndPoint(IPAddress.Loopback, 5921));
        var disconnectedHandler = Substitute.For<EventHandler>();
        client.Disconnected += disconnectedHandler;

        client.Disconnect();

        disconnectedHandler.DidNotReceive().Invoke(Arg.Any<object>(), Arg.Any<EventArgs>());
    }

    #endregion disconnection

    #region dispose

    [Fact]
    public void Connect_WhenDisposed_Exception()
    {
        var client = new Connector(new IPEndPoint(IPAddress.Loopback, 5921));

        client.Dispose();
        Assert.Throws<ObjectDisposedException>(client.Connect);
    }

    [Fact]
    public async Task ConnectAsync_WhenDisposed_Exception()
    {
        var client = new Connector(new IPEndPoint(IPAddress.Loopback, 5921));

        client.Dispose();
        await Assert.ThrowsAsync<ObjectDisposedException>(async () => await client.ConnectAsync());
    }

    [Fact]
    public void SendMessage_WhenDisposed_Exception()
    {
        var client = new Connector(new IPEndPoint(IPAddress.Loopback, 5921));

        client.Dispose();
        Assert.Throws<ObjectDisposedException>(() => client.SendMessage("abc"));
    }

    [Fact]
    public async Task SendMessageAsync_WhenDisposed_Exception()
    {
        var client = new Connector(new IPEndPoint(IPAddress.Loopback, 5921));

        client.Dispose();
        await Assert.ThrowsAsync<ObjectDisposedException>(async () => await client.SendMessageAsync("abc"));
    }

    [Fact]
    public void ReadMessage_WhenDisposed_Exception()
    {
        var client = new Connector(new IPEndPoint(IPAddress.Loopback, 5921));

        client.Dispose();
        Assert.Throws<ObjectDisposedException>(client.ReadMessage);
    }

    [Fact]
    public async Task ReadMessageAsync_WhenDisposed_Exception()
    {
        var client = new Connector(new IPEndPoint(IPAddress.Loopback, 5921));

        client.Dispose();
        await Assert.ThrowsAsync<ObjectDisposedException>(async () => await client.ReadMessageAsync());
    }

    [Fact]
    public void Disconnect_WhenDisposed_Exception()
    {
        var client = new Connector(new IPEndPoint(IPAddress.Loopback, 5921));

        client.Dispose();
        Assert.Throws<ObjectDisposedException>(client.Disconnect);
    }

    [Fact]
    public async Task DisconnectAsync_WhenDisposed_Exception()
    {
        var client = new Connector(new IPEndPoint(IPAddress.Loopback, 5921));

        client.Dispose();
        await Assert.ThrowsAsync<ObjectDisposedException>(async () => await client.DisconnectAsync());
    }

    #endregion dispose
}