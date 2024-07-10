using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using xAPI.Commands;
using xAPI.Errors;
using xAPI.Utils;

namespace xAPI.Sync;

public class SyncAPIConnector : Connector
{
    #region Settings

    /// <summary>
    /// Wrappers version.
    /// </summary>
    public const string VERSION = "2.5.0";

    /// <summary>
    /// Delay between each command to the server.
    /// </summary>
		private const long COMMAND_TIME_SPACE = 200;

    /// <summary>
    /// Maximum number of redirects (to avoid redirection loops).
    /// </summary>
    public const long MAX_REDIRECTS = 3;

    /// <summary>
    /// Default maximum connection time (in milliseconds). After that the connection attempt is immediately dropped.
    /// </summary>
    private const int TIMEOUT = 5000;

    #endregion Settings

    #region Events

    /// <summary>
    /// Event raised when a connection is established.
    /// </summary>
    public event EventHandler<ServerEventArgs>? Connected;

    /// <summary>
    /// Event raised when a connection is redirected.
    /// </summary>
    public event EventHandler<ServerEventArgs>? Redirected;

    /// <summary>
    /// Event raised when a command is being executed.
    /// </summary>
    public event EventHandler<CommandEventArgs>? CommandExecuting;

    #endregion Events

    /// <summary>
    /// Streaming API connector.
    /// </summary>
    private StreamingAPIConnector _streamingConnector;

    /// <summary>
    /// Last command timestamp (used to calculate interval between each command).
    /// </summary>
    private long _lastCommandTimestamp;

    /// <summary>
    /// Lock object used to synchronize access to read/write socket operations.
    /// </summary>
    private readonly SemaphoreSlim _lock = new(1, 1);

    /// <summary>
    /// Maximum connection time (in milliseconds). After that the connection attempt is immediately dropped.
    /// </summary>
    private readonly int _connectionTimeout;

    /// <summary>
    /// Creates new SyncAPIConnector instance based on given Server data.
    /// </summary>
    /// <param name="server">Server data</param>
    /// <param name="connectionTimeout">Connection timeout</param>
    public SyncAPIConnector(Server server, int connectionTimeout = TIMEOUT)
    {
        Server = server;
        _connectionTimeout = connectionTimeout;
    }

    /// <summary>
    /// Connects to the remote server.
    /// </summary>
    /// <param name="server">Server data</param>
    /// <param name="lookForBackups">If false, no connection to backup servers will be made</param>
    private void Connect(Server server, bool lookForBackups = true)
    {
        Server = server;
        ApiSocket = new TcpClient();

        bool connectionAttempted = false;

        while (!connectionAttempted || !ApiSocket.Connected)
        {
            // Try to connect asynchronously and wait for the result
            IAsyncResult result = ApiSocket.BeginConnect(Server.Address, Server.MainPort, null, null);
            connectionAttempted = result.AsyncWaitHandle.WaitOne(_connectionTimeout, true);

            // If connection attempt failed (timeout) or not connected
            if (!connectionAttempted || !ApiSocket.Connected)
            {
                ApiSocket.Close();
                if (lookForBackups)
                {
                    Server = Servers.GetBackup(Server);
                    if (Server == null)
                    {
                        throw new APICommunicationException("Connection timeout.");
                    }
                    ApiSocket = new TcpClient();
                }
                else
                {
                    throw new APICommunicationException($"Cannot connect to:{server.Address}:{server.MainPort}");
                }
            }
        }

        if (server.IsSecure)
        {
            SslStream sl = new SslStream(ApiSocket.GetStream(), false, new RemoteCertificateValidationCallback(SSLHelper.TrustAllCertificatesCallback));

            //sl.AuthenticateAsClient(server.Address);

            bool authenticated = ExecuteWithTimeLimit.Execute(TimeSpan.FromMilliseconds(5000), () =>
            {
                sl.AuthenticateAsClient(server.Address, new X509CertificateCollection(), System.Security.Authentication.SslProtocols.None, false);
            });

            if (!authenticated)
                throw new APICommunicationException("Error during SSL handshaking (timed out?).");

            ApiWriteStream = new StreamWriter(sl);
            ApiReadStream = new StreamReader(sl);
        }
        else
        {
            NetworkStream ns = ApiSocket.GetStream();
            ApiWriteStream = new StreamWriter(ns);
            ApiReadStream = new StreamReader(ns);
        }

        apiConnected = true;

        Connected?.Invoke(this, new(server));

        _streamingConnector = new StreamingAPIConnector(Server);
    }

    /// <summary>
    /// Connects to the remote server (NOTE: server must be already set).
    /// </summary>
    public void Connect()
    {
        if (Server != null)
        {
            Connect(Server);
        }
        else
        {
            throw new APICommunicationException("No server to connect to.");
        }
    }

    /// <summary>
    /// Redirects to the given server.
    /// </summary>
    /// <param name="server">Server data</param>
    public void Redirect(Server server)
    {
        Redirected?.Invoke(this, new(server));

        if (apiConnected)
            Disconnect(true);

        Connect(server);
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

            var parsedResponse = JsonNode.Parse(response).AsObject();

            return parsedResponse;
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

            CommandExecuting?.Invoke(this, new(command));
            var response = await ExecuteCommandAsync(request, cancellationToken).ConfigureAwait(false);

            var parsedResponse = JsonNode.Parse(response).AsObject();

            return parsedResponse;
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

            WriteMessage(message);

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

            await WriteMessageAsync(message, cancellationToken).ConfigureAwait(false);

            _lastCommandTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            string response = await ReadMessageAsync().ConfigureAwait(false);

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
    /// Streaming connector.
    /// </summary>
    public StreamingAPIConnector Streaming
    {
        get { return _streamingConnector; }
    }

    /// <summary>
    /// Stream session id (given upon login).
    /// </summary>
    public string StreamSessionId
    {
        get; set;
    }

    private bool _disposed;

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _streamingConnector?.Dispose();
                _lock.Dispose();
            }

            base.Dispose(disposing);

            _disposed = true;
        }
    }

    ~SyncAPIConnector()
    {
        Dispose(false);
    }
}