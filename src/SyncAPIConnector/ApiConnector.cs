using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Xtb.XApi.Commands;
using Xtb.XApi.Utils;

namespace Xtb.XApi;

public class ApiConnector : Connector
{
    /// <summary>
    /// Delay between each command to the server.
    /// </summary>
    private const int COMMAND_TIME_SPACE = 200;

    /// <summary>
    /// Helper method to create a new instance based on address and ports.
    /// </summary>
    /// <param name="address">Endpoint address.</param>
    /// <param name="requestingPort">Port for requesting data.</param>
    /// <param name="streamingPort">Port for streaming data.</param>
    /// <param name="streamingListener">Streaming listener.</param>
    public static ApiConnector Create(string address, int requestingPort, int streamingPort, IStreamingListener? streamingListener = null)
    {
        var requestingEndpoint = new IPEndPoint(IPAddress.Parse(address), requestingPort);
        var streamingEndpoint = new IPEndPoint(IPAddress.Parse(address), streamingPort);
        return new ApiConnector(requestingEndpoint, new StreamingApiConnector(streamingEndpoint, streamingListener));
    }

    /// <summary>
    /// Helper method to create a new instance based on endpoints.
    /// </summary>
    /// <param name="requestingEndpoint">Endpoint for requesting data.</param>
    /// <param name="streamingEndpoint">Endpoint for streaming data.</param>
    /// <param name="streamingListener">Streaming listener.</param>
    public static ApiConnector Create(IPEndPoint requestingEndpoint, IPEndPoint streamingEndpoint, IStreamingListener? streamingListener = null)
    {
        return new ApiConnector(requestingEndpoint, new StreamingApiConnector(streamingEndpoint, streamingListener));
    }

    /// <summary>
    /// Last command timestamp (used to calculate interval between each command).
    /// </summary>
    private long _lastCommandTimestamp;

    /// <summary>
    /// Creates new instance.
    /// </summary>
    /// <param name="endpoint">Endpoint for requesting data.</param>
    /// <param name="streamingConnector">streaming connector.</param>
    public ApiConnector(IPEndPoint endpoint, StreamingApiConnector streamingConnector)
        : base(endpoint)
    {
        Streaming = streamingConnector;
    }

    #region Events

    /// <summary>
    /// Event raised when a connection is redirected.
    /// </summary>
    public event EventHandler<EndpointEventArgs>? Redirected;

    /// <summary>
    /// Event raised when a command is being executed.
    /// </summary>
    public event EventHandler<CommandEventArgs>? CommandExecuting;

    #endregion Events

    /// <summary>
    /// Streaming connector.
    /// </summary>
    public StreamingApiConnector Streaming { get; private init; }

    /// <summary>
    /// Stream session id (given upon login).
    /// </summary>
    public string? StreamSessionId { get; }

    /// <summary>
    /// Redirects to the given endpoint.
    /// </summary>
    /// <param name="endpoint">Endpoint to redirect to.</param>
    public void Redirect(IPEndPoint endpoint)
    {
        if (IsConnected)
            Disconnect();

        Endpoint = endpoint;
        Connect();

        if (Streaming is not null)
        {
            Streaming.Endpoint = new IPEndPoint(endpoint.Address, Streaming.Endpoint.Port);
        }

        Redirected?.Invoke(this, new(endpoint));
    }

    /// <summary>
    /// Redirects to the given endpoint.
    /// </summary>
    /// <param name="endpoint">Endpoint to redirect to.</param>
    /// <param name="cancellationToken">Token to cancel operation.</param>
    public async Task RedirectAsync(IPEndPoint endpoint, CancellationToken cancellationToken = default)
    {
        Redirected?.Invoke(this, new(endpoint));

        if (IsConnected)
            Disconnect();

        Endpoint = endpoint;
        await ConnectAsync(cancellationToken).ConfigureAwait(false);

        if (Streaming is not null)
        {
            Streaming.Endpoint = new IPEndPoint(endpoint.Address, Streaming.Endpoint.Port);
        }

        Redirected?.Invoke(this, new(endpoint));
    }

    /// <summary>
    /// Executes given command and receives response (withholding API inter-command timeout).
    /// </summary>
    /// <param name="command">Command to execute</param>
    /// <returns>Response from the server</returns>
    public JsonObject ExecuteCommand(BaseCommand command)
    {
        try
        {
            var request = command.ToJSONString();

            long currentTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long interval = currentTimestamp - _lastCommandTimestamp;
            // If interval between now and last command is less than minimum command time space - wait
            if (interval < COMMAND_TIME_SPACE)
            {
                Thread.Sleep((int)(COMMAND_TIME_SPACE - interval));
            }

            CommandExecuting?.Invoke(this, new(command));
            var response = SendMessageWaitResponse(request);
            _lastCommandTimestamp = currentTimestamp;

            var parsedResponse = JsonNode.Parse(response);
            if (parsedResponse is null)
                throw new InvalidOperationException("Parsed command response is null.");

            var jsonObj = parsedResponse.AsObject();

            return jsonObj;
        }
        catch (Exception ex)
        {
            throw new APICommunicationException($"Problem with executing command:'{command.CommandName}'", ex);
        }
    }

    /// <summary>
    /// Executes given command and receives response (withholding API inter-command timeout).
    /// </summary>
    /// <param name="command">Command to execute</param>
    /// <param name="cancellationToken">Token to cancel operation.</param>
    /// <returns>Response from the server</returns>
    public async Task<JsonObject> ExecuteCommandAsync(BaseCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = command.ToJSONString();

            long currentTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long interval = currentTimestamp - _lastCommandTimestamp;
            // If interval between now and last command is less than minimum command time space - wait
            if (interval < COMMAND_TIME_SPACE)
            {
                await Task.Delay((int)(COMMAND_TIME_SPACE - interval), cancellationToken);
            }

            CommandExecuting?.Invoke(this, new(command));
            var response = await SendMessageWaitResponseAsync(request, cancellationToken).ConfigureAwait(false);
            _lastCommandTimestamp = currentTimestamp;

            var parsedResponse = JsonNode.Parse(response);
            if (parsedResponse is null)
                throw new InvalidOperationException("Parsed command response is null.");

            var jsonObj = parsedResponse.AsObject();

            return jsonObj;
        }
        catch (Exception ex)
        {
            throw new APICommunicationException($"Problem with executing command:'{command.CommandName}'", ex);
        }
    }

    private bool _disposed;

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                Streaming?.Dispose();
            }

            base.Dispose(disposing);

            _disposed = true;
        }
    }

    ~ApiConnector()
    {
        Dispose(false);
    }
}