using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
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
        private Object writeLocker = new Object();

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
            lock (writeLocker)
            {
                if (this.Connected())
                {
                    try
                    {
                        apiWriteStream.WriteLine(message);
                        apiWriteStream.Flush();
                    }
                    catch (IOException ex)
                    {
                        Disconnect();
                        throw new APICommunicationException("Error while sending the data: " + ex.Message);
                    }
                }
                else
                {
                    Disconnect();
                    throw new APICommunicationException("Error while sending the data (socket disconnected)");
                }

                if (OnMessageSended != null)
                    OnMessageSended.Invoke(message);
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
                byte[] buffer = new byte[apiSocket.ReceiveBufferSize];

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
                    throw new APICommunicationException("Disconnected from server");
                }

                if (OnMessageReceived != null)
                    OnMessageReceived.Invoke(result.ToString());

                return result.ToString();

            }
            catch (Exception ex)
            {
                Disconnect();
                throw new APICommunicationException("Disconnected from server: " + ex.Message);
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

        /// <summary>
        /// Diesposes the client.
        /// </summary>
        public void Dispose()
        {
            this.apiReadStream.Close();
            this.apiWriteStream.Close();
            this.apiSocket.Close();
        }
    }
}
