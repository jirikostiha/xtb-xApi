using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using xAPI.Commands;
using xAPI.Errors;

namespace xAPI.Sync;

public class Connector : IDisposable//, IConnector
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

    /// <summary>
    /// Event raised when a request is being sent.
    /// </summary>
    public event EventHandler<RequestEventArgs>? RequestSent;

    /// <summary>
    /// Event raised when a message is sent.
    /// </summary>
    public event EventHandler<MessageEventArgs>? MessageSent;


    /// <summary>
    /// Event raised when a message is received.
    /// </summary>
    public event EventHandler<MessageEventArgs>? MessageReceived;

    /// <summary>
    /// Event raised when the client disconnects from the server.
    /// </summary>
    public event EventHandler? Disconnected;

    #endregion Events

    /// <summary>
    /// Server that the connection was established with.
    /// </summary>
    protected Server Server { get; set; }

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
    /// True if connected to the remote server.
    /// </summary>
    protected volatile bool _apiConnected;

    /// <summary>
    /// Checks if socket is connected to the remote server.
    /// </summary>
    /// <returns>True if socket is connected, otherwise false</returns>
    public bool IsConnected => _apiConnected;

    public void SendRequest(ICommand command)
    {
        RequestSent?.Invoke(this, new(command));

        var request = command.ToString();
        WriteMessage(request);
    }

    public async Task SendRequestAsync(ICommand command)
    {
        RequestSent?.Invoke(this, new(command));

        var request = command.ToString();
        await WriteMessageAsync(request);
    }

    /// <summary>
    /// Executes given command and receives response (withholding API inter-command timeout).
    /// </summary>
    /// <param name="command">Command to execute</param>
    /// <returns>Response from the server</returns>
    public JsonObject? SendRequestWaitResponse(BaseCommand command) //ICommand
    {
        try
        {
            RequestSent?.Invoke(this, new(command));

            var request = command.ToJSONString();
            var response = SendRequestWaitResponse(request);

            var parsedResponse = JsonNode.Parse(response)?.AsObject();

            return parsedResponse;
        }
        catch (Exception ex)
        {
            throw new APICommunicationException($"Problem with executing command:'{command.CommandName}'", ex);
        }
    }

    /// <summary>
    /// Executes given command and receives response (withholding API inter-command timeout).
    /// </summary>
    /// <param name="message">Command to execute</param>
    /// <returns>Response from the server</returns>
    private string SendRequestWaitResponse(string message)
    {
        _lock.Wait();
        try
        {
            long currentTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            long interval = currentTimestamp - _lastCommandTimestamp;

            // If interval between now and last command is less than minimum command time space - wait
            if (interval < COMMAND_TIME_SPACE)
            {
                Thread.Sleep((int)(COMMAND_TIME_SPACE - interval));
            }

            WriteMessage(message);

            _lastCommandTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

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

    /// <summary>
    /// Executes given command and receives response (withholding API inter-command timeout).
    /// </summary>
    /// <param name="command">Command to execute</param>
    /// <param name="cancellationToken">Token to cancel operation.</param>
    /// <returns>Response from the server</returns>
    public async Task<JsonObject?> SendRequestWaitResponseAsync(BaseCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            RequestSent?.Invoke(this, new(command));

            var request = command.ToJSONString();
            var response = await SendRequestWaitResponseAsync(request, cancellationToken).ConfigureAwait(false);

            var parsedResponse = JsonNode.Parse(response)?.AsObject();

            return parsedResponse;
        }
        catch (Exception ex)
        {
            throw new APICommunicationException($"Problem with executing command:'{command.CommandName}'", ex);
        }
    }

    /// <summary>
    /// Executes given command and receives response (withholding API inter-command timeout).
    /// </summary>
    /// <param name="message">Command to execute</param>
    /// <param name="cancellationToken">Token to cancel operation.</param>
    /// <returns>Response from the server</returns>
    internal async Task<string> SendRequestWaitResponseAsync(string message, CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            long currentTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            long interval = currentTimestamp - _lastCommandTimestamp;

            // If interval between now and last command is less than minimum command time space - wait
            if (interval < COMMAND_TIME_SPACE)
            {
                await Task.Delay((int)(COMMAND_TIME_SPACE - interval), cancellationToken);
            }

            await WriteMessageAsync(message, cancellationToken).ConfigureAwait(false);

            _lastCommandTimestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;

            string response = await ReadMessageAsync().ConfigureAwait(false);

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

    /// <summary>
    /// Writes raw message to the remote server.
    /// </summary>
    /// <param name="message">Message to send</param>
    protected void WriteMessage(string message)
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

    /// <summary>
    /// Writes raw message to the remote server.
    /// </summary>
    /// <param name="message">Message to send</param>
    /// <param name="cancellationToken">Cancellation token to cancel the operation</param>
    protected async Task WriteMessageAsync(string message, CancellationToken cancellationToken = default)
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

    /// <summary>
    /// Reads raw message from the remote server.
    /// </summary>
    /// <returns>Read message</returns>
    protected async Task<string> ReadMessageAsync()
    {
        var result = new StringBuilder();
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