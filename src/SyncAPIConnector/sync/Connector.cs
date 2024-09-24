using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xtb.XApi.Utils;

namespace Xtb.XApi;

public class Connector : IClient, IDisposable
{
    /// <summary>
    /// Default maximum connection time (in milliseconds). After that the connection attempt is immediately dropped.
    /// </summary>
    public const int DefaultConnectionTimeout = 5000;

    /// <summary>
    /// Lock object used to synchronize access to write socket operations.
    /// </summary>
    private readonly SemaphoreSlim _lock = new(1, 1);

    /// <summary>
    /// Creates new instance.
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
    /// Endpoint that the connection was established with.
    /// </summary>
    public IPEndPoint Endpoint { get; set; }

    /// <summary>
    /// Determines if secure connection shall be used.
    /// </summary>
    public bool ShallUseSecureConnection { get; init; }

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
    public TimeSpan ConnectionTimeout { get; set; } = TimeSpan.FromMilliseconds(DefaultConnectionTimeout);

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

    ///// <summary>
    ///// Connects to the remote server.
    ///// </summary>
    //public void Connect()
    //{
    //    if (Endpoint == null)
    //        throw new APICommunicationException("No endpoint to connect to.");

    //    var endpoint = Endpoint;
    //    TcpClient = new TcpClient();

    //    bool connectionAttempted = false;

    //    while (!connectionAttempted || !TcpClient.Connected)
    //    {
    //        // Try to connect asynchronously and wait for the result
    //        IAsyncResult result = TcpClient.BeginConnect(endpoint.Address, endpoint.Port, null, null);
    //        connectionAttempted = result.AsyncWaitHandle.WaitOne(ConnectionTimeout, true);

    //        // If connection attempt failed (timeout) or not connected
    //        if (!connectionAttempted || !TcpClient.Connected)
    //        {
    //            TcpClient.Close();
    //            throw new APICommunicationException($"Cannot connect to:{endpoint.Address}:{endpoint.Port}");
    //        }
    //    }

    //    if (ShallUseSecureConnection)
    //    {
    //        EstablishSecureConnection();
    //    }
    //    else
    //    {
    //        NetworkStream ns = TcpClient.GetStream();
    //        StreamWriter = new StreamWriter(ns);
    //        StreamReader = new StreamReader(ns);
    //    }

    //    IsConnected = true;

    //    Connected?.Invoke(this, new(endpoint));

    //    //Streaming = new StreamingApiConnector(_streamingEndpoint, _streamingListener);
    //}

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

    /// <summary>
    /// Connects to the remote server.
    /// </summary>
    /// <param name="cancellationToken">Token to cancel operation.</param>
    //public async Task ConnectAsync(CancellationToken cancellationToken = default)
    //{
    //    if (Endpoint == null)
    //        throw new APICommunicationException("No server to connect to.");

    //    var endpoint = Endpoint;
    //    TcpClient = new TcpClient
    //    {
    //        ReceiveTimeout = ConnectionTimeout.Milliseconds,
    //        SendTimeout = ConnectionTimeout.Milliseconds
    //    };

    //    bool connectionAttempted = false;

    //    while (!connectionAttempted || !TcpClient.Connected)
    //    {
    //        try
    //        {
    //            // Try to connect asynchronously and wait for the result
    //            var connectTask = TcpClient.ConnectAsync(endpoint.Address, endpoint.Port);
    //            var timeoutTask = Task.Delay(ConnectionTimeout, cancellationToken);

    //            var completedTask = await Task.WhenAny(connectTask, timeoutTask);

    //            connectionAttempted = completedTask == connectTask && TcpClient.Connected;

    //            if (cancellationToken.IsCancellationRequested)
    //            {
    //                TcpClient.Close();
    //                throw new OperationCanceledException(cancellationToken);
    //            }

    //            if (!connectionAttempted || !TcpClient.Connected)
    //            {
    //                TcpClient.Close();
    //                throw new APICommunicationException($"Cannot connect to:{endpoint.Address}:{endpoint.Port}");
    //            }
    //        }
    //        catch
    //        {
    //            TcpClient.Close();
    //            throw new APICommunicationException($"Cannot connect to:{endpoint.Address}:{endpoint.Port}");
    //        }
    //    }

