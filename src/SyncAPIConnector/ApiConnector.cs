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

        return Create(requestingEndpoint, streamingEndpoint);
    }

    /// <summary>
    /// Helper method to create a new instance based on endpoints.
    /// </summary>
    /// <param name="requestingEndpoint">Endpoint for requesting data.</param>
    /// <param name="streamingEndpoint">Endpoint for streaming data.</param>
    /// <param name="streamingListener">Streaming listener.</param>
    public static ApiConnector Create(IPEndPoint requestingEndpoint, IPEndPoint streamingEndpoint, IStreamingListener? streamingListener = null)
    {
        var requestingConnector = new Connector(requestingEndpoint);
        var streamingApiConnector = StreamingApiConnector.Create(streamingEndpoint, streamingListener);

        return new ApiConnector(requestingConnector, streamingApiConnector)
        {
            IsConnectorOwner = true,
            IsStreamingApiConnectorOwner = true
        };
    }

    /// <summary>
    /// Last command timestamp (used to calculate interval between each command).
    /// </summary>
    private long _lastCommandTimestamp;

    /// <summary>
    /// Creates new instance.
    /// </summary>
    /// <param name="requestingConnector">Underlaying connector for requests.</param>
    /// <param name="streamingConnector">streaming connector.</param>
    public ApiConnector(IClient requestingConnector, IClient streamingConnector)
        : this(requestingConnector, new StreamingApiConnector(streamingConnector))
    {
        IsStreamingApiConnectorOwner = true;
    }

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

    /// <inheritdoc/>
    public event EventHandler<EndpointEventArgs>? Connected
    {
        add => Connector.Connected += value;
        remove => Connector.Connected -= value;
    }

    /// <summary>
    /// Event raised when a command is being executed.
    /// </summary>
    public event EventHandler<CommandEventArgs>? CommandExecuting;

    /// <inheritdoc/>
    public event EventHandler? Disconnected
    {
        add => Connector.Disconnected += value;
        remove => Connector.Disconnected -= value;
    }

    #endregion Events

    /// <summary>
    /// Delay.between commands.
    /// </summary>
    public TimeSpan CommandDelay { get; set; } = TimeSpan.FromMilliseconds(200);

    /// <summary>
    /// Streaming connector.
    /// </summary>
    public IClient Connector { get; private set; }

    /// <summary>
    /// Indicates whether the connector is owned.
    /// </summary>
    internal bool IsConnectorOwner { get; init; }

    /// <inheritdoc/>
    public bool IsConnected => Connector.IsConnected;

    /// <inheritdoc/>
    public IPEndPoint Endpoint => Connector.Endpoint;

    /// <summary>
    /// Streaming connector.
    /// </summary>
    public StreamingApiConnector Streaming { get; private init; }

    /// <summary>
    /// Indicates whether the streaming api connector is owned.
    /// </summary>
    internal bool IsStreamingApiConnectorOwner { get; init; }

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
    /// Executes given command and receives response (withholding API inter-command timeout).
    /// </summary>
    /// <param name="command">Command to execute</param>
    /// <returns>Response from the server</returns>
    public JsonObject ExecuteCommand(BaseCommand command)
    {
        try
        {
            var request = command.ToJsonString();

            EnforceCommandDelay();

            CommandExecuting?.Invoke(this, new(command));
            var response = Connector.SendMessageWaitResponse(request);
            _lastCommandTimestamp = DateTimeOffset.Now.Ticks / TimeSpan.TicksPerMillisecond;

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

    private void EnforceCommandDelay()
    {
        long currentTimestamp = DateTimeOffset.Now.Ticks / TimeSpan.TicksPerMillisecond;
        long interval = currentTimestamp - _lastCommandTimestamp;

        if (interval < CommandDelay.TotalMilliseconds)
        {
            Thread.Sleep((int)(CommandDelay.TotalMilliseconds - interval));
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

            await EnforceCommandDelayAsync();

            CommandExecuting?.Invoke(this, new(command));
            var response = await Connector.SendMessageWaitResponseAsync(request, cancellationToken).ConfigureAwait(false);
            _lastCommandTimestamp = DateTimeOffset.Now.Ticks / TimeSpan.TicksPerMillisecond;

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

    private async Task EnforceCommandDelayAsync()
    {
        long currentTimestamp = DateTimeOffset.Now.Ticks / TimeSpan.TicksPerMillisecond;
        long interval = currentTimestamp - _lastCommandTimestamp;

        if (interval < CommandDelay.TotalMilliseconds)
        {
            await Task.Delay((int)(CommandDelay.TotalMilliseconds - interval));
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
                if (IsConnectorOwner && Connector is IDisposable disposableConnetor)
                {
                    disposableConnetor.Dispose();
                }

                if (IsStreamingApiConnectorOwner && Streaming is IDisposable disposableStreming)
                {
                    disposableStreming.Dispose();
                }

                _disposed = true;
            }
        }
    }

    ~ApiConnector()
    {
        Dispose(false);
    }
}