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
    /// Helper method to create a new instance based on address and port.
    /// </summary>
    /// <param name="address">Endpoint address.</param>
    /// <param name="port">Endpoint port.</param>
    public static Connector Create(string address, int port)
    {
        var endpoint = new IPEndPoint(IPAddress.Parse(address), port);
        return new Connector(endpoint);
    }

    /// <summary>
    /// Lock object used to synchronize access to write socket operations.
    /// </summary>
    private readonly SemaphoreSlim _lock = new(1, 1);

    /// <summary>
    /// Creates new instance.
    /// </summary>
    public Connector(IPEndPoint endpoint, ConnectorOptions? _options = null)
    {
        Options = _options ?? ConnectorOptions.Default;
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
    /// Options.
    /// </summary>
    protected ConnectorOptions Options { get; init; }

    /// <summary>
    /// Socket that handles the connection.
    /// </summary>
    protected TcpClient TcpClient { get; set; } = new TcpClient();

    /// <summary>
    /// Stream writer (for outgoing data).
    /// </summary>
    protected StreamWriter StreamWriter { get; set; } = StreamWriter.Null;

    /// <summary>
    /// Stream reader (for incoming data).
    /// </summary>
    protected StreamReader StreamReader { get; set; } = StreamReader.Null;

    /// <inheritdoc/>
    public bool IsConnected => TcpClient?.Connected ?? false;

    private TcpClient CreateTcpClient()
    {
        return new TcpClient
        {
            ReceiveTimeout = Options.ReceiveTimeout.Milliseconds,
            SendTimeout = Options.SendTimeout.Milliseconds
        };
    }

    /// <inheritdoc/>
    public virtual void Connect()
    {
        if (_disposed)
            throw new ObjectDisposedException(ToString());

        TcpClient = CreateTcpClient();

        try
        {
            TcpClient.Connect(Endpoint);
        }
        catch (Exception ex)
        {
            throw new APICommunicationException($"Connection to {Endpoint} failed.", ex);
        }

        EstablishSecureConnection();

        OnConnected(Endpoint);
    }

    /// <inheritdoc/>
    public virtual async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        if (_disposed)
            throw new ObjectDisposedException(ToString());

        TcpClient = CreateTcpClient();

        try
        {
            var connectTask = TcpClient.ConnectAsync(Endpoint.Address, Endpoint.Port);
            var timeoutTask = Task.Delay(Options.ReceiveTimeout, cancellationToken);

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

        await EstablishSecureConnectionAsync(cancellationToken);

        OnConnected(Endpoint);
    }

    /// <inheritdoc/>
    public void SendMessage(string message)
    {
        if (_disposed)
            throw new ObjectDisposedException(ToString());

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
        if (_disposed)
            throw new ObjectDisposedException(ToString());

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

    protected void SendMessageInternal(string message)
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

    protected async Task SendMessageInternalAsync(string message, CancellationToken cancellationToken = default)
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
        if (_disposed)
            throw new ObjectDisposedException(ToString());

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
        if (_disposed)
            throw new ObjectDisposedException(ToString());

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
        if (_disposed)
            throw new ObjectDisposedException(ToString());

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
        if (_disposed)
            throw new ObjectDisposedException(ToString());

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
        if (_disposed)
            throw new ObjectDisposedException(ToString());

        if (IsConnected)
        {
            StreamReader.Close();
            StreamWriter.Close();
            TcpClient.Close();

            Disconnected?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <inheritdoc/>
    public Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        Disconnect();
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    public override string ToString() => $"{Endpoint?.ToString() ?? "no endpoint"}, {(IsConnected ? "connected" : "disconnected")}";

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

    /// <inheritdoc/>
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