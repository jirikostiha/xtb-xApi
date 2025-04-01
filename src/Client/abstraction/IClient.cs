using System.Threading;
using System.Threading.Tasks;

namespace Xtb.XApi.Client;

/// <summary>
/// Remote client interface for single endpoint.
/// </summary>
public interface IClient : IConnectable, ISender, IReceiver
{
    /// <summary>
    /// Send a message to the remote endpoint and wait for response.
    /// </summary>
    /// <param name="message">Message to send.</param>
    /// <returns>Response from the endpoint.</returns>
    string SendMessageWaitResponse(string message);

    /// <summary>
    /// Send a message to the remote endpoint.
    /// </summary>
    /// <param name="message">A message to send.</param>
    /// <param name="cancellationToken">Token to cancel operation.</param>
    /// <returns>Response from the endpoint.</returns>
    Task<string> SendMessageWaitResponseAsync(string message, CancellationToken cancellationToken = default);
}