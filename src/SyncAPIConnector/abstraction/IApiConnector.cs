using System;
using System.Threading;
using System.Threading.Tasks;

namespace xAPI.Sync
{
    public interface IConnector
    {
        event EventHandler<ServerEventArgs>? Connected;

        event EventHandler? Disconnected;

        bool IsConnected { get; }

        void Connect();

        Task ConnectAsync(CancellationToken cancellationToken = default);

        void Disconnect();
    }

    public interface ISenderReceiver
    {
        event EventHandler<MessageEventArgs>? MessageSent;

        event EventHandler<MessageEventArgs>? MessageReceived;

        void SendMessage(string message);

        Task SendMessageAsync(string message, CancellationToken cancellationToken = default);

        string? ReadResponse(string message);

        Task<string?> ReadResponseAsync(string message, CancellationToken cancellationToken = default);
    }

    public interface IClient : IConnector, ISenderReceiver
    {
    }
}