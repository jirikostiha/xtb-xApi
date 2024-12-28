using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xtb.XApiClient.Model;

namespace Xtb.XApiClient;

/// <summary>
/// Remote connector interface.
/// </summary>
public interface IConnectable
{
    /// <summary>
    /// Event raised when the client connects to the endpoint.
    /// </summary>
    event EventHandler<EndpointEventArgs>? Connected;

    /// <summary>
    /// Event raised when the client disconnects from the endpoint.
    /// </summary>
    event EventHandler? Disconnected;

    /// <summary>
    /// Endpoint that the connection was established with.
    /// </summary>
    IPEndPoint Endpoint { get; }

    /// <summary>
    /// Indicates whether the client is connected to the endpoint.
    /// </summary>
    /// <returns>True if connected, otherwise false</returns>
    bool IsConnected { get; }

    /// <summary>
    /// Connect client to the remote endpoint.
    /// </summary>
    void Connect();

    /// <summary>
    /// Connect client async to the remote server.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel operation.</param>
    Task ConnectAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Disconnects client from the remote endpoint.
    /// </summary>
    void Disconnect();

    /// <summary>
    /// Disconnects client async from the remote endpoint.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel operation.</param>
    Task DisconnectAsync(CancellationToken cancellationToken = default);
}