using System;
using System.Threading;
using xAPI.Errors;
using xAPI.Commands;
using System.Threading.Tasks;
using System.Text.Json.Nodes;

namespace xAPI.Sync
{
    public class ApiConnector
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
        #endregion

        /// <summary>
        /// Last command timestamp (used to calculate interval between each command).
        /// </summary>
        private long _lastCommandTimestamp;

        public ApiConnector(IConnector connector, StreamingApiConnector? streamingApiConnector)
        {
            Connector = connector;
            Streaming = streamingApiConnector;
        }

        public IConnector Connector { get; private set; }

        public bool IsConnected => Connector.IsConnected;

        /// <summary>
        /// Streaming connector.
        /// </summary>
        public StreamingApiConnector Streaming { get; private set; }

        /// <summary>
        /// Connects to the remote server (NOTE: server must be already set).
        /// </summary>
        public bool Connect()
        {
            if (IsConnected)
                return false;

            return Connector.Connect();

            //Streaming = new StreamingApiConnector();
        }

        /// Redirects to the given server.
        /// </summary>
        /// <param name="server">Server data</param>
        //public void Redirect(Server server)
        //{
        //    Redirected?.Invoke(this, new(server));

        //    if (IsConnected)
        //        Disconnect(true);

        //    Server = server;
        //    Connect();
        //}

        /// <summary>
        /// Executes given command and receives response (withholding API inter-command timeout).
        /// </summary>
        /// <param name="cmd">Command to execute</param>
        /// <returns>Response from the server</returns>
		public JsonObject ExecuteCommand(BaseCommand cmd)
        {
            try
            {
                var response = ExecuteCommand(cmd.ToJSONString());
                return (JsonObject)JsonObject.Parse(response);
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
                var response = await ExecuteCommandAsync(cmd.ToJSONString()).ConfigureAwait(false);
                return (JsonObject)JsonObject.Parse(response);
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
            long currentTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            long interval = currentTimestamp - _lastCommandTimestamp;

            // If interval between now and last command is less than minimum command time space - wait
            if (interval < COMMAND_TIME_SPACE)
            {
                Thread.Sleep((int)(COMMAND_TIME_SPACE - interval));
            }

            Connector.WriteMessage(message);

            _lastCommandTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            string response = Connector.ReadMessage();

            if (string.IsNullOrEmpty(response))
            {
                Connector.Disconnect();
                throw new APICommunicationException("Server not responding. Response has no value.");
            }

            return response;
        }

        /// <summary>
        /// Executes given command and receives response (withholding API inter-command timeout).
        /// </summary>
        /// <param name="message">Command to execute</param>
        /// <returns>Response from the server</returns>
		public async Task<string> ExecuteCommandAsync(string message)
        {
            long currentTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            long interval = currentTimestamp - _lastCommandTimestamp;

            // If interval between now and last command is less than minimum command time space - wait
            if (interval < COMMAND_TIME_SPACE)
            {
                await Task.Delay((int)(COMMAND_TIME_SPACE - interval));
            }

            await Connector.WriteMessageAsync(message).ConfigureAwait(false);

            _lastCommandTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            string response = await Connector.ReadMessageAsync().ConfigureAwait(false);

            if (string.IsNullOrEmpty(response))
            {
                Connector.Disconnect();
                throw new APICommunicationException("Server not responding. Response has no value.");
            }

            return response;
        }
    }
}