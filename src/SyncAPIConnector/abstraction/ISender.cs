using System;
using System.Threading;
using System.Threading.Tasks;

namespace Xtb.XApi;

/// <summary>
/// Remote sender interface.
/// </summary>
public interface ISender
{
    /// <summary>
    /// Event raised when a message is sent.
    /// </summary>
    event EventHandler<MessageEventArgs>? MessageSent;

    /// <summary>
    /// Send a message to the remote endpoint.
    /// </summary>
    /// <param name="message">Message to send.</param>
    void SendMessage(string message);

    /// <summary>
    /// Send a message to the remote endpoint.
    /// </summary>
    /// <param name="message">A message to send.</param>
    /// <param name="cancellationToken">Token to cancel operation.</param>
    Task SendMessageAsync(string message, CancellationToken cancellationToken = default);
}