    //    if (ShallUseSecureConnection)
    //    {
    //        await EstablishSecureConnectionAsync(cancellationToken);
    //    }
    //    else
    //    {
    //        NetworkStream ns = TcpClient.GetStream();
    //        StreamWriter = new StreamWriter(ns);
    //        StreamReader = new StreamReader(ns);
    //    }

    //    IsConnected = true;

    //    Connected?.Invoke(this, new(endpoint));

    //    //Streaming = new StreamingApiConnector(_streamingEndpoint, _streamingListener);
    //}

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
#if NET8_0_OR_GREATER
            await StreamWriter.FlushAsync(cancellationToken).ConfigureAwait(false);
#else
            await StreamWriter.FlushAsync().ConfigureAwait(false);
#endif

            MessageSent?.Invoke(this, new(message));
        }
        catch (IOException ex)
        {
            throw new APICommunicationException($"Error while sending message:'{message.Substring(0, 250)}'", ex);
        }
    }

    /// <summary>
    /// Reads raw message from the remote server.
    /// </summary>
    /// <returns>Read message</returns>
    public string? ReadMessage()
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
    public async Task<string?> ReadMessageAsync(CancellationToken cancellationToken = default)
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

    /// <inheritdoc/>
    public string SendMessageWaitResponse(string message)
    {
        _lock.Wait();
        try
        {
            SendMessageInternal(message);
            string? response = ReadMessage();

            if (string.IsNullOrEmpty(response))
            {
                Disconnect();
                throw new APICommunicationException("Server not responding. Response has no value.");
            }

            return response!;
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
            string? response = await ReadMessageAsync(cancellationToken).ConfigureAwait(false);

            if (string.IsNullOrEmpty(response))
            {
                Disconnect();
                throw new APICommunicationException("Server not responding. Response has no value.");
            }

            return response!;
        }
        finally
        {
            _lock.Release();
        }
    }

    protected void EstablishSecureConnection()
    {
#pragma warning disable CA5359 // Do Not Disable Certificate Validation
        var callback = new RemoteCertificateValidationCallback(SslHelper.TrustAllCertificatesCallback);
#pragma warning restore CA5359 // Do Not Disable Certificate Validation
        var sslStream = new SslStream(TcpClient.GetStream(), false, callback);

        bool authenticated = ExecuteWithTimeLimit.Execute(TimeSpan.FromMilliseconds(5000), () =>
        {
#if NET5_0_OR_GREATER
            sslStream.AuthenticateAsClient(CreateAuthenticationOptions());
#else
            sslStream.AuthenticateAsClient(Endpoint.Address.ToString(), [], SslProtocols.None, false);
#endif
        });

        if (!authenticated)
            throw new APICommunicationException("Error during SSL handshaking (timed out?).");

        StreamWriter = new StreamWriter(sslStream);
        StreamReader = new StreamReader(sslStream);
    }

    protected async Task EstablishSecureConnectionAsync(CancellationToken cancellationToken = default)
    {
#pragma warning disable CA5359 // Do Not Disable Certificate Validation
        var callback = new RemoteCertificateValidationCallback(SslHelper.TrustAllCertificatesCallback);
#pragma warning restore CA5359 // Do Not Disable Certificate Validation

        var sslStream = new SslStream(TcpClient.GetStream(), false, callback);

#if NETSTANDARD2_1_OR_GREATER
        await sslStream.AuthenticateAsClientAsync(CreateAuthenticationOptions(), cancellationToken);
#else
        await sslStream.AuthenticateAsClientAsync(Endpoint.Address.ToString(), [], SslProtocols.None, false);
#endif

        StreamWriter = new StreamWriter(sslStream);
        StreamReader = new StreamReader(sslStream);
    }

#if (NETSTANDARD2_1_OR_GREATER || NETCOREAPP2_1_OR_GREATER)
    private SslClientAuthenticationOptions CreateAuthenticationOptions()
    {
        return new SslClientAuthenticationOptions()
        {
            TargetHost = Endpoint.Address.ToString(),
            ClientCertificates = [],
            EnabledSslProtocols = SslProtocols.None,
            CertificateRevocationCheckMode = X509RevocationMode.NoCheck
        };
    }
#endif

    /// <summary>
    /// Disconnects from the remote server.
    /// </summary>
    public void Disconnect()
    {
        if (IsConnected)
        {
            StreamReader.Close();
            StreamWriter.Close();
            TcpClient.Close();

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