using System;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xAPI.Errors;
using xAPI.Utils;

namespace xAPI.Sync;

public class Connector : ISender, IReceiver, IConnector, IDisposable
{
    /// <summary>
    /// Default maximum connection time (in milliseconds). After that the connection attempt is immediately dropped.
    /// </summary>
    public const int TIMEOUT = 5000;

    /// <summary>
    /// Lock object used to synchronize access to write socket operations.
    /// </summary>
    private readonly SemaphoreSlim _lock = new(1, 1);

    /// <summary>
    /// Creates new connector instance.
    /// </summary>
    public Connector(Server server)
    {
        Server = server;
    }

    #region Events
    /// <inheritdoc/>
    public event EventHandler<ServerEventArgs>? Connected;

    /// <inheritdoc/>
    public event EventHandler<MessageEventArgs>? MessageReceived;

    /// <inheritdoc/>
    public event EventHandler<MessageEventArgs>? MessageSent;

    /// <inheritdoc/>
    public event EventHandler? Disconnected;

    protected void OnConnected(Server server) => Connected?.Invoke(this, new(server));
    #endregion Events

    /// <summary>
    /// Server that the connection was established with.
    /// </summary>
    protected Server Server { get; init; }

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
    /// Maximum connection time. After that the connection attempt is immediately dropped.
    /// </summary>
    public TimeSpan ConnectionTimeout { get; set; }

    /// <inheritdoc/>
    public bool IsConnected => TcpClient.Connected;

    /// <inheritdoc/>
    public virtual void Connect()
    {
        if (Server == null)
            throw new APICommunicationException("No server to connect to.");

        TcpClient = new TcpClient
        {
            ReceiveTimeout = ConnectionTimeout.Milliseconds,
            SendTimeout = ConnectionTimeout.Milliseconds
        };

        try
        {
            TcpClient.Connect(Server.Address, Server.StreamingPort);
        }
        catch (Exception ex)
        {
            throw new APICommunicationException($"Connection to {Server.Address} failed.", ex);
        }

        if (Server.IsSecure)
        {
            EstablishSecureConnection();
        }
        else
        {
            NetworkStream ns = TcpClient.GetStream();
            StreamWriter = new StreamWriter(ns);
            StreamReader = new StreamReader(ns);
        }

        OnConnected(Server);
    }

    /// <inheritdoc/>
    public virtual async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        if (Server == null)
            throw new APICommunicationException("No server to connect to.");

        TcpClient = new TcpClient
        {
            ReceiveTimeout = ConnectionTimeout.Milliseconds,
            SendTimeout = ConnectionTimeout.Milliseconds
        };

        try
        {
            var connectTask = TcpClient.ConnectAsync(Server.Address, Server.MainPort);
            var timeoutTask = Task.Delay(ConnectionTimeout, cancellationToken);

            var completedTask = await Task.WhenAny(connectTask, timeoutTask).ConfigureAwait(false);

            if (completedTask == timeoutTask)
            {
                throw new TimeoutException($"Connection attempt to {Server.Address} timed out.");
            }
        }
        catch (Exception ex)
        {
            throw new APICommunicationException($"Connection to {Server.Address} failed.", ex);
        }

        if (Server.IsSecure)
        {
            await EstablishSecureConnectionAsync(cancellationToken);
        }
        else
        {
            NetworkStream ns = TcpClient.GetStream();
            StreamWriter = new StreamWriter(ns);
            StreamReader = new StreamReader(ns);
        }

        OnConnected(Server);
    }

    private void EstablishSecureConnection()
    {
        var callback = new RemoteCertificateValidationCallback(SSLHelper.TrustAllCertificatesCallback);
        var sslStream = new SslStream(TcpClient.GetStream(), false, callback);

        bool authenticated = ExecuteWithTimeLimit.Execute(TimeSpan.FromMilliseconds(5000), () =>
        {
            sslStream.AuthenticateAsClient(Server.Address, [], System.Security.Authentication.SslProtocols.None, false);
        });

        if (!authenticated)
            throw new APICommunicationException("Error during SSL handshaking (timed out?).");

        StreamWriter = new StreamWriter(sslStream);
        StreamReader = new StreamReader(sslStream);
    }

    //todo use token in higher .net versions
    private async Task EstablishSecureConnectionAsync(CancellationToken cancellationToken = default)
    {
        var callback = new RemoteCertificateValidationCallback(SSLHelper.TrustAllCertificatesCallback);
        var sslStream = new SslStream(TcpClient.GetStream(), false, callback);

        await sslStream.AuthenticateAsClientAsync(Server.Address, [], System.Security.Authentication.SslProtocols.None, false);

        StreamWriter = new StreamWriter(sslStream);
        StreamReader = new StreamReader(sslStream);
    }

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
    public async Task<string?> ReadMessageAsync(CancellationToken cancellationToken = default)
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
            TcpClient.Client.Disconnect(true);

            Disconnected?.Invoke(this, EventArgs.Empty);
        }
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