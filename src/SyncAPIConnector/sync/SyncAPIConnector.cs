using System;
using System.Text;
using System.Threading;
using System.Runtime.Serialization.Json;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net.Security;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using JSONObject = Newtonsoft.Json.Linq.JObject;
using xAPI.Streaming;
using System.Net;
using System.Runtime.CompilerServices;
using xAPI.Errors;
using xAPI.Commands;
using xAPI.Streaming;
using xAPI.Utils;
using SyncAPIConnect.Utils;

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
        /// Maximum connection time (in milliseconds). After that the connection attempt is immidiately dropped.
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
        private long lastCommandTimestamp = 0;

        /// <summary>
        /// Lock object used to synchronize access to read/write socket operations.
        /// </summary>
        private Object locker = new Object();

        /// <summary>
        /// Creates new SyncAPIConnector instance based on given Server data.
        /// </summary>
        /// <param name="server">Server data</param>
        /// <param name="lookForBackups">If false, no connection to backup servers will be made</param>
        public SyncAPIConnector(Server server, bool lookForBackups = true)
        {
            Connect(server, lookForBackups);
        }

        /// <summary>
        /// Connects to the remote server.
        /// </summary>
        /// <param name="server">Server data</param>
        /// <param name="lookForBackups">If false, no connection to backup servers will be made</param>
        private void Connect(Server server, bool lookForBackups = true)
        {
            this.server = server;
            this.apiSocket = new System.Net.Sockets.TcpClient();

            bool connectionAttempted = false;

            while (!connectionAttempted || !apiSocket.Connected)
            {
                // Try to connect asynchronously and wait for the result
                IAsyncResult result = apiSocket.BeginConnect(this.server.Address, this.server.MainPort, null, null);
                connectionAttempted = result.AsyncWaitHandle.WaitOne(TIMEOUT, true);

                // If connection attempt failed (timeout) or not connected
                if (!connectionAttempted || !apiSocket.Connected)
                {
                    apiSocket.Close();
                    if (lookForBackups)
                    {
                        this.server = Servers.GetBackup(this.server);
                        apiSocket = new System.Net.Sockets.TcpClient();
                    }
                    else
                    {
                        throw new APICommunicationException("Cannot connect to: " + server.Address + ":" + server.MainPort);
                    }
                }
            }

            if (server.Secure)
            {
                SslStream sl = new SslStream(apiSocket.GetStream(), false, new RemoteCertificateValidationCallback(SSLHelper.TrustAllCertificatesCallback));

                //sl.AuthenticateAsClient(server.Address);

                bool authenticated = ExecuteWithTimeLimit.Execute(TimeSpan.FromMilliseconds(5000), () =>
                {
                    sl.AuthenticateAsClient(server.Address, new X509CertificateCollection(), System.Security.Authentication.SslProtocols.Default, false);
                });

                if (!authenticated)
                    throw new APICommunicationException("Error during SSL handshaking (timed out?)");

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
            
            if(OnConnected != null)
                OnConnected.Invoke(this.server);


            this.streamingConnector = new StreamingAPIConnector(this.server);
        }

        /// <summary>
        /// Connects to the remote server (NOTE: server must be already set).
        /// </summary>
        public void Connect()
        {
            if (this.server != null)
                Connect(this.server);

            throw new APICommunicationException("No server to connect to");
        }

        /// <summary>
        /// Creates new SyncAPIConnector instance based on given data.
        /// </summary>
        /// <param name="address">Address</param>
        /// <param name="port">Main port</param>
        /// <param name="secure">SSL enabled</param>
        [Obsolete("Use SyncAPIConnector(Server server) instead")]
        private SyncAPIConnector(string address, int port, bool secure) : this(new Server(address, port, port+1, secure, ""))
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
		public JSONObject ExecuteCommand(BaseCommand cmd)
		{
			try
			{
				return (JSONObject)JSONObject.Parse(this.ExecuteCommand(cmd.ToJSONString()));
			}
			catch (Exception ex)
			{
				throw new APICommunicationException("Problem with executing command: " + ex.Message);
			}
		}
		
        /// <summary>
        /// Executes given command and receives response (withholding API inter-command timeout).
        /// </summary>
        /// <param name="message">Command to execute</param>
        /// <returns>Response from the server</returns>
		public string ExecuteCommand(string message)
        {
            lock (locker)
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

                if (response == null || response.Equals(""))
                {
                    Disconnect();
                    throw new APICommunicationException("Server not responding");
                }

			    return response;
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

    }
}