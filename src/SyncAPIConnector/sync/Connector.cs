using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using xAPI.Errors;
using xAPI.Utils;

namespace xAPI.Sync;

public class Connector : IClient, IDisposable
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
    public Connector(IPEndPoint endpoint)
    {
        Endpoint = endpoint;
    }

    #region Events
    /// <inheritdoc/>
    public event EventHandler<EndpointEventArgs>? Connected;

    /// <inheritdoc/>
    public event EventHandler<MessageEventArgs>? MessageReceived;

    /// <inheritdoc/>
    public event EventHandler<MessageEventArgs>? MessageSent;

    /// <inheritdoc/>
    public event EventHandler? Disconnected;

    protected void OnConnected(IPEndPoint endpoint) => Connected?.Invoke(this, new(endpoint));
    #endregion Events

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
    public TimeSpan ConnectionTimeout { get; set; } = TimeSpan.FromMilliseconds(TIMEOUT);

    /// <inheritdoc/>
    public IPEndPoint Endpoint { get; init; }

    public bool ShallUseSecureConnection { get; init; }

    /// <inheritdoc/>
    public bool IsConnected => TcpClient.Connected;

    /// <inheritdoc/>
    public virtual void Connect()
    {
        TcpClient = new TcpClient
        {
            ReceiveTimeout = ConnectionTimeout.Milliseconds,
            SendTimeout = ConnectionTimeout.Milliseconds
        };

        try
        {
            TcpClient.Connect(Endpoint);
        }
        catch (Exception ex)
        {
            throw new APICommunicationException($"Connection to {Endpoint} failed.", ex);
        }

        if (ShallUseSecureConnection)
        {
            EstablishSecureConnection();
        }
        else
        {
            NetworkStream ns = TcpClient.GetStream();
            StreamWriter = new StreamWriter(ns);
            StreamReader = new StreamReader(ns);
        }

        OnConnected(Endpoint);
    }

    /// <inheritdoc/>
    public virtual async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        TcpClient = new TcpClient
        {
            ReceiveTimeout = ConnectionTimeout.Milliseconds,
            SendTimeout = ConnectionTimeout.Milliseconds
        };

        try
        {
            var connectTask = TcpClient.ConnectAsync(Endpoint.Address, Endpoint.Port);
            var timeoutTask = Task.Delay(ConnectionTimeout, cancellationToken);

            var completedTask = await Task.WhenAny(connectTask, timeoutTask).ConfigureAwait(false);

            if (completedTask == timeoutTask)
            {
                throw new TimeoutException($"Connection attempt to {Endpoint} timed out.");
            }
        }
        catch (Exception ex)
        {
            throw new APICommunicationException($"Connection to {Endpoint} failed.", ex);
        }

        if (ShallUseSecureConnection)
        {
            await EstablishSecureConnectionAsync(cancellationToken);
        }
        else
        {
            NetworkStream ns = TcpClient.GetStream();
            StreamWriter = new StreamWriter(ns);
            StreamReader = new StreamReader(ns);
        }

        OnConnected(Endpoint);
    }

    private void EstablishSecureConnection()
    {
        var callback = new RemoteCertificateValidationCallback(SslHelper.TrustAllCertificatesCallback);
        var sslStream = new SslStream(TcpClient.GetStream(), false, callback);

        bool authenticated = ExecuteWithTimeLimit.Execute(TimeSpan.FromMilliseconds(5000), () =>
        {
            sslStream.AuthenticateAsClient(Endpoint.Address.ToString(), [], System.Security.Authentication.SslProtocols.None, false);
        });

        if (!authenticated)
            throw new APICommunicationException("Error during SSL handshaking (timed out?).");

        StreamWriter = new StreamWriter(sslStream);
        StreamReader = new StreamReader(sslStream);
    }

    //todo use token in higher .net versions
    private async Task EstablishSecureConnectionAsync(CancellationToken cancellationToken = default)
    {
        var callback = new RemoteCertificateValidationCallback(SslHelper.TrustAllCertificatesCallback);
        var sslStream = new SslStream(TcpClient.GetStream(), false, callback);

        await sslStream.AuthenticateAsClientAsync(Endpoint.Address.ToString(), [], System.Security.Authentication.SslProtocols.None, false);

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
                SendMessageInternal(message);
            }
            else
            {
                Disconnect();
                throw new APICommunicationException("Error while sending data (socket disconnected).");
            }
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
                await SendMessageInternalAsync(message, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                Disconnect();
                throw new APICommunicationException("Error while sending the data (socket disconnected).");
            }
        }
        finally
        {
            _lock.Release();
        }
    }

    public void SendMessageInternal(string message)
    {
        try
        {
            StreamWriter.WriteLine(message);
            StreamWriter.Flush();

            MessageSent?.Invoke(this, new(message));
        }
        catch (IOException ex)
        {
            throw new APICommunicationException($"Error while sending message:'{message.Substring(0, 250)}'", ex);
        }
    }

    public async Task SendMessageInternalAsync(string message, CancellationToken cancellationToken = default)
    {
        try
        {
            await StreamWriter.WriteLineAsync(message).ConfigureAwait(false);
            await StreamWriter.FlushAsync().ConfigureAwait(false);

            MessageSent?.Invoke(this, new(message));
        }
        catch (IOException ex)
        {
            throw new APICommunicationException($"Error while sending message:'{message.Substring(0, 250)}'", ex);
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
    public string SendMessageWaitResponse(string message)
    {
        _lock.Wait();
        try
        {
            SendMessageInternal(message);
            string response = ReadMessage();

            if (string.IsNullOrEmpty(response))
            {
                Disconnect();
                throw new APICommunicationException("Server not responding. Response has no value.");
            }

            return response;
        }
        finally
        {
            _lock.Release();
        }
    }

    /// <inheritdoc/>
    public async Task<string> SendMessageWaitResponseAsync(string message, CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            await SendMessageInternalAsync(message, cancellationToken).ConfigureAwait(false);
            var response = await ReadMessageAsync(cancellationToken).ConfigureAwait(false);

            if (string.IsNullOrEmpty(response))
            {
                Disconnect();
                throw new APICommunicationException("Server not responding. Response has no value.");
            }

            return response;
        }
        finally
        {
            _lock.Release();
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