using System;
using System.Threading;
using System.Threading.Tasks;
using Xtb.XApiClient.Model;

namespace Xtb.XApiClient;

/// <summary>
/// Remote receiver interface.
/// </summary>
public interface IReceiver
{
    /// <summary>
    /// Event raised when a message is received.
    /// </summary>
    event EventHandler<MessageEventArgs>? MessageReceived;

    /// <summary>
    /// Read a message from the remote endpoint.
    /// </summary>
    /// <returns>A message.</returns>
    string? ReadMessage();

    /// <summary>
    /// Read a message from the remote endpoint.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel operation.</param>
    /// <returns>A message.</returns>
    Task<string?> ReadMessageAsync(CancellationToken cancellationToken = default);
}