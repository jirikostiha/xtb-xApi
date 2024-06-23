using System;
using System.Net;
using System.Threading.Tasks;

namespace xAPI.Sync
{
    public interface IConnector
    {
        /// <summary>
        /// Event raised when the client connects to the server.
        /// </summary>
        event EventHandler<ServerEventArgs>? Connected;

        /// <summary>
        /// Event raised when the client is trying to reconnects to the server.
        /// </summary>
        event EventHandler<ServerEventArgs>? Reconnecting;

        /// <summary>
        /// Event raised when a message is sent.
        /// </summary>
        event EventHandler<MessageEventArgs>? MessageSent;

        /// <summary>
        /// Event raised when a message is received.
        /// </summary>
        event EventHandler<MessageEventArgs>? MessageReceived;

        /// <summary>
        /// Event raised when the client disconnects from the server.
        /// </summary>
        event EventHandler? Disconnected;

        /// <summary>
        /// Connection endpoint.
        /// </summary>
        public IPEndPoint Endpoint { get; }

        /// <summary>
        /// Indicates whether the client is connected to the server.
        /// </summary>
        bool IsConnected { get; }

        /// <summary>
        /// Connect client to the remote server.
        /// </summary>
        /// <returns> true if connected; false if already connected </returns>
        bool Connect();

        /// <summary>
        /// Connect client async to the remote server.
        /// </summary>
        Task<bool> ConnectAsync();

        /// <summary>
        /// Write a message to the remote server.
        /// </summary>
        /// <param name="message">Message to send</param>
        void WriteMessage(string message);

        /// <summary>
        /// Write a message to the remote server.
        /// </summary>
        /// <param name="message">A message to send</param>
        Task WriteMessageAsync(string message);

        /// <summary>
        /// Read a message from the remote server.
        /// </summary>
        /// <returns>A message</returns>
        string ReadMessage();

        /// <summary>
        /// Read a message from the remote server.
        /// </summary>
        /// <returns>A message</returns>
        Task<string> ReadMessageAsync();

        /// <summary>
        /// Disconnects client from the remote server.
        /// </summary>
        /// <returns> true if disconnected; false if already disconnected </returns>
        bool Disconnect();
    }
}