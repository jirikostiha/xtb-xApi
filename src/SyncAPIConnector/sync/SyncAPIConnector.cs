using System;
using System.Threading;
using System.Net.Sockets;
using System.Net.Security;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using xAPI.Errors;
using xAPI.Commands;
using xAPI.Utils;
using SyncAPIConnect.Utils;
using System.Threading.Tasks;
using System.Text.Json.Nodes;

namespace xAPI.Sync
{
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
        #endregion

        #region Events
        /// <summary>
        /// Delegate called on connection establish.
        /// </summary>
        /// <param name="server">Server that the connection was made to</param>
        public delegate void OnConnectedCallback(Server server);

        /// <summary>
        /// Event raised when connection is established.
        /// </summary>
        public event OnConnectedCallback OnConnected;

        /// <summary>
        /// Delegate called on connection redirect.
        /// </summary>
        /// <param name="server">Server that the connection was made to</param>
        public delegate void OnRedirectedCallback(Server server);

        /// <summary>
        /// Event raised when connection is redirected.
        /// </summary>
        public event OnRedirectedCallback OnRedirected;
        #endregion

        /// <summary>
        /// Streaming API connector.
        /// </summary>
        private StreamingAPIConnector streamingConnector;

        /// <summary>
        /// Last command timestamp (used to calculate interval between each command).
        /// </summary>
        private long lastCommandTimestamp;

        /// <summary>
        /// Lock object used to synchronize access to read/write socket operations.
        /// </summary>
        //private Object locker = new Object();
        private readonly SemaphoreSlim locker = new SemaphoreSlim(1, 1);

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
            _connectionTimeout = connectionTimeout;
        }

        /// <summary>
        /// Connects to the remote server.
        /// </summary>
        /// <param name="server">Server data</param>
        /// <param name="lookForBackups">If false, no connection to backup servers will be made</param>
        private void Connect(Server server, bool lookForBackups = true)
        {
            this.server = server;
            this.apiSocket = new TcpClient();

            bool connectionAttempted = false;

            while (!connectionAttempted || !apiSocket.Connected)
            {
                // Try to connect asynchronously and wait for the result
                IAsyncResult result = apiSocket.BeginConnect(this.server.Address, this.server.MainPort, null, null);
                connectionAttempted = result.AsyncWaitHandle.WaitOne(_connectionTimeout, true);

                // If connection attempt failed (timeout) or not connected
                if (!connectionAttempted || !apiSocket.Connected)
                {
                    apiSocket.Close();
                    if (lookForBackups)
                    {
                        this.server = Servers.GetBackup(this.server);
                        if (this.server == null)
                        {
                            throw new APICommunicationException("Connection timeout.");
                        }
                        apiSocket = new System.Net.Sockets.TcpClient();
                    }
                    else
                    {
                        throw new APICommunicationException($"Cannot connect to:{server.Address}:{server.MainPort}");
                    }
                }
            }

            if (server.Secure)
            {
                SslStream sl = new SslStream(apiSocket.GetStream(), false, new RemoteCertificateValidationCallback(SSLHelper.TrustAllCertificatesCallback));

                //sl.AuthenticateAsClient(server.Address);

                bool authenticated = ExecuteWithTimeLimit.Execute(TimeSpan.FromMilliseconds(5000), () =>
                {
                    sl.AuthenticateAsClient(server.Address, new X509CertificateCollection(), System.Security.Authentication.SslProtocols.None, false);
                });

                if (!authenticated)
                    throw new APICommunicationException("Error during SSL handshaking (timed out?).");

                apiWriteStream = new StreamWriter(sl);
                apiReadStream = new StreamReader(sl);
            }
            else
            {
                NetworkStream ns = apiSocket.GetStream();
                apiWriteStream = new StreamWriter(ns);
                apiReadStream = new StreamReader(ns);
            }

            this.apiConnected = true;

            if (OnConnected != null)
                OnConnected.Invoke(this.server);

            this.streamingConnector = new StreamingAPIConnector(this.server);
            this.streamingConnector.Connect();
        }

        /// <summary>
        /// Connects to the remote server (NOTE: server must be already set).
        /// </summary>
        public void Connect()
        {
            if (this.server != null)
                Connect(this.server);

            throw new APICommunicationException("No server to connect to.");
        }

        /// <summary>
        /// Creates new SyncAPIConnector instance based on given data.
        /// </summary>
        /// <param name="address">Address</param>
        /// <param name="port">Main port</param>
        /// <param name="secure">SSL enabled</param>
        [Obsolete("Use SyncAPIConnector(Server server) instead")]
        private SyncAPIConnector(string address, int port, bool secure) : this(new Server(address, port, port + 1, secure, ""))
        {
        }

        /// <summary>
        /// Redirects to the given server.
        /// </summary>
        /// <param name="server">Server data</param>
        public void Redirect(Server server)
        {
            if (OnRedirected != null)
                OnRedirected.Invoke(server);

            if (this.apiConnected)
                Disconnect(true);

            Connect(server);
        }

        /// <summary>
        /// Executes given command and receives response (withholding API inter-command timeout).
        /// </summary>
        /// <param name="cmd">Command to execute</param>
        /// <returns>Response from the server</returns>
		public JsonObject ExecuteCommand(BaseCommand cmd)
        {
            try
            {
                return (JsonObject)JsonObject.Parse(this.ExecuteCommand(cmd.ToJSONString()));
            }
            catch (Exception ex)
            {
                throw new APICommunicationException($"Problem with executing command:'{cmd.CommandName}'", ex);
            }
        }

        /// <summary>
        /// Executes given command and receives response (withholding API inter-command timeout).
        /// </summary>
        /// <param name="cmd">Command to execute</param>
        /// <returns>Response from the server</returns>
		public async Task<JsonObject> ExecuteCommandAsync(BaseCommand cmd)
        {
            try
            {
                return (JsonObject)JsonObject.Parse(await this.ExecuteCommandAsync(cmd.ToJSONString()).ConfigureAwait(false));
            }
            catch (Exception ex)
            {
                throw new APICommunicationException($"Problem with executing command:'{cmd.CommandName}'", ex);
            }
        }

        /// <summary>
        /// Executes given command and receives response (withholding API inter-command timeout).
        /// </summary>
        /// <param name="message">Command to execute</param>
        /// <returns>Response from the server</returns>
        public string ExecuteCommand(string message)
        {
            locker.Wait();
            try
            {
                long currentTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                long interval = currentTimestamp - lastCommandTimestamp;

                // If interval between now and last command is less than minimum command time space - wait
                if (interval < COMMAND_TIME_SPACE)
                {
                    Thread.Sleep((int)(COMMAND_TIME_SPACE - interval));
                }

                this.WriteMessage(message);

                this.lastCommandTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                string response = this.ReadMessage();

                if (string.IsNullOrEmpty(response))
                {
                    Disconnect();
                    throw new APICommunicationException("Server not responding. Response has no value.");
                }

                return response;
            }
            finally
            {
                locker.Release();
            }
        }

        /// <summary>
        /// Executes given command and receives response (withholding API inter-command timeout).
        /// </summary>
        /// <param name="message">Command to execute</param>
        /// <returns>Response from the server</returns>
		public async Task<string> ExecuteCommandAsync(string message)
        {
            await locker.WaitAsync();
            try
            {
                long currentTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                long interval = currentTimestamp - lastCommandTimestamp;

                // If interval between now and last command is less than minimum command time space - wait
                if (interval < COMMAND_TIME_SPACE)
                {
                    await Task.Delay((int)(COMMAND_TIME_SPACE - interval));
                }

                await this.WriteMessageAsync(message).ConfigureAwait(false);

                this.lastCommandTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

                string response = await this.ReadMessageAsync().ConfigureAwait(false);

                if (string.IsNullOrEmpty(response))
                {
                    Disconnect();
                    throw new APICommunicationException("Server not responding. Response has no value.");
                }

                return response;
            }
            finally
            {
                locker.Release();
            }
        }

        /// <summary>
        /// Connects to streaming.
        /// </summary>
        /// <returns>New instance of streaming connector</returns>
        [Obsolete("Use Streaming.Connect() instead")]
        public StreamingAPIConnector ConnectStreaming()
        {
            if (this.streamingConnector != null)
                this.streamingConnector.Disconnect();

            this.streamingConnector = new StreamingAPIConnector(this.server);
            return this.streamingConnector;
        }

        /// <summary>
        /// Disconnects streaming.
        /// </summary>
        [Obsolete("Use Streaming.Disconnect() instead")]
        public void DisconnectStreaming()
        {
            if (this.streamingConnector != null)
                this.streamingConnector.Disconnect();
        }

        /// <summary>
        /// Streaming connector.
        /// </summary>
        public StreamingAPIConnector Streaming
        {
            get { return this.streamingConnector; }
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
                    streamingConnector?.Dispose();
                    locker.Dispose();
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
}