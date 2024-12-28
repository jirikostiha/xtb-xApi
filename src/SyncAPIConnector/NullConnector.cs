using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xtb.XApi.Model;

namespace Xtb.XApi;

public class NullConnector : IClient
{
    public IPEndPoint Endpoint => throw new NotImplementedException();

    public bool IsConnected => throw new NotImplementedException();

    public event EventHandler<EndpointEventArgs>? Connected;
    public event EventHandler? Disconnected;
    public event EventHandler<MessageEventArgs>? MessageSent;
    public event EventHandler<MessageEventArgs>? MessageReceived;

    public void Connect()
    {
    }

    public Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public void Disconnect()
    {
    }

    public Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public string? ReadMessage()
    {
        return string.Empty;
    }

    public Task<string?> ReadMessageAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<string?>(string.Empty);
    }

    public void SendMessage(string message)
    {
    }

    public Task SendMessageAsync(string message, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }

    public string SendMessageWaitResponse(string message)
    {
        return string.Empty;
    }

    public Task<string> SendMessageWaitResponseAsync(string message, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(string.Empty);
    }
}