using System;
using System.Net;
using System.Net;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using xAPI.Commands;

namespace XApi;

public class ApiConnector : IConnectable
{
    /// <summary>
    /// Delay between each command to the server.
    /// </summary>
    private const int COMMAND_TIME_SPACE = 200;

    #endregion Settings

    /// <summary>
    /// Last command timestamp (used to calculate interval between each command).
    /// </summary>
    private long _lastCommandTimestamp;

    /// <summary>
    /// Creates new instance.
    /// </summary>
    /// <param name="address">Endpoint address.</param>
    /// <param name="requestingPort">Port for requesting data.</param>
    /// <param name="streamingPort">Port for streaming data.</param>
    /// /// <param name="streamingListener">Streaming listener.</param>
    public ApiConnector(string address, int requestingPort, int streamingPort, IStreamingListener? streamingListener = null)
        : this(
              new Connector(new IPEndPoint(IPAddress.Parse(address), requestingPort)),
              new StreamingApiConnector(new IPEndPoint(IPAddress.Parse(address), streamingPort), streamingListener))
    {
    }

    /// <summary>
    /// Creates new instance.
    /// </summary>
    /// <param name="endpoint">Endpoint for requesting data.</param>
    /// <param name="streamingEndpoint">Endpoint for streaming data.</param>
    /// <param name="streamingListener">Streaming listener.</param>
    public ApiConnector(IPEndPoint endpoint, IPEndPoint streamingEndpoint, IStreamingListener? streamingListener = null)
        : this(new Connector(endpoint), new StreamingApiConnector(streamingEndpoint, streamingListener))
    {
    }
    /// <param name="address">Endpoint address.</param>
    /// <param name="requestingPort">Port for requesting data.</param>
    /// <param name="streamingPort">Port for streaming data.</param>
    /// <param name="streamingListener">Streaming listener.</param>
    public ApiConnector(string address, int requestingPort, int streamingPort, IStreamingListener? streamingListener = null)
        : this(new IPEndPoint(IPAddress.Parse(address), requestingPort), new IPEndPoint(IPAddress.Parse(address), streamingPort))
    {
    }

    /// <summary>
    /// Creates new instance.
    /// </summary>
    /// <param name="connector">Underlaying client.</param>
    /// <param name="streamingConnector">Streaming client.</param>
    public ApiConnector(IClient connector, StreamingApiConnector streamingConnector)
    public ApiConnector(IPEndPoint endpoint, IPEndPoint streamingEndpoint, IStreamingListener? streamingListener = null)
        : base(endpoint)
    {
        Client = connector;
        Streaming = streamingConnector;
    }

    #region Events
    /// <inheritdoc/>
    public event EventHandler<EndpointEventArgs>? Connected
    {
        add => Client.Connected += value;
        remove => Client.Connected -= value;
    }

    /// <summary>
    /// Event raised when a command is being executed.
    /// </summary>
    public event EventHandler<CommandEventArgs>? CommandExecuting;

    /// <inheritdoc/>
    public event EventHandler? Disconnected
    {
        add => Client.Disconnected += value;
        remove => Client.Disconnected -= value;
    }
    #endregion Events

    /// <summary>
    /// Streaming connector.
    /// </summary>
    public IClient Client { get; private set; }

    /// <summary>
    /// Streaming connector.
    /// </summary>
    public StreamingApiConnector? Streaming { get; private set; }

    /// <inheritdoc/>
    public bool IsConnected => Client.IsConnected;

    /// <inheritdoc/>
    public IPEndPoint Endpoint => Client.Endpoint;

    /// <inheritdoc/>
    public void Connect()
    {
        Client.Connect();
    }

    /// <inheritdoc/>
    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        await Client.ConnectAsync(cancellationToken).ConfigureAwait(false);
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
            var response = Client.SendMessageWaitResponse(request);
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
            var response = await Client.SendMessageWaitResponseAsync(request, cancellationToken).ConfigureAwait(false);
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

    public void Disconnect() => Client.Disconnect();
}