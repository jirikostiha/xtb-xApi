using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace XApi;

public class Connector : IDisposable
{
    /// <summary>
    /// Default maximum connection time (in milliseconds). After that the connection attempt is immediately dropped.
    /// </summary>
    private const int TIMEOUT = 5000;

    /// <summary>
    /// Lock object used to synchronize access to write socket operations.
    /// </summary>
    private readonly SemaphoreSlim _writeLock = new(1, 1);

    /// <summary>
    /// Creates new instance.
    /// </summary>
    public Connector(IPEndPoint endpoint)
    {
        Endpoint = endpoint;
    }

    #region Events

    /// <summary>
    /// Event raised when a message is received.
    /// </summary>
    public event EventHandler<MessageEventArgs>? MessageReceived;

    /// <summary>
    /// Event raised when a message is sent.
    /// </summary>
    public event EventHandler<MessageEventArgs>? MessageSent;

    /// <summary>
    /// Event raised when the client disconnects from the server.
    /// </summary>
    public event EventHandler? Disconnected;

    #endregion Events

    /// <summary>
    /// Endpoint that the connection was established with.
    /// </summary>
    public IPEndPoint Endpoint { get; set; }

    public bool ShallUseSecureConnection { get; init; }

    /// <summary>
    /// Socket that handles the connection.
    /// </summary>
    protected TcpClient ApiSocket { get; set; }

    /// <summary>
    /// Stream writer (for outgoing data).
    /// </summary>
    protected StreamWriter StreamWriter { get; set; }

    /// <summary>
    /// Stream reader (for incoming data).
    /// </summary>
    protected StreamReader StreamReader { get; set; }

    /// <summary>
    /// Maximum connection time. After that the connection attempt is immediately dropped.
    /// </summary>
    public TimeSpan ConnectionTimeout { get; set; } = TimeSpan.FromMilliseconds(TIMEOUT);


    /// <summary>
    /// True if connected to the remote server.
    /// </summary>
    protected volatile bool _apiConnected;

    /// <summary>
    /// Checks if socket is connected to the remote server.
    /// </summary>
    /// <returns>True if socket is connected, otherwise false</returns>
    public bool IsConnected => _apiConnected;

    /// <summary>
    /// Writes raw message to the remote server.
    /// </summary>
    /// <param name="message">Message to send</param>
    protected void WriteMessage(string message)
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

    /// <summary>
    /// Writes raw message to the remote server.
    /// </summary>
    /// <param name="message">Message to send</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
    protected async Task WriteMessageAsync(string message, CancellationToken cancellationToken = default)
    {
        await _writeLock.WaitAsync(cancellationToken);
        try
        {
            if (IsConnected)
            {
                try
                {
                    await StreamWriter.WriteLineAsync(message).ConfigureAwait(false);
#if NET8_0_OR_GREATER
                    await StreamWriter.FlushAsync(cancellationToken).ConfigureAwait(false);
#else
                    await StreamWriter.FlushAsync().ConfigureAwait(false);
#endif
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

    /// <summary>
    /// Reads raw message from the remote server.
    /// </summary>
    /// <returns>Read message</returns>
    protected string ReadMessage()
    {
        var result = new StringBuilder();
        char lastChar = ' ';

        try
        {
            string? line;
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

    /// <summary>
    /// Reads raw message from the remote server.
    /// </summary>
    /// <returns>Read message</returns>
    protected async Task<string> ReadMessageAsync(CancellationToken cancellationToken = default)
    {
        var result = new StringBuilder();
        char lastChar = ' ';

        try
        {
            string? line;

#if NET8_0_OR_GREATER
            while ((line = await StreamReader.ReadLineAsync(cancellationToken).ConfigureAwait(false)) != null)
#else
            while ((line = await StreamReader.ReadLineAsync().ConfigureAwait(false)) != null)
#endif
            {
                cancellationToken.ThrowIfCancellationRequested();

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

    /// <summary>
    /// Disconnects from the remote server.
    /// </summary>
    /// <param name="silent">If true then no event will be triggered (used in redirect process)</param>
    public void Disconnect(bool silent = false)
    {
        if (IsConnected)
        {
            StreamReader.Close();
            StreamWriter.Close();
            ApiSocket.Close();

            Disconnected?.Invoke(this, EventArgs.Empty);
        }

        _apiConnected = false;
    }

    private bool _disposed;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                StreamReader?.Dispose();
                StreamWriter?.Dispose();
                ApiSocket?.Dispose();
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

    ~Connector()
    {
        Dispose(false);
    }
}