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
    /// Lock object used to synchronize access to read/write socket operations.
    /// </summary>
    private readonly SemaphoreSlim _lock = new(1, 1);

    /// <summary>
    /// Creates new SyncAPIConnector instance based on given Server data.
    /// </summary>
    /// <param name="endpoint">Target endpoint</param>
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

    /// <summary>
    /// Stream session id (given upon login).
    /// </summary>
    public string? StreamSessionId { get; }

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

            CommandExecuting?.Invoke(this, new(command));
            var response = ExecuteCommand(request);

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
    /// <param name="message">Command to execute</param>
    /// <returns>Response from the server</returns>
    public string ExecuteCommand(string message)
    {
        _lock.Wait();
        try
        {
            long currentTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            long interval = currentTimestamp - _lastCommandTimestamp;

            // If interval between now and last command is less than minimum command time space - wait
            if (interval < COMMAND_TIME_SPACE)
            {
                Thread.Sleep((int)(COMMAND_TIME_SPACE - interval));
            }

            SendMessage(message);

            _lastCommandTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            string response = ReadMessage();

            if (string.IsNullOrEmpty(response))
            {
                Disconnect();
                throw new APICommunicationException("Server not responding. Response has no value.");
            }

            return response;
        }
        finally
        {
            _lock.Release();
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

            CommandExecuting?.Invoke(this, new(command));
            var response = await ExecuteCommandAsync(request, cancellationToken).ConfigureAwait(false);

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
    /// <param name="message">Command to execute</param>
    /// <param name="cancellationToken">Token to cancel operation.</param>
    /// <returns>Response from the server</returns>
    internal async Task<string> ExecuteCommandAsync(string message, CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            long currentTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            long interval = currentTimestamp - _lastCommandTimestamp;

            // If interval between now and last command is less than minimum command time space - wait
            if (interval < COMMAND_TIME_SPACE)
            {
                await Task.Delay((int)(COMMAND_TIME_SPACE - interval), cancellationToken);
            }

            await SendMessageAsync(message, cancellationToken).ConfigureAwait(false);

            _lastCommandTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            string response = await ReadMessageAsync(cancellationToken).ConfigureAwait(false);

            if (string.IsNullOrEmpty(response))
            {
                Disconnect();
                throw new APICommunicationException("Server not responding. Response has no value.");
            }

            return response;
        }
        finally
        {
            _lock.Release();
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
                _lock.Dispose();
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