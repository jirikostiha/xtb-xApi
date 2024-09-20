using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Xtb.XApi;

/// <summary>
/// Remote connector interface.
/// </summary>
public interface IConnectable
{
    /// <summary>
    /// Event raised when the client connects to the server.
    /// </summary>
    event EventHandler<EndpointEventArgs>? Connected;

    /// <summary>
    /// Event raised when the client disconnects from the server.
    /// </summary>
    event EventHandler? Disconnected;

    /// <summary>
    /// Connection endpoint.
    /// </summary>
    public IPEndPoint Endpoint { get; }

    /// <summary>
    /// Indicates whether the client is connected to the server.
    /// </summary>
    /// <returns>True if connected, otherwise false</returns>
    bool IsConnected { get; }

    /// <summary>
    /// Connect client to the remote server.
    /// </summary>
    void Connect();

    /// <summary>
    /// Connect client async to the remote server.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel operation.</param>
    Task ConnectAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Disconnects client from the remote server.
    /// </summary>
    void Disconnect();
}