
using System;
using System.Net;
using System.Net.Sockets;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Xtb.XApi.Records;
using Xtb.XApi.Streaming;

namespace Xtb.XApi;

public class StreamingApiConnector : IConnectable, IDisposable
{
    private Task? _streamingReaderTask;

    /// <summary>
    /// Helper method to create a new instance based on address and port.
    /// </summary>
    /// <param name="address">Endpoint address.</param>
    /// <param name="port">Port for streaming data.</param>
    /// <param name="streamingListener">Streaming listener.</param>
    public static StreamingApiConnector Create(string address, int port, IStreamingListener? streamingListener = null)
    {
        var endpoint = new IPEndPoint(IPAddress.Parse(address), port);

        return Create(endpoint, streamingListener);
    }

    /// <summary>
    /// Helper method to create a new instance based on address and port.
    /// </summary>
    /// <param name="endpoint">Endpoint for streaming data.</param>
    /// <param name="streamingListener">Streaming listener.</param>
    public static StreamingApiConnector Create(IPEndPoint endpoint, IStreamingListener? streamingListener = null)
    {
        var client = new Connector(endpoint);

        return new StreamingApiConnector(client, streamingListener)
        {
            IsConnectorOwner = true
        };
    }

    /// <summary>
    /// Creates new instance.
    /// </summary>
    /// <param name="connector">Underlaying client.</param>
    /// <param name="streamingListener">Streaming listener.</param>
    public StreamingApiConnector(IClient connector, IStreamingListener? streamingListener = null)
    {
        Connector = connector;
        StreamingListener = streamingListener;
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
    #endregion Events

    /// <summary>
    /// Streaming connector.
    /// </summary>
    protected IClient Connector { get; private set; }

    /// <summary>
    /// Indicates whether the connector is owned.
    /// </summary>
    internal bool IsConnectorOwner { get; init; }

    /// <summary>
    /// Stream session id (member of login response). Should be set after the successful login.
    /// </summary>
    public string? StreamSessionId { get; set; }

    /// <summary>
    /// Dedicated streaming listener.
    /// </summary>
    public IStreamingListener? StreamingListener { get; }

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
        string? commandName = null;
        try
        {
            var message = await Connector.ReadMessageAsync(cancellationToken).ConfigureAwait(false)
                ?? throw new InvalidOperationException("Incoming streaming message is null.");

            var responseBody = JsonNode.Parse(message)
                ?? throw new InvalidOperationException("Result of incoming parsed streaming message is null.");

            commandName = (responseBody["command"]?.ToString())
                ?? throw new InvalidOperationException("Incoming streaming command is null.");

            var jsonSubnode = responseBody["data"]
                ?? throw new InvalidOperationException("Parsed json data object is null.");

            var jsonDataObject = jsonSubnode.AsObject();

            if (commandName == StreamingCommandName.TickPrices)
            {
                var tickRecord = new StreamingTickRecord();
                tickRecord.FieldsFromJsonObject(jsonDataObject);

                if (StreamingListener != null)
                    await StreamingListener.ReceiveTickRecordAsync(tickRecord, cancellationToken).ConfigureAwait(false);

                TickReceived?.Invoke(this, new(tickRecord));
            }
            else if (commandName == StreamingCommandName.Trade)
            {
                var tradeRecord = new StreamingTradeRecord();
                tradeRecord.FieldsFromJsonObject(jsonDataObject);

                if (StreamingListener != null)
                    await StreamingListener.ReceiveTradeRecordAsync(tradeRecord, cancellationToken).ConfigureAwait(false);

                TradeReceived?.Invoke(this, new(tradeRecord));
            }
            else if (commandName == StreamingCommandName.Balance)
            {
                var balanceRecord = new StreamingBalanceRecord();
                balanceRecord.FieldsFromJsonObject(jsonDataObject);

                if (StreamingListener != null)
                    await StreamingListener.ReceiveBalanceRecordAsync(balanceRecord, cancellationToken).ConfigureAwait(false);

                BalanceReceived?.Invoke(this, new(balanceRecord));
            }
            else if (commandName == StreamingCommandName.TradeStatus)
            {
                var tradeStatusRecord = new StreamingTradeStatusRecord();
                tradeStatusRecord.FieldsFromJsonObject(jsonDataObject);

                if (StreamingListener != null)
                    await StreamingListener.ReceiveTradeStatusRecordAsync(tradeStatusRecord, cancellationToken).ConfigureAwait(false);

                TradeStatusReceived?.Invoke(this, new(tradeStatusRecord));
            }
            else if (commandName == StreamingCommandName.Profit)
            {
                var profitRecord = new StreamingProfitRecord();
                profitRecord.FieldsFromJsonObject(jsonDataObject);

                if (StreamingListener != null)
                    await StreamingListener.ReceiveProfitRecordAsync(profitRecord, cancellationToken).ConfigureAwait(false);

                ProfitReceived?.Invoke(this, new(profitRecord));
            }
            else if (commandName == StreamingCommandName.News)
            {
                var newsRecord = new StreamingNewsRecord();
                newsRecord.FieldsFromJsonObject(jsonDataObject);

                if (StreamingListener != null)
                    await StreamingListener.ReceiveNewsRecordAsync(newsRecord, cancellationToken).ConfigureAwait(false);

                NewsReceived?.Invoke(this, new(newsRecord));
            }
            else if (commandName == StreamingCommandName.KeepAlive)
            {
                var keepAliveRecord = new StreamingKeepAliveRecord();
                keepAliveRecord.FieldsFromJsonObject(jsonDataObject);

                if (StreamingListener != null)
                    await StreamingListener.ReceiveKeepAliveRecordAsync(keepAliveRecord, cancellationToken).ConfigureAwait(false);

                KeepAliveReceived?.Invoke(this, new(keepAliveRecord));
            }
            else if (commandName == StreamingCommandName.Candle)
            {
                var candleRecord = new StreamingCandleRecord();
                candleRecord.FieldsFromJsonObject(jsonDataObject);

                if (StreamingListener != null)
                    await StreamingListener.ReceiveCandleRecordAsync(candleRecord, cancellationToken).ConfigureAwait(false);

                CandleReceived?.Invoke(this, new(candleRecord));
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
            OnStreamingErrorOccurred(ex, commandName);
        }
    }

    #region subscribe, unsubscribe

    public void SubscribePrice(string symbol, DateTimeOffset? minArrivalTime = null, int? maxLevel = null)
    {
        var tickPricesSubscribe = new TickPricesSubscribe(symbol, GetVerifiedSessionId(), minArrivalTime, maxLevel);
        Connector.SendMessage(tickPricesSubscribe.ToString());
    }

    public void UnsubscribePrice(string symbol)
    {
        var tickPricesStop = new TickPricesStop(symbol);
        Connector.SendMessage(tickPricesStop.ToString());
    }

    public void SubscribePrices(string[] symbols)
    {
        foreach (var symbol in symbols)
        {
            SubscribePrice(symbol);
        }
    }

    public void UnsubscribePrices(string[] symbols)
    {
        foreach (var symbol in symbols)
        {
            UnsubscribePrice(symbol);
        }
    }

    public void SubscribeTrades()
    {
        var tradeRecordsSubscribe = new TradeRecordsSubscribe(GetVerifiedSessionId());
        Connector.SendMessage(tradeRecordsSubscribe.ToString());
    }

    public void UnsubscribeTrades()
    {
        var tradeRecordsStop = new TradeRecordsStop();
        Connector.SendMessage(tradeRecordsStop.ToString());
    }

    public void SubscribeBalance()
    {
        var balanceRecordsSubscribe = new BalanceRecordsSubscribe(GetVerifiedSessionId());
        Connector.SendMessage(balanceRecordsSubscribe.ToString());
    }

    public void UnsubscribeBalance()
    {
        var balanceRecordsStop = new BalanceRecordsStop();
        Connector.SendMessage(balanceRecordsStop.ToString());
    }

    public void SubscribeTradeStatus()
    {
        var tradeStatusRecordsSubscribe = new TradeStatusRecordsSubscribe(GetVerifiedSessionId());
        Connector.SendMessage(tradeStatusRecordsSubscribe.ToString());
    }

    public void UnsubscribeTradeStatus()
    {
        var tradeStatusRecordsStop = new TradeRecordsSubscribe(GetVerifiedSessionId());
        Connector.SendMessage(tradeStatusRecordsStop.ToString());
    }

    public void SubscribeProfits()
    {
        var profitsSubscribe = new ProfitsSubscribe(GetVerifiedSessionId());
        Connector.SendMessage(profitsSubscribe.ToString());
    }

    public void UnsubscribeProfits()
    {
        var profitsStop = new ProfitsStop();
        Connector.SendMessage(profitsStop.ToString());
    }

    public void SubscribeNews()
    {
        var newsSubscribe = new NewsSubscribe(GetVerifiedSessionId());
        Connector.SendMessage(newsSubscribe.ToString());
    }

    public void UnsubscribeNews()
    {
        var newsStop = new NewsStop();
        Connector.SendMessage(newsStop.ToString());
    }

    public void SubscribeKeepAlive()
    {
        var keepAliveSubscribe = new KeepAliveSubscribe(GetVerifiedSessionId());
        Connector.SendMessage(keepAliveSubscribe.ToString());
    }

    public void UnsubscribeKeepAlive()
    {
        var keepAliveStop = new KeepAliveStop();
        Connector.SendMessage(keepAliveStop.ToString());
    }

    public void SubscribeCandles(string symbol)
    {
        var candleRecordsSubscribe = new CandleRecordsSubscribe(symbol, GetVerifiedSessionId());
        Connector.SendMessage(candleRecordsSubscribe.ToString());
    }

    public void UnsubscribeCandles(string symbol)
    {
        var candleRecordsStop = new CandleRecordsStop(symbol);
        Connector.SendMessage(candleRecordsStop.ToString());
    }

    public async Task SubscribePriceAsync(string symbol, DateTimeOffset? minArrivalTime = null, int? maxLevel = null, CancellationToken cancellationToken = default)
    {
        var tickPricesSubscribe = new TickPricesSubscribe(symbol, GetVerifiedSessionId(), minArrivalTime, maxLevel);
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
        var tradeRecordsSubscribe = new TradeRecordsSubscribe(GetVerifiedSessionId());
        await Connector.SendMessageAsync(tradeRecordsSubscribe.ToString(), cancellationToken);
    }

    public async Task UnsubscribeTradesAsync(CancellationToken cancellationToken = default)
    {
        var tradeRecordsStop = new TradeRecordsStop();
        await Connector.SendMessageAsync(tradeRecordsStop.ToString(), cancellationToken);
    }

    public async Task SubscribeBalanceAsync(CancellationToken cancellationToken = default)
    {
        var balanceRecordsSubscribe = new BalanceRecordsSubscribe(GetVerifiedSessionId());
        await Connector.SendMessageAsync(balanceRecordsSubscribe.ToString(), cancellationToken);
    }

    public async Task UnsubscribeBalanceAsync(CancellationToken cancellationToken = default)
    {
        var balanceRecordsStop = new BalanceRecordsStop();
        await Connector.SendMessageAsync(balanceRecordsStop.ToString(), cancellationToken);
    }

    public async Task SubscribeTradeStatusAsync(CancellationToken cancellationToken = default)
    {
        var tradeStatusRecordsSubscribe = new TradeStatusRecordsSubscribe(GetVerifiedSessionId());
        await Connector.SendMessageAsync(tradeStatusRecordsSubscribe.ToString(), cancellationToken);
    }

    public async Task UnsubscribeTradeStatusAsync(CancellationToken cancellationToken = default)
    {
        var tradeStatusRecordsStop = new TradeStatusRecordsStop();
        await Connector.SendMessageAsync(tradeStatusRecordsStop.ToString(), cancellationToken);
    }

    public async Task SubscribeProfitsAsync(CancellationToken cancellationToken = default)
    {
        var profitsSubscribe = new ProfitsSubscribe(GetVerifiedSessionId());
        await Connector.SendMessageAsync(profitsSubscribe.ToString(), cancellationToken);
    }

    public async Task UnsubscribeProfitsAsync(CancellationToken cancellationToken = default)
    {
        var profitsStop = new ProfitsStop();
        await Connector.SendMessageAsync(profitsStop.ToString(), cancellationToken);
    }

    public async Task SubscribeNewsAsync(CancellationToken cancellationToken = default)
    {
        var newsSubscribe = new NewsSubscribe(GetVerifiedSessionId());
        await Connector.SendMessageAsync(newsSubscribe.ToString(), cancellationToken);
    }

    public async Task UnsubscribeNewsAsync(CancellationToken cancellationToken = default)
    {
        var newsStop = new NewsStop();
        await Connector.SendMessageAsync(newsStop.ToString(), cancellationToken);
    }

    public async Task SubscribeKeepAliveAsync(CancellationToken cancellationToken = default)
    {
        var keepAliveSubscribe = new KeepAliveSubscribe(GetVerifiedSessionId());
        await Connector.SendMessageAsync(keepAliveSubscribe.ToString(), cancellationToken);
    }

    public async Task UnsubscribeKeepAliveAsync(CancellationToken cancellationToken = default)
    {
        var keepAliveStop = new KeepAliveStop();
        await Connector.SendMessageAsync(keepAliveStop.ToString(), cancellationToken);
    }

    public async Task SubscribeCandlesAsync(string symbol, CancellationToken cancellationToken = default)
    {
        var candleRecordsSubscribe = new CandleRecordsSubscribe(symbol, GetVerifiedSessionId());
        await Connector.SendMessageAsync(candleRecordsSubscribe.ToString(), cancellationToken);
    }

    public async Task UnsubscribeCandlesAsync(string symbol, CancellationToken cancellationToken = default)
    {
        var candleRecordsStop = new CandleRecordsStop(symbol);
        await Connector.SendMessageAsync(candleRecordsStop.ToString(), cancellationToken);
    }

    private string GetVerifiedSessionId()
    {
        return StreamSessionId
            ?? throw new InvalidOperationException($"{nameof(StreamSessionId)} is null");
    }

    #endregion subscribe, unsubscribe

    protected virtual void OnStreamingErrorOccurred(Exception ex, string? dataType = null)
    {
        var args = new ExceptionEventArgs(ex, dataType);
        StreamingErrorOccurred?.Invoke(this, args);

        if (!args.Handled)
        {
            // If the exception was not handled, re-throw it
            throw new APICommunicationException($"Read streaming message of '{dataType}' type failed.", ex);
        }
    }

    /// <inheritdoc/>
    public void Disconnect() => Connector.Disconnect();

    /// <inheritdoc/>
    public Task DisconnectAsync(CancellationToken cancellationToken = default) => Connector.DisconnectAsync(cancellationToken);

    /// <inheritdoc/>
    public override string ToString() => $"{base.ToString()}, {StreamSessionId ?? "no session"}";

    private bool _disposed;

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                if (IsConnectorOwner && Connector is IDisposable disposableConnetor)
                {
                    disposableConnetor.Dispose();
                }
            }

            _disposed = true;
        }
    }

    ~StreamingApiConnector()
    {
        Dispose(false);
    }
}