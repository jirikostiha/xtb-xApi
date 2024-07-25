using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xAPI.Errors;

namespace xAPI.Sync;

public class Connector : ISender, IReceiver, IDisposable
{
    /// <summary>
    /// Lock object used to synchronize access to write socket operations.
    /// </summary>
    private readonly SemaphoreSlim _lock = new(1, 1);

    /// <summary>
    /// Creates new connector instance.
    /// </summary>
    public Connector()
    {
    }

    #region Events
    /// <inheritdoc/>
    public event EventHandler<MessageEventArgs>? MessageReceived;

    /// <inheritdoc/>
    public event EventHandler<MessageEventArgs>? MessageSent;

    /// <inheritdoc/>
    public event EventHandler? Disconnected;
    #endregion Events

    /// <summary>
    /// Server that the connection was established with.
    /// </summary>
    protected Server Server { get; set; }

    /// <summary>
    /// Socket that handles the connection.
    /// </summary>
    protected TcpClient TcpClient { get; set; }

    /// <summary>
    /// Stream writer (for outgoing data).
    /// </summary>
    protected StreamWriter StreamWriter { get; set; }

    /// <summary>
    /// Stream reader (for incoming data).
    /// </summary>
    protected StreamReader StreamReader { get; set; }

    /// <summary>
    /// True if connected to the remote server.
    /// </summary>
    protected volatile bool _apiConnected;

    /// <inheritdoc/>
    public bool IsConnected => _apiConnected;

    /// <inheritdoc/>
    public void SendMessage(string message)
    {
        _lock.Wait();
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
            _lock.Release();
        }
    }

    /// <inheritdoc/>
    public async Task SendMessageAsync(string message, CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            if (IsConnected)
            {
                try
                {
                    await StreamWriter.WriteLineAsync(message).ConfigureAwait(false);
                    await StreamWriter.FlushAsync().ConfigureAwait(false);
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
            _lock.Release();
        }
    }

    /// <inheritdoc/>
    public string ReadMessage()
    {
        var result = new StringBuilder();
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
    public async Task<string> ReadMessageAsync(CancellationToken cancellationToken = default)
    {
        var result = new StringBuilder();
        char lastChar = ' ';

        try
        {
            string line;
            while ((line = await StreamReader.ReadLineAsync().ConfigureAwait(false)) != null)
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

    /// <inheritdoc/>
    public void Disconnect()
    {
        if (IsConnected)
        {
            StreamReader.Close();
            StreamWriter.Close();
            TcpClient.Close();

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
                TcpClient?.Dispose();
                _lock?.Dispose();
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