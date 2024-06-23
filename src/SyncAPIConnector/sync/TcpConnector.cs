using SyncAPIConnect.Utils;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xAPI.Errors;
using xAPI.Utils;

namespace xAPI.Sync
{
    public class TcpConnector : IConnector, IDisposable
    {
        /// <summary>
        /// Lock object used to synchronize access to write socket operations.
        /// </summary>
        private readonly SemaphoreSlim _writeLock = new(1, 1);

        /// <summary>
        /// Socket that handles the connection.
        /// </summary>
        protected TcpClient TcpClient { get; set; }

        /// <summary>
        /// Stream writer (for outgoing data).
        /// </summary>
        protected StreamWriter? StreamWriter { get; set; }

        /// <summary>
        /// Stream reader (for incoming data).
        /// </summary>
        protected StreamReader? StreamReader { get; set; }

        /// <summary>
        /// Create new instance.
        /// </summary>
        public TcpConnector(IPEndPoint endpoint)
        {
            Endpoint = endpoint;
            TcpClient = new TcpClient();
        }

        #region Events
        /// <inheritdoc/>
        public event EventHandler<ServerEventArgs>? Connected;

        /// <inheritdoc/>
        public event EventHandler<ServerEventArgs>? Reconnecting;

        /// <inheritdoc/>
        public event EventHandler<MessageEventArgs>? MessageReceived;

        /// <inheritdoc/>
        public event EventHandler<MessageEventArgs>? MessageSent;

        /// <inheritdoc/>
        public event EventHandler? Disconnected;
        #endregion

        public IPEndPoint Endpoint { get; }

        public TimeSpan Timeout { get; set; }

        /// <inheritdoc/>
        public bool IsConnected => TcpClient.Connected;

        /// <inheritdoc/>
        public bool Connect()
        {
            if (IsConnected)
                return false;

            TcpClient.ReceiveTimeout = Timeout.Milliseconds;
            TcpClient.SendTimeout = Timeout.Milliseconds;
            TcpClient.Connect(Endpoint);
            StreamWriter = new StreamWriter(TcpClient.GetStream());

            //if (Server.IsSecure)
            if (false)
            {
                RemoteCertificateValidationCallback userCertificateValidationCallback = new(SSLHelper.TrustAllCertificatesCallback);
                SslStream sl = new(TcpClient.GetStream(), false, userCertificateValidationCallback);

                //sl.AuthenticateAsClient(server.Address);

                bool authenticated = ExecuteWithTimeLimit.Execute(TimeSpan.FromMilliseconds(5000), () =>
                {
                    sl.AuthenticateAsClient(HostName, new X509CertificateCollection(), System.Security.Authentication.SslProtocols.None, false);
                });

                if (!authenticated)
                    throw new APICommunicationException("Error during SSL handshaking (timed out?).");

                StreamWriter = new StreamWriter(sl) { AutoFlush = true };
                StreamReader = new StreamReader(sl);
            }
            else
            {
                NetworkStream ns = TcpClient.GetStream();
                StreamWriter = new StreamWriter(ns) { AutoFlush = true };
                StreamReader = new StreamReader(ns);
            }

            //IsConnected = true;

            Connected?.Invoke(this, new(HostName, Port));

            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> ConnectAsync()
        {
            if (IsConnected)
                return false;

            TcpClient.ReceiveTimeout = Timeout.Milliseconds;
            TcpClient.SendTimeout = Timeout.Milliseconds;
            await TcpClient.ConnectAsync(HostName, Port).ConfigureAwait(false);
            StreamWriter = new StreamWriter(TcpClient.GetStream());

            //if (Server.IsSecure)
            if (false)
            {
                var sl = CertificationVerification();
                StreamWriter = new StreamWriter(sl) { AutoFlush = true };
                StreamReader = new StreamReader(sl);
            }
            else
            {
                NetworkStream ns = TcpClient.GetStream();
                StreamWriter = new StreamWriter(ns) { AutoFlush = true };
                StreamReader = new StreamReader(ns);
            }

            //IsConnected = true;

            Connected?.Invoke(this, new(HostName, Port));

            return true;
        }

        /// <inheritdoc/>
        public void WriteMessage(string message)
        {
            _writeLock.Wait();
            try
            {
                if (IsConnected)
                {
                    try
                    {
                        StreamWriter.WriteLine(message);
                        StreamWriter.Flush();
                    }
                    catch (IOException ex)
                    {
                        Disconnect();
                        throw new APICommunicationException($"Error while sending data:'{message.Substring(0, 250)}'", ex);
                    }
                }
                else
                {
                    Disconnect();
                    throw new APICommunicationException("Error while sending data (socket disconnected).");
                }

                MessageSent?.Invoke(this, new(message));
            }
            finally
            {
                _writeLock.Release();
            }
        }

        /// <inheritdoc/>
        public async Task WriteMessageAsync(string message)
        {
            await _writeLock.WaitAsync();
            try
            {
                if (IsConnected)
                {
                    try
                    {
                        await StreamWriter.WriteLineAsync(message).ConfigureAwait(false);
                    }
                    catch (IOException ex)
                    {
                        Disconnect();
                        throw new APICommunicationException($"Error while sending data:'{message.Substring(0, 250)}'", ex);
                    }
                }
                else
                {
                    Disconnect();
                    throw new APICommunicationException("Error while sending the data (socket disconnected).");
                }

                MessageSent?.Invoke(this, new(message));
            }
            finally
            {
                _writeLock.Release();
            }
        }

        /// <inheritdoc/>
        public string ReadMessage()
        {
            StringBuilder result = new();
            char lastChar = ' ';

            try
            {
                string line;
                while ((line = StreamReader.ReadLine()) != null)
                {
                    result.Append(line);

                    // Last line is always empty
                    if (line == "" && lastChar == '}')
                        break;

                    if (line.Length != 0)
                    {
                        lastChar = line[line.Length - 1];
                    }
                }

                if (line == null)
                {
                    Disconnect();
                    throw new APICommunicationException("Disconnected from server. No data in stream.");
                }

                MessageReceived?.Invoke(this, new(result.ToString()));

                return result.ToString();

            }
            catch (Exception ex)
            {
                Disconnect();
                throw new APICommunicationException("Disconnected from server.", ex);
            }
        }

        /// <inheritdoc/>
        public async Task<string> ReadMessageAsync()
        {
            StringBuilder result = new();
            char lastChar = ' ';

            try
            {
                string line;
                while ((line = await StreamReader.ReadLineAsync().ConfigureAwait(false)) != null)
                {
                    result.Append(line);

                    // Last line is always empty
                    if (line == "" && lastChar == '}')
                        break;

                    if (line.Length != 0)
                    {
                        lastChar = line[line.Length - 1];
                    }
                }

                if (line == null)
                {
                    Disconnect();
                    throw new APICommunicationException("Disconnected from server. No data in stream.");
                }

                MessageReceived?.Invoke(this, new(result.ToString()));

                return result.ToString();

            }
            catch (Exception ex)
            {
                Disconnect();
                throw new APICommunicationException("Disconnected from server.", ex);
            }
        }

        private SslStream CertificationVerification()
        {
            var userCertificateValidationCallback = new RemoteCertificateValidationCallback(SSLHelper.TrustAllCertificatesCallback);
            var sl = new SslStream(TcpClient.GetStream(), false, userCertificateValidationCallback);

            //sl.AuthenticateAsClient(server.Address);

            bool authenticated = ExecuteWithTimeLimit.Execute(TimeSpan.FromMilliseconds(5000), () =>
            {
                sl.AuthenticateAsClient(HostName, new X509CertificateCollection(), System.Security.Authentication.SslProtocols.None, false);
            });

            if (!authenticated)
                throw new APICommunicationException("Error during SSL handshaking (timed out?).");

            return sl;
        }

        /// <inheritdoc/>
        public bool Disconnect()
        {
            if (!IsConnected)
                return false;

            StreamReader?.Close();
            StreamWriter?.Close();
            TcpClient?.Close();
            //_isConnected = false;

            Disconnected?.Invoke(this, EventArgs.Empty);

            return true;
        }

        #region disposing
        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    StreamReader?.Dispose();
                    StreamWriter?.Dispose();
                    TcpClient?.Dispose();
                    _writeLock?.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~TcpConnector()
        {
            Dispose(false);
        }
        #endregion Disposing
    }
}