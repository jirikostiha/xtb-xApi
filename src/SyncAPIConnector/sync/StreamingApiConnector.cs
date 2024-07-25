using System.IO;
using System;
using System.Net.Sockets;
using System.Threading;
using System.Net.Security;

using xAPI.Utils;
using xAPI.Records;
using xAPI.Errors;
using xAPI.Streaming;

using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Security.Authentication;

namespace xAPI.Sync;

public class StreamingApiConnector : Connector
{
    private Task? _streamingReaderTask;

    /// <summary>
    /// Dedicated streaming listener.
    /// </summary>
    private readonly IStreamingListener? _streamingListener;

    /// <summary>
    /// Creates new StreamingAPIConnector instance based on given server data.
    /// </summary>
    /// <param name="server">Server data</param>
    public StreamingApiConnector(Server server)
    {
        Server = server;
        _apiConnected = false;
    }

    /// <summary>
    /// Creates new StreamingAPIConnector instance based on given server data, stream session id and streaming listener.
    /// </summary>
    /// <param name="server">Server data</param>
    /// <param name="streamingListener">Streaming listener.</param>
    public StreamingApiConnector(Server server, IStreamingListener streamingListener)
        : this(server)
    {
        _streamingListener = streamingListener;
    }

    #region Events

    /// <summary>
    /// Event raised when a connection is established.
    /// </summary>
    public event EventHandler<ServerEventArgs>? Connected;

    /// <summary>
    /// Event raised when a tick record is received.
    /// </summary>
    public event EventHandler<TickReceivedEventArgs>? TickReceived;

    /// <summary>
    /// Event raised when a trade record is received.
    /// </summary>
    public event EventHandler<TradeReceivedEventArgs>? TradeReceived;

    /// <summary>
    /// Event raised when a balance record is received.
    /// </summary>
    public event EventHandler<BalanceReceivedEventArgs>? BalanceReceived;

    /// <summary>
    /// Event raised when a trade status record is received.
    /// </summary>
    public event EventHandler<TradeStatusReceivedEventArgs>? TradeStatusReceived;

    /// <summary>
    /// Event raised when a profit record is received.
    /// </summary>
    public event EventHandler<ProfitReceivedEventArgs>? ProfitReceived;

    /// <summary>
    /// Event raised when a news record is received.
    /// </summary>
    public event EventHandler<NewsReceivedEventArgs>? NewsReceived;

    /// <summary>
    /// Event raised when a keep alive record is received.
    /// </summary>
    public event EventHandler<KeepAliveReceivedEventArgs>? KeepAliveReceived;

    /// <summary>
    /// Event raised when a candle record is received.
    /// </summary>
    public event EventHandler<CandleReceivedEventArgs>? CandleReceived;

    /// <summary>
    /// Event raised when read streamed message.
    /// </summary>
    public event EventHandler<ExceptionEventArgs>? StreamingErrorOccurred;

    #endregion

    /// <summary>
    /// Stream session id (member of login response). Should be set after the successful login.
    /// </summary>
    public string? StreamSessionId { get; set; }

    /// <summary>
    /// Connect to the streaming.
    /// </summary>
    public void Connect()
    {
        if (StreamSessionId == null)
        {
            throw new APICommunicationException("No session exists. Please login first.");
        }

        if (IsConnected)
        {
            throw new APICommunicationException("Stream already connected.");
        }

        ApiSocket = new TcpClient();
        var server = Server;
        ApiSocket.Connect(server.Address, server.StreamingPort);

        _apiConnected = true;

        Connected?.Invoke(this, new(server));

        if (server.IsSecure)
        {
            var callback = new RemoteCertificateValidationCallback(SSLHelper.TrustAllCertificatesCallback);
            var ssl = new SslStream(ApiSocket.GetStream(), false, callback);
            ssl.AuthenticateAsClient(server.Address);
            StreamWriter = new StreamWriter(ssl);
            StreamReader = new StreamReader(ssl);
        }
        else
        {
            NetworkStream ns = ApiSocket.GetStream();
            StreamWriter = new StreamWriter(ns);
            StreamReader = new StreamReader(ns);
        }

        if (_streamingReaderTask == null)
        {
            CreateAndRunNewStreamingReaderTask(default);
        }
        else if (_streamingReaderTask.IsCompleted || _streamingReaderTask.IsFaulted || _streamingReaderTask.IsCanceled)
        {
            _streamingReaderTask = null;
            CreateAndRunNewStreamingReaderTask(default);
        }
    }

    /// <summary>
    /// Connect to the streaming.
    /// </summary>
    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        if (StreamSessionId == null)
        {
            throw new APICommunicationException("No session exists. Please login first.");
        }

        if (IsConnected)
        {
            throw new APICommunicationException("Stream already connected.");
        }

        ApiSocket = new TcpClient();
        var server = Server;
        try
        {
            await ApiSocket.ConnectAsync(server.Address, server.StreamingPort);
        }
        catch (OperationCanceledException)
        {
            throw new APICommunicationException("Connection attempt was canceled.");
        }

        _apiConnected = true;

        Connected?.Invoke(this, new(server));

        if (server.IsSecure)
        {
            var callback = new RemoteCertificateValidationCallback(SSLHelper.TrustAllCertificatesCallback);
            var ssl = new SslStream(ApiSocket.GetStream(), false, callback);
            await ssl.AuthenticateAsClientAsync(server.Address);
            StreamWriter = new StreamWriter(ssl);
            StreamReader = new StreamReader(ssl);
        }
        else
        {
            var networkStream = ApiSocket.GetStream();
            StreamWriter = new StreamWriter(networkStream);
            StreamReader = new StreamReader(networkStream);
        }

        if (_streamingReaderTask == null)
        {
            CreateAndRunNewStreamingReaderTask(cancellationToken);
        }
        else if (_streamingReaderTask.IsCompleted || _streamingReaderTask.IsFaulted || _streamingReaderTask.IsCanceled)
        {
            _streamingReaderTask = null;
            CreateAndRunNewStreamingReaderTask(cancellationToken);
        }
    }

    private void CreateAndRunNewStreamingReaderTask(CancellationToken cancellationToken)
    {
        _streamingReaderTask = Task.Run(async () =>
        {
            while (IsConnected)
            {
                await ReadStreamMessageAsync(cancellationToken);
            }
        }, cancellationToken);
    }

    /// <summary>
    /// Reads stream message.
    /// </summary>
    private async Task ReadStreamMessageAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var message = await ReadMessageAsync(cancellationToken).ConfigureAwait(false);

            if (message == null)
                throw new ArgumentNullException(message, "Incoming streaming message is null.");

            var responseBody = JsonNode.Parse(message);
            if (responseBody is null)
                throw new ArgumentNullException(message, "Result of incoming parsed streaming message is null.");

            var commandName = responseBody["command"]?.ToString();
            if (commandName == null)
                throw new ArgumentNullException(commandName, "Incoming streaming command is null.");

            if (commandName == StreamingCommandName.TickPrices)
            {
                var tickRecord = new StreamingTickRecord();
                tickRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                TickReceived?.Invoke(this, new(tickRecord));
                if (_streamingListener != null)
                    await _streamingListener.ReceiveTickRecordAsync(tickRecord, cancellationToken).ConfigureAwait(false);
            }
            else if (commandName == StreamingCommandName.Trade)
            {
                StreamingTradeRecord tradeRecord = new StreamingTradeRecord();
                tradeRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                TradeReceived?.Invoke(this, new(tradeRecord));
                if (_streamingListener != null)
                    await _streamingListener.ReceiveTradeRecordAsync(tradeRecord, cancellationToken).ConfigureAwait(false);
            }
            else if (commandName == StreamingCommandName.Balance)
            {
                StreamingBalanceRecord balanceRecord = new StreamingBalanceRecord();
                balanceRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                BalanceReceived?.Invoke(this, new(balanceRecord));
                if (_streamingListener != null)
                    await _streamingListener.ReceiveBalanceRecordAsync(balanceRecord, cancellationToken).ConfigureAwait(false);
            }
            else if (commandName == StreamingCommandName.TradeStatus)
            {
                StreamingTradeStatusRecord tradeStatusRecord = new StreamingTradeStatusRecord();
                tradeStatusRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                TradeStatusReceived?.Invoke(this, new(tradeStatusRecord));
                if (_streamingListener != null)
                    await _streamingListener.ReceiveTradeStatusRecordAsync(tradeStatusRecord, cancellationToken).ConfigureAwait(false);
            }
            else if (commandName == StreamingCommandName.Profit)
            {
                StreamingProfitRecord profitRecord = new StreamingProfitRecord();
                profitRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                ProfitReceived?.Invoke(this, new(profitRecord));
                if (_streamingListener != null)
                    await _streamingListener.ReceiveProfitRecordAsync(profitRecord, cancellationToken).ConfigureAwait(false);
            }
            else if (commandName == StreamingCommandName.News)
            {
                StreamingNewsRecord newsRecord = new StreamingNewsRecord();
                newsRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                NewsReceived?.Invoke(this, new(newsRecord));
                if (_streamingListener != null)
                    await _streamingListener.ReceiveNewsRecordAsync(newsRecord, cancellationToken).ConfigureAwait(false);
            }
            else if (commandName == StreamingCommandName.KeepAlive)
            {
                StreamingKeepAliveRecord keepAliveRecord = new StreamingKeepAliveRecord();
                keepAliveRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                KeepAliveReceived?.Invoke(this, new(keepAliveRecord));
                if (_streamingListener != null)
                    await _streamingListener.ReceiveKeepAliveRecordAsync(keepAliveRecord, cancellationToken).ConfigureAwait(false);
            }
            else if (commandName == StreamingCommandName.Candle)
            {
                StreamingCandleRecord candleRecord = new StreamingCandleRecord();
                candleRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                CandleReceived?.Invoke(this, new(candleRecord));
                if (_streamingListener != null)
                    await _streamingListener.ReceiveCandleRecordAsync(candleRecord, cancellationToken).ConfigureAwait(false);
            }
            else
            {
                throw new APICommunicationException($"Unknown streaming record received. command:'{commandName}'");
            }
        }
        catch (APICommunicationException ex) when (ex.InnerException.InnerException is SocketException se)
        {
            if (se.ErrorCode != (int)SocketError.OperationAborted)
            {
                throw;
            }
        }
        catch (Exception ex)
        {
            OnStreamingErrorOccurred(ex);
        }
    }

    #region subscribe, unsubscribe
    public void SubscribePrice(string symbol, long? minArrivalTime = null, long? maxLevel = null)
    {
        TickPricesSubscribe tickPricesSubscribe = new(symbol, StreamSessionId, minArrivalTime, maxLevel);
        WriteMessage(tickPricesSubscribe.ToString());
    }

    public void UnsubscribePrice(string symbol)
    {
        TickPricesStop tickPricesStop = new(symbol);
        WriteMessage(tickPricesStop.ToString());
    }

    public void SubscribePrices(string[] symbols)
    {
        foreach (string symbol in symbols)
        {
            SubscribePrice(symbol);
        }
    }

    public void UnsubscribePrices(string[] symbols)
    {
        foreach (string symbol in symbols)
        {
            UnsubscribePrice(symbol);
        }
    }

    public void SubscribeTrades()
    {
        TradeRecordsSubscribe tradeRecordsSubscribe = new(StreamSessionId);
        WriteMessage(tradeRecordsSubscribe.ToString());
    }

    public void UnsubscribeTrades()
    {
        TradeRecordsStop tradeRecordsStop = new();
        WriteMessage(tradeRecordsStop.ToString());
    }

    public void SubscribeBalance()
    {
        BalanceRecordsSubscribe balanceRecordsSubscribe = new(StreamSessionId);
        WriteMessage(balanceRecordsSubscribe.ToString());
    }

    public void UnsubscribeBalance()
    {
        BalanceRecordsStop balanceRecordsStop = new();
        WriteMessage(balanceRecordsStop.ToString());
    }

    public void SubscribeTradeStatus()
    {
        TradeStatusRecordsSubscribe tradeStatusRecordsSubscribe = new(StreamSessionId);
        WriteMessage(tradeStatusRecordsSubscribe.ToString());
    }

    public void UnsubscribeTradeStatus()
    {
        TradeStatusRecordsStop tradeStatusRecordsStop = new();
        WriteMessage(tradeStatusRecordsStop.ToString());
    }

    public void SubscribeProfits()
    {
        ProfitsSubscribe profitsSubscribe = new(StreamSessionId);
        WriteMessage(profitsSubscribe.ToString());
    }

    public void UnsubscribeProfits()
    {
        ProfitsStop profitsStop = new();
        WriteMessage(profitsStop.ToString());
    }

    public void SubscribeNews()
    {
        NewsSubscribe newsSubscribe = new(StreamSessionId);
        WriteMessage(newsSubscribe.ToString());
    }

    public void UnsubscribeNews()
    {
        NewsStop newsStop = new();
        WriteMessage(newsStop.ToString());
    }

    public void SubscribeKeepAlive()
    {
        KeepAliveSubscribe keepAliveSubscribe = new(StreamSessionId);

        WriteMessage(keepAliveSubscribe.ToString());
    }

    public void UnsubscribeKeepAlive()
    {
        KeepAliveStop keepAliveStop = new();
        WriteMessage(keepAliveStop.ToString());
    }

    public void SubscribeCandles(string symbol)
    {
        CandleRecordsSubscribe candleRecordsSubscribe = new(symbol, StreamSessionId);
        WriteMessage(candleRecordsSubscribe.ToString());
    }

    public void UnsubscribeCandles(string symbol)
    {
        CandleRecordsStop candleRecordsStop = new(symbol);
        WriteMessage(candleRecordsStop.ToString());
    }

    public async Task SubscribePriceAsync(string symbol, long? minArrivalTime = null, long? maxLevel = null, CancellationToken cancellationToken = default)
    {
        var tickPricesSubscribe = new TickPricesSubscribe(symbol, StreamSessionId, minArrivalTime, maxLevel);
        await WriteMessageAsync(tickPricesSubscribe.ToString(), cancellationToken);
    }

    public async Task UnsubscribePriceAsync(string symbol, CancellationToken cancellationToken = default)
    {
        var tickPricesStop = new TickPricesStop(symbol);
        await WriteMessageAsync(tickPricesStop.ToString(), cancellationToken);
    }

    public async Task SubscribePricesAsync(string[] symbols, CancellationToken cancellationToken = default)
    {
        foreach (string symbol in symbols)
        {
            await SubscribePriceAsync(symbol, cancellationToken: cancellationToken);
        }
    }

    public async Task UnsubscribePricesAsync(string[] symbols, CancellationToken cancellationToken = default)
    {
        foreach (string symbol in symbols)
        {
            await UnsubscribePriceAsync(symbol, cancellationToken);
        }
    }

    public async Task SubscribeTradesAsync(CancellationToken cancellationToken = default)
    {
        var tradeRecordsSubscribe = new TradeRecordsSubscribe(StreamSessionId);
        await WriteMessageAsync(tradeRecordsSubscribe.ToString(), cancellationToken);
    }

    public async Task UnsubscribeTradesAsync(CancellationToken cancellationToken = default)
    {
        var tradeRecordsStop = new TradeRecordsStop();
        await WriteMessageAsync(tradeRecordsStop.ToString(), cancellationToken);
    }

    public async Task SubscribeBalanceAsync(CancellationToken cancellationToken = default)
    {
        var balanceRecordsSubscribe = new BalanceRecordsSubscribe(StreamSessionId);
        await WriteMessageAsync(balanceRecordsSubscribe.ToString(), cancellationToken);
    }

    public async Task UnsubscribeBalanceAsync(CancellationToken cancellationToken = default)
    {
        var balanceRecordsStop = new BalanceRecordsStop();
        await WriteMessageAsync(balanceRecordsStop.ToString(), cancellationToken);
    }

    public async Task SubscribeTradeStatusAsync(CancellationToken cancellationToken = default)
    {
        var tradeStatusRecordsSubscribe = new TradeStatusRecordsSubscribe(StreamSessionId);
        await WriteMessageAsync(tradeStatusRecordsSubscribe.ToString(), cancellationToken);
    }

    public async Task UnsubscribeTradeStatusAsync(CancellationToken cancellationToken = default)
    {
        var tradeStatusRecordsStop = new TradeStatusRecordsStop();
        await WriteMessageAsync(tradeStatusRecordsStop.ToString(), cancellationToken);
    }

    public async Task SubscribeProfitsAsync(CancellationToken cancellationToken = default)
    {
        var profitsSubscribe = new ProfitsSubscribe(StreamSessionId);
        await WriteMessageAsync(profitsSubscribe.ToString(), cancellationToken);
    }

    public async Task UnsubscribeProfitsAsync(CancellationToken cancellationToken = default)
    {
        var profitsStop = new ProfitsStop();
        await WriteMessageAsync(profitsStop.ToString(), cancellationToken);
    }

    public async Task SubscribeNewsAsync(CancellationToken cancellationToken = default)
    {
        var newsSubscribe = new NewsSubscribe(StreamSessionId);
        await WriteMessageAsync(newsSubscribe.ToString(), cancellationToken);
    }

    public async Task UnsubscribeNewsAsync(CancellationToken cancellationToken = default)
    {
        var newsStop = new NewsStop();
        await WriteMessageAsync(newsStop.ToString(), cancellationToken);
    }

    public async Task SubscribeKeepAliveAsync(CancellationToken cancellationToken = default)
    {
        var keepAliveSubscribe = new KeepAliveSubscribe(StreamSessionId);
        await WriteMessageAsync(keepAliveSubscribe.ToString(), cancellationToken);
    }

    public async Task UnsubscribeKeepAliveAsync(CancellationToken cancellationToken = default)
    {
        var keepAliveStop = new KeepAliveStop();
        await WriteMessageAsync(keepAliveStop.ToString(), cancellationToken);
    }

    public async Task SubscribeCandlesAsync(string symbol, CancellationToken cancellationToken = default)
    {
        var candleRecordsSubscribe = new CandleRecordsSubscribe(symbol, StreamSessionId);
        await WriteMessageAsync(candleRecordsSubscribe.ToString(), cancellationToken);
    }

    public async Task UnsubscribeCandlesAsync(string symbol, CancellationToken cancellationToken = default)
    {
        var candleRecordsStop = new CandleRecordsStop(symbol);
        await WriteMessageAsync(candleRecordsStop.ToString(), cancellationToken);
    }
    #endregion

    protected virtual void OnStreamingErrorOccurred(Exception ex)
    {
        var args = new ExceptionEventArgs(ex);
        StreamingErrorOccurred?.Invoke(this, args);

        if (!args.Handled)
        {
            // If the exception was not handled, rethrow it
            throw new APICommunicationException("Read streaming message failed.", ex);
        }
    }

    private bool _disposed;

    protected override void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            base.Dispose(disposing);
            StreamSessionId = null!;

            _disposed = true;
        }
    }

    ~StreamingApiConnector()
    {
        Dispose(false);
    }
}