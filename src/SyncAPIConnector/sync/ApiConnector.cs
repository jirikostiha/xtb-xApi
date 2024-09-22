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

public class ApiConnector : Connector, IClient
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
    /// Lock object used to synchronize access to read/write socket operations.
    /// </summary>
    private readonly SemaphoreSlim _lock = new(1, 1);

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
    /// Event raised when a connection is established.
    /// </summary>
    public event EventHandler<EndpointEventArgs>? Connected;

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
    /// Connects to the remote server.
    /// </summary>
    public void Connect()
    {
        if (Endpoint == null)
            throw new APICommunicationException("No endpoint to connect to.");

        var endpoint = Endpoint;
        TcpClient = new TcpClient();

        bool connectionAttempted = false;

        while (!connectionAttempted || !TcpClient.Connected)
        {
            // Try to connect asynchronously and wait for the result
            IAsyncResult result = TcpClient.BeginConnect(endpoint.Address, endpoint.Port, null, null);
            connectionAttempted = result.AsyncWaitHandle.WaitOne(ConnectionTimeout, true);

            // If connection attempt failed (timeout) or not connected
            if (!connectionAttempted || !TcpClient.Connected)
            {
                TcpClient.Close();
                throw new APICommunicationException($"Cannot connect to:{endpoint.Address}:{endpoint.Port}");
            }
        }

        if (ShallUseSecureConnection)
        {
            EstablishSecureConnection();
        }
        else
        {
            NetworkStream ns = TcpClient.GetStream();
            StreamWriter = new StreamWriter(ns);
            StreamReader = new StreamReader(ns);
        }

        IsConnected = true;

        Connected?.Invoke(this, new(endpoint));

        //Streaming = new StreamingApiConnector(_streamingEndpoint, _streamingListener);
    }

    /// <summary>
    /// Connects to the remote server.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel operation.</param>
    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        if (Endpoint == null)
            throw new APICommunicationException("No server to connect to.");

        var endpoint = Endpoint;
        TcpClient = new TcpClient
        {
            ReceiveTimeout = ConnectionTimeout.Milliseconds,
            SendTimeout = ConnectionTimeout.Milliseconds
        };

        bool connectionAttempted = false;

        while (!connectionAttempted || !TcpClient.Connected)
        {
            try
            {
                // Try to connect asynchronously and wait for the result
                var connectTask = TcpClient.ConnectAsync(endpoint.Address, endpoint.Port);
                var timeoutTask = Task.Delay(ConnectionTimeout, cancellationToken);

                var completedTask = await Task.WhenAny(connectTask, timeoutTask);

                connectionAttempted = completedTask == connectTask && TcpClient.Connected;

                if (cancellationToken.IsCancellationRequested)
                {
                    TcpClient.Close();
                    throw new OperationCanceledException(cancellationToken);
                }

                if (!connectionAttempted || !TcpClient.Connected)
                {
                    TcpClient.Close();
                    throw new APICommunicationException($"Cannot connect to:{endpoint.Address}:{endpoint.Port}");
                }
            }
            catch
            {
                TcpClient.Close();
                throw new APICommunicationException($"Cannot connect to:{endpoint.Address}:{endpoint.Port}");
            }
        }

        if (ShallUseSecureConnection)
        {
            await EstablishSecureConnectionAsync(cancellationToken);
        }
        else
        {
            NetworkStream ns = TcpClient.GetStream();
            StreamWriter = new StreamWriter(ns);
            StreamReader = new StreamReader(ns);
        }

        IsConnected = true;

        Connected?.Invoke(this, new(endpoint));

        //Streaming = new StreamingApiConnector(_streamingEndpoint, _streamingListener);
    }

    /// <summary>
    /// Redirects to the given server.
    /// </summary>
    /// <param name="endpoint">Endpoint to redirect to.</param>
    public void Redirect(IPEndPoint endpoint)
    {
        Redirected?.Invoke(this, new(endpoint));

        if (IsConnected)
            Disconnect(true);

        Endpoint = endpoint;
        Streaming.Endpoint = new IPEndPoint(endpoint.Address, Streaming.Endpoint.Port);
        Connect();
    }

    /// <summary>
    /// Redirects to the given server.
    /// </summary>
    /// <param name="endpoint">Endpoint to redirect to.</param>
    /// <param name="cancellationToken">Token to cancel operation.</param>
    public async Task RedirectAsync(IPEndPoint endpoint, CancellationToken cancellationToken = default)
    {
        Redirected?.Invoke(this, new(endpoint));

        if (IsConnected)
            Disconnect(true);

        Endpoint = endpoint;
        Streaming.Endpoint = new IPEndPoint(endpoint.Address, Streaming.Endpoint.Port);
        await ConnectAsync(cancellationToken).ConfigureAwait(false);
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