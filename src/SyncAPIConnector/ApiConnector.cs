using System;
using System.Net;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Xtb.XApi.Commands;

namespace Xtb.XApi;

public class ApiConnector : IConnectable, IDisposable
{
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
        return new ApiConnector(new Connector(requestingEndpoint), new StreamingApiConnector(streamingEndpoint, streamingListener));
    }

    /// <summary>
    /// Helper method to create a new instance based on endpoints.
    /// </summary>
    /// <param name="requestingEndpoint">Endpoint for requesting data.</param>
    /// <param name="streamingEndpoint">Endpoint for streaming data.</param>
    /// <param name="streamingListener">Streaming listener.</param>
    public static ApiConnector Create(IPEndPoint requestingEndpoint, IPEndPoint streamingEndpoint, IStreamingListener? streamingListener = null)
    {
        return new ApiConnector(new Connector(requestingEndpoint), new StreamingApiConnector(streamingEndpoint, streamingListener));
    }

    /// <summary>
    /// Last command timestamp (used to calculate interval between each command).
    /// </summary>
    private long _lastCommandTimestamp;

    /// <summary>
    /// Creates new instance.
    /// </summary>
    /// <param name="connector">Underlaying client.</param>
    /// <param name="streamingConnector">streaming connector.</param>
    public ApiConnector(IClient connector, StreamingApiConnector streamingConnector)
    {
        Connector = connector;
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
    public event EventHandler<EndpointEventArgs>? Connected;
    public event EventHandler? Disconnected;

    #endregion Events

    /// <summary>
    /// Delay.between commands.
    /// </summary>
    public TimeSpan CommandDelay { get; set; } = TimeSpan.FromMilliseconds(200);

    /// <summary>
    /// Streaming connector.
    /// </summary>
    public IClient Connector { get; private set; }

    /// <inheritdoc/>
    public bool IsConnected => Connector.IsConnected;

    /// <inheritdoc/>
    public IPEndPoint Endpoint => Connector.Endpoint;

    /// <summary>
    /// Streaming connector.
    /// </summary>
    public StreamingApiConnector Streaming { get; private init; }

    /// <inheritdoc/>
    public void Connect()
    {
        Connector.Connect();
        //_streamingEndpoint = new IPEndPoint(endpoint.Address, _streamingEndpoint.Port);
    }

    /// <inheritdoc/>
    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        await Connector.ConnectAsync(cancellationToken).ConfigureAwait(false);
        //_streamingEndpoint = new IPEndPoint(endpoint.Address, _streamingEndpoint.Port);
    }


    /// <summary>
    /// Redirects to the given endpoint.
    /// </summary>
    /// <param name="endpoint">Endpoint to redirect to.</param>
    //public void Redirect(IPEndPoint endpoint)
    //{
    //    if (IsConnected)
    //        Connector.Disconnect();

    //    Connector.Connect();

    //    if (Streaming is not null)
    //    {
    //        Streaming.Endpoint = new IPEndPoint(endpoint.Address, Streaming.Endpoint.Port);
    //    }

    //    Redirected?.Invoke(this, new(endpoint));
    //}

    /// <summary>
    /// Redirects to the given endpoint.
    /// </summary>
    /// <param name="endpoint">Endpoint to redirect to.</param>
    /// <param name="cancellationToken">Token to cancel operation.</param>
    //public async Task RedirectAsync(IPEndPoint endpoint, CancellationToken cancellationToken = default)
    //{
    //    Redirected?.Invoke(this, new(endpoint));

    //    if (IsConnected)
    //        Disconnect();

    //    Endpoint = endpoint;
    //    await ConnectAsync(cancellationToken).ConfigureAwait(false);

    //    if (Streaming is not null)
    //    {
    //        Streaming.Endpoint = new IPEndPoint(endpoint.Address, Streaming.Endpoint.Port);
    //    }

    //    Redirected?.Invoke(this, new(endpoint));
    //}

    /// <summary>
    /// Executes given command and receives response (withholding API inter-command timeout).
    /// </summary>
    /// <param name="command">Command to execute</param>
    /// <returns>Response from the server</returns>
    public JsonObject ExecuteCommand(BaseCommand command)
    {
        try
        {
            var request = command.ToJsonString();

            long currentTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long interval = currentTimestamp - _lastCommandTimestamp;
            // If interval between now and last command is less than minimum command time space - wait
            if (interval < CommandDelay.TotalMilliseconds)
            {
                Thread.Sleep((int)(CommandDelay.TotalMilliseconds - interval));
            }

            CommandExecuting?.Invoke(this, new(command));
            var response = Connector.SendMessageWaitResponse(request);
            _lastCommandTimestamp = currentTimestamp;

            var parsedResponse = JsonNode.Parse(response)
                ?? throw new InvalidOperationException("Parsed command response is null.");
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
            var request = command.ToJsonString();

            long currentTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            long interval = currentTimestamp - _lastCommandTimestamp;
            // If interval between now and last command is less than minimum command time space - wait
            if (interval < CommandDelay.TotalMilliseconds)
            {
                await Task.Delay((int)(CommandDelay.TotalMilliseconds - interval), cancellationToken);
            }

            CommandExecuting?.Invoke(this, new(command));
            var response = await Connector.SendMessageWaitResponseAsync(request, cancellationToken).ConfigureAwait(false);
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

    /// <inheritdoc/>
    public void Disconnect()
    {
        Connector.Disconnect();
    }

    /// <inheritdoc/>
    public Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        return Connector.DisconnectAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public override string ToString() => $"{base.ToString()}";

    private bool _disposed;

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                if (Connector is IDisposable disposable)
                {
                    disposable.Dispose();
                }
                //Streaming?.Dispose();
            }

            _disposed = true;
        }
    }

    ~ApiConnector()
    {
        Dispose(false);
    }
}