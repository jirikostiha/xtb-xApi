using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xAPI.Errors;

namespace xAPI.Sync
{
    public class Connector : IDisposable
    {
        #region Events
        /// <summary>
        /// Delegate called on message arrival from the server.
        /// </summary>
        /// <param name="response">Received response</param>
        public delegate void OnReceiveMessageCallback(string response);

        /// <summary>
        /// Event raised when message is received.
        /// </summary>
        public event OnReceiveMessageCallback OnMessageReceived;

        /// <summary>
        /// Delegate called on message send to the server.
        /// </summary>
        /// <param name="command">Command sent</param>
        public delegate void OnSendMessageCallback(string message);

        /// <summary>
        /// Event raised when message is sended.
        /// </summary>
        public event OnSendMessageCallback OnMessageSended;

        /// <summary>
        /// Delegate called on client disconnection from the server.
        /// </summary>
        public delegate void OnDisconnectCallback();

        /// <summary>
        /// Event raised when client disconnects from server.
        /// </summary>
        public event OnDisconnectCallback OnDisconnected;
        #endregion

        /// <summary>
        /// Socket that handles the connection.
        /// </summary>
        protected TcpClient apiSocket;

        /// <summary>
        /// Stream writer (for outgoing data).
        /// </summary>
        protected StreamWriter apiWriteStream;

        /// <summary>
        /// Stream reader (for incoming data).
        /// </summary>
        protected StreamReader apiReadStream;

        /// <summary>
        /// True if connected to the remote server.
        /// </summary>
        protected volatile bool apiConnected;

        /// <summary>
        /// Server that the connection was established with.
        /// </summary>
        protected Server server;

        /// <summary>
        /// Lock object used to synchronize access to write socket operations.
        /// </summary>
        //private Object writeLocker = new Object();
        private readonly SemaphoreSlim writeLocker = new SemaphoreSlim(1, 1);

        /// <summary>
        /// Creates new connector instance.
        /// </summary>
        public Connector()
        {

        }

        /// <summary>
        /// Checks if socket is connected to the remote server.
        /// </summary>
        /// <returns>True if socket is connected, otherwise false</returns>
        public bool Connected()
        {
            return this.apiConnected;
        }

        /// <summary>
        /// Writes raw message to the remote server.
        /// </summary>
        /// <param name="message">Message to send</param>
        protected void WriteMessage(string message)
        {
            writeLocker.Wait();
            try
            {
                if (Connected())
                {
                    try
                    {
                        apiWriteStream.WriteLine(message);
                        apiWriteStream.Flush();
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

                if (OnMessageSended != null)
                    OnMessageSended.Invoke(message);
            }
            finally
            {
                writeLocker.Release();
            }
        }

        /// <summary>
        /// Writes raw message to the remote server.
        /// </summary>
        /// <param name="message">Message to send</param>
        protected async Task WriteMessageAsync(string message)
        {
            await writeLocker.WaitAsync();
            try
            {
                if (Connected())
                {
                    try
                    {
                        await apiWriteStream.WriteLineAsync(message).ConfigureAwait(false);
                        await apiWriteStream.FlushAsync().ConfigureAwait(false);
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

                if (OnMessageSended != null)
                    OnMessageSended.Invoke(message);
            }
            finally
            {
                writeLocker.Release();
            }
        }

        /// <summary>
        /// Reads raw message from the remote server.
        /// </summary>
        /// <returns>Read message</returns>
        protected string ReadMessage()
        {
            StringBuilder result = new StringBuilder();
            char lastChar = ' ';

            try
            {
                string line;
                while ((line = apiReadStream.ReadLine()) != null)
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

                if (OnMessageReceived != null)
                    OnMessageReceived.Invoke(result.ToString());

                return result.ToString();

            }
            catch (Exception ex)
            {
                Disconnect();
                throw new APICommunicationException("Disconnected from server.", ex);
            }
        }

        /// <summary>
        /// Reads raw message from the remote server.
        /// </summary>
        /// <returns>Read message</returns>
        protected async Task<string> ReadMessageAsync()
        {
            StringBuilder result = new StringBuilder();
            char lastChar = ' ';

            try
            {
                string line;
                while ((line = await apiReadStream.ReadLineAsync().ConfigureAwait(false)) != null)
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

                if (OnMessageReceived != null)
                    OnMessageReceived.Invoke(result.ToString());

                return result.ToString();

            }
            catch (Exception ex)
            {
                Disconnect();
                throw new APICommunicationException("Disconnected from server.", ex);
            }
        }


        /// <summary>
        /// Disconnects from the remote server.
        /// </summary>
        /// <param name="silent">If true then no event will be trigered (used in redirect process)</param>
        public void Disconnect(bool silent = false)
        {
            if (Connected())
            {
                apiReadStream.Close();
                apiWriteStream.Close();
                apiSocket.Close();

                if (!silent && OnDisconnected != null)
                    OnDisconnected.Invoke();
            }

            this.apiConnected = false;
        }

        private bool _disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    apiReadStream?.Dispose();
                    apiWriteStream?.Dispose();
                    apiSocket?.Dispose();
                    writeLocker?.Dispose();
                }

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~Connector()
        {
            Dispose(false);
        }
    }
}