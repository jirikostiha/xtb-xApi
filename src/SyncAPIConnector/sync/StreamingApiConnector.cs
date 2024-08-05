
using System;
using System.Net.Sockets;
using System.Threading;

using xAPI.Records;
using xAPI.Errors;
using xAPI.Streaming;

using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Net;

namespace xAPI.Sync;

public class StreamingApiConnector : IConnectable
{
    private Task? _streamingReaderTask;

    /// <summary>
    /// Dedicated streaming listener.
    /// </summary>
    private readonly IStreamingListener? _streamingListener;

    /// <summary>
    /// Creates new instance.
    /// </summary>
    /// <param name="connector">Underlying client.</param>
    public StreamingApiConnector(IClient connector)
    {
        Connector = connector;
    }

    /// <summary>
    /// Creates new instance.
    /// </summary>
    /// <param name="endpoint">Endpoint for streaming data.</param>
    public StreamingApiConnector(IPEndPoint endpoint)
        : this(new Connector(endpoint))
    {
    }

    /// <summary>
    /// Creates new instance.
    /// </summary>
    /// <param name="connector">Underlying client.</param>
    /// <param name="streamingListener">Streaming listener.</param>
    public StreamingApiConnector(IClient connector, IStreamingListener streamingListener)
        :this(connector)
    {
        _streamingListener = streamingListener;
    }

    /// <summary>
    /// Creates new instance.
    /// </summary>
    /// <param name="endpoint">Endpoint for streaming data.</param>
    /// <param name="streamingListener">Streaming listener.</param>
    public StreamingApiConnector(IPEndPoint endpoint, IStreamingListener streamingListener)
        : this(new Connector(endpoint), streamingListener)
    {
    }

    #region Events
    /// <inheritdoc/>
    public event EventHandler<EndpointEventArgs>? Connected
    {
        add => Connector.Connected += value;
        remove => Connector.Connected -= value;
    }

    /// <inheritdoc/>
    public event EventHandler? Disconnected
    {
        add => Connector.Disconnected += value;
        remove => Connector.Disconnected -= value;
    }

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
    /// Streaming connector.
    /// </summary>
    protected IClient Connector { get; private set; }

    /// <summary>
    /// Stream session id (member of login response). Should be set after the successful login.
    /// </summary>
    public string? StreamSessionId { get; set; }

    /// <inheritdoc/>
    public bool IsConnected => Connector.IsConnected;

    /// <inheritdoc/>
    public IPEndPoint Endpoint => Connector.Endpoint;

    /// <inheritdoc/>
    public void Connect()
    {
        if (StreamSessionId == null)
        {
            throw new APICommunicationException("No session exists. Please login first.");
        }

        Connector.Connect();

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

    /// <inheritdoc/>
    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        if (StreamSessionId == null)
        {
            throw new APICommunicationException("No session exists. Please login first.");
        }

        await Connector.ConnectAsync(cancellationToken);

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
            var message = await Connector.ReadMessageAsync(cancellationToken).ConfigureAwait(false);

            if (message == null)
                throw new InvalidOperationException("Incoming streaming message is null.");

            var responseBody = JsonNode.Parse(message);
            if (responseBody is null)
                throw new InvalidOperationException("Result of incoming parsed streaming message is null.");

            var commandName = responseBody["command"]?.ToString();
            if (commandName == null)
                throw new InvalidOperationException("Incoming streaming command is null.");

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
                var tradeRecord = new StreamingTradeRecord();
                tradeRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                TradeReceived?.Invoke(this, new(tradeRecord));
                if (_streamingListener != null)
                    await _streamingListener.ReceiveTradeRecordAsync(tradeRecord, cancellationToken).ConfigureAwait(false);
            }
            else if (commandName == StreamingCommandName.Balance)
            {
                var balanceRecord = new StreamingBalanceRecord();
                balanceRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                BalanceReceived?.Invoke(this, new(balanceRecord));
                if (_streamingListener != null)
                    await _streamingListener.ReceiveBalanceRecordAsync(balanceRecord, cancellationToken).ConfigureAwait(false);
            }
            else if (commandName == StreamingCommandName.TradeStatus)
            {
                var tradeStatusRecord = new StreamingTradeStatusRecord();
                tradeStatusRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                TradeStatusReceived?.Invoke(this, new(tradeStatusRecord));
                if (_streamingListener != null)
                    await _streamingListener.ReceiveTradeStatusRecordAsync(tradeStatusRecord, cancellationToken).ConfigureAwait(false);
            }
            else if (commandName == StreamingCommandName.Profit)
            {
                var profitRecord = new StreamingProfitRecord();
                profitRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                ProfitReceived?.Invoke(this, new(profitRecord));
                if (_streamingListener != null)
                    await _streamingListener.ReceiveProfitRecordAsync(profitRecord, cancellationToken).ConfigureAwait(false);
            }
            else if (commandName == StreamingCommandName.News)
            {
                var newsRecord = new StreamingNewsRecord();
                newsRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                NewsReceived?.Invoke(this, new(newsRecord));
                if (_streamingListener != null)
                    await _streamingListener.ReceiveNewsRecordAsync(newsRecord, cancellationToken).ConfigureAwait(false);
            }
            else if (commandName == StreamingCommandName.KeepAlive)
            {
                var keepAliveRecord = new StreamingKeepAliveRecord();
                keepAliveRecord.FieldsFromJsonObject(responseBody["data"].AsObject());

                KeepAliveReceived?.Invoke(this, new(keepAliveRecord));
                if (_streamingListener != null)
                    await _streamingListener.ReceiveKeepAliveRecordAsync(keepAliveRecord, cancellationToken).ConfigureAwait(false);
            }
            else if (commandName == StreamingCommandName.Candle)
            {
                var candleRecord = new StreamingCandleRecord();
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
    public void SubscribePrice(string symbol, long? minArrivalTime = null, int? maxLevel = null)
    {
        var tickPricesSubscribe = new TickPricesSubscribe(symbol, StreamSessionId, minArrivalTime, maxLevel);
        Connector.SendMessage(tickPricesSubscribe.ToString());
    }

    public void UnsubscribePrice(string symbol)
    {
        var tickPricesStop = new TickPricesStop(symbol);
        Connector.SendMessage(tickPricesStop.ToString());
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
        var tradeRecordsSubscribe = new TradeRecordsSubscribe(StreamSessionId);
        Connector.SendMessage(tradeRecordsSubscribe.ToString());
    }

    public void UnsubscribeTrades()
    {
        var tradeRecordsStop = new TradeRecordsStop();
        Connector.SendMessage(tradeRecordsStop.ToString());
    }

    public void SubscribeBalance()
    {
        var balanceRecordsSubscribe = new BalanceRecordsSubscribe(StreamSessionId);
        Connector.SendMessage(balanceRecordsSubscribe.ToString());
    }

    public void UnsubscribeBalance()
    {
        var balanceRecordsStop = new BalanceRecordsStop();
        Connector.SendMessage(balanceRecordsStop.ToString());
    }

    public void SubscribeTradeStatus()
    {
        var tradeStatusRecordsSubscribe = new TradeStatusRecordsSubscribe(StreamSessionId);
        Connector.SendMessage(tradeStatusRecordsSubscribe.ToString());
    }

    public void UnsubscribeTradeStatus()
    {
        var tradeStatusRecordsStop = new TradeRecordsSubscribe(StreamSessionId);
        Connector.SendMessage(tradeStatusRecordsStop.ToString());
    }

    public void SubscribeProfits()
    {
        var profitsSubscribe = new ProfitsSubscribe(StreamSessionId);
        Connector.SendMessage(profitsSubscribe.ToString());
    }

    public void UnsubscribeProfits()
    {
        var profitsStop = new ProfitsStop();
        Connector.SendMessage(profitsStop.ToString());
    }

    public void SubscribeNews()
    {
        var newsSubscribe = new NewsSubscribe(StreamSessionId);
        Connector.SendMessage(newsSubscribe.ToString());
    }

    public void UnsubscribeNews()
    {
        var newsStop = new NewsStop();
        Connector.SendMessage(newsStop.ToString());
    }

    public void SubscribeKeepAlive()
    {
        var keepAliveSubscribe = new KeepAliveSubscribe(StreamSessionId);
        Connector.SendMessage(keepAliveSubscribe.ToString());
    }

    public void UnsubscribeKeepAlive()
    {
        var keepAliveStop = new KeepAliveStop();
        Connector.SendMessage(keepAliveStop.ToString());
    }

    public void SubscribeCandles(string symbol)
    {
        var candleRecordsSubscribe = new CandleRecordsSubscribe(symbol, StreamSessionId);
        Connector.SendMessage(candleRecordsSubscribe.ToString());
    }

    public void UnsubscribeCandles(string symbol)
    {
        var candleRecordsStop = new CandleRecordsStop(symbol);
        Connector.SendMessage(candleRecordsStop.ToString());
    }

    public async Task SubscribePriceAsync(string symbol, long? minArrivalTime = null, int? maxLevel = null, CancellationToken cancellationToken = default)
    {
        var tickPricesSubscribe = new TickPricesSubscribe(symbol, StreamSessionId, minArrivalTime, maxLevel);
        await Connector.SendMessageAsync(tickPricesSubscribe.ToString(), cancellationToken);
    }

    public async Task UnsubscribePriceAsync(string symbol, CancellationToken cancellationToken = default)
    {
        var tickPricesStop = new TickPricesStop(symbol);
        await Connector.SendMessageAsync(tickPricesStop.ToString(), cancellationToken);
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
        await Connector.SendMessageAsync(tradeRecordsSubscribe.ToString(), cancellationToken);
    }

    public async Task UnsubscribeTradesAsync(CancellationToken cancellationToken = default)
    {
        var tradeRecordsStop = new TradeRecordsStop();
        await Connector.SendMessageAsync(tradeRecordsStop.ToString(), cancellationToken);
    }

    public async Task SubscribeBalanceAsync(CancellationToken cancellationToken = default)
    {
        var balanceRecordsSubscribe = new BalanceRecordsSubscribe(StreamSessionId);
        await Connector.SendMessageAsync(balanceRecordsSubscribe.ToString(), cancellationToken);
    }

    public async Task UnsubscribeBalanceAsync(CancellationToken cancellationToken = default)
    {
        var balanceRecordsStop = new BalanceRecordsStop();
        await Connector.SendMessageAsync(balanceRecordsStop.ToString(), cancellationToken);
    }

    public async Task SubscribeTradeStatusAsync(CancellationToken cancellationToken = default)
    {
        var tradeStatusRecordsSubscribe = new TradeStatusRecordsSubscribe(StreamSessionId);
        await Connector.SendMessageAsync(tradeStatusRecordsSubscribe.ToString(), cancellationToken);
    }

    public async Task UnsubscribeTradeStatusAsync(CancellationToken cancellationToken = default)
    {
        var tradeStatusRecordsStop = new TradeStatusRecordsStop();
        await Connector.SendMessageAsync(tradeStatusRecordsStop.ToString(), cancellationToken);
    }

    public async Task SubscribeProfitsAsync(CancellationToken cancellationToken = default)
    {
        var profitsSubscribe = new ProfitsSubscribe(StreamSessionId);
        await Connector.SendMessageAsync(profitsSubscribe.ToString(), cancellationToken);
    }

    public async Task UnsubscribeProfitsAsync(CancellationToken cancellationToken = default)
    {
        var profitsStop = new ProfitsStop();
        await Connector.SendMessageAsync(profitsStop.ToString(), cancellationToken);
    }

    public async Task SubscribeNewsAsync(CancellationToken cancellationToken = default)
    {
        var newsSubscribe = new NewsSubscribe(StreamSessionId);
        await Connector.SendMessageAsync(newsSubscribe.ToString(), cancellationToken);
    }

    public async Task UnsubscribeNewsAsync(CancellationToken cancellationToken = default)
    {
        var newsStop = new NewsStop();
        await Connector.SendMessageAsync(newsStop.ToString(), cancellationToken);
    }

    public async Task SubscribeKeepAliveAsync(CancellationToken cancellationToken = default)
    {
        var keepAliveSubscribe = new KeepAliveSubscribe(StreamSessionId);
        await Connector.SendMessageAsync(keepAliveSubscribe.ToString(), cancellationToken);
    }

    public async Task UnsubscribeKeepAliveAsync(CancellationToken cancellationToken = default)
    {
        var keepAliveStop = new KeepAliveStop();
        await Connector.SendMessageAsync(keepAliveStop.ToString(), cancellationToken);
    }

    public async Task SubscribeCandlesAsync(string symbol, CancellationToken cancellationToken = default)
    {
        var candleRecordsSubscribe = new CandleRecordsSubscribe(symbol, StreamSessionId);
        await Connector.SendMessageAsync(candleRecordsSubscribe.ToString(), cancellationToken);
    }

    public async Task UnsubscribeCandlesAsync(string symbol, CancellationToken cancellationToken = default)
    {
        var candleRecordsStop = new CandleRecordsStop(symbol);
        await Connector.SendMessageAsync(candleRecordsStop.ToString(), cancellationToken);
    }
    #endregion

    protected virtual void OnStreamingErrorOccurred(Exception ex)
    {
        var args = new ExceptionEventArgs(ex);
        StreamingErrorOccurred?.Invoke(this, args);

        if (!args.Handled)
        {
            // If the exception was not handled, re-throw it
            throw new APICommunicationException("Read streaming message failed.", ex);
        }
    }

    public void Disconnect() => Connector.Disconnect();
}