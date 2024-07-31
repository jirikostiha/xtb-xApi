using System;
using System.Net;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using xAPI.Commands;
using xAPI.Errors;

namespace xAPI.Sync;

public class ApiConnector : Connector
{
    /// <summary>
    /// Delay between each command to the server.
    /// </summary>
    private const int COMMAND_TIME_SPACE = 200;

    /// <summary>
    /// Last command timestamp (used to calculate interval between each command).
    /// </summary>
    private long _lastCommandTimestamp;

    /// <summary>
    /// Creates new SyncAPIConnector instance based on given Server data.
    /// </summary>
    /// <param name="endpoint">Target endpoint</param>
    /// <param name="streamingEndpoint">Streaming endpoint</param>
    public ApiConnector(IPEndPoint endpoint, IPEndPoint streamingEndpoint)
        : base(endpoint)
    {
        StreamingEndpoint = streamingEndpoint;
    }

    #region Events
    /// <summary>
    /// Event raised when a command is being executed.
    /// </summary>
    public event EventHandler<CommandEventArgs>? CommandExecuting;
    #endregion Events

    /// <summary>
    /// Streaming connector.
    /// </summary>
    public StreamingApiConnector? Streaming { get; private set; }

    /// <summary>
    /// Streaming endpoint.
    /// </summary>
    public IPEndPoint StreamingEndpoint { get; private set; }

    /// <inheritdoc/>
    public override void Connect()
    {
        base.Connect();

        Streaming = new StreamingApiConnector(StreamingEndpoint);
    }

    /// <inheritdoc/>
    public override async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        await base.ConnectAsync(cancellationToken).ConfigureAwait(false);

        Streaming = new StreamingApiConnector(StreamingEndpoint);
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