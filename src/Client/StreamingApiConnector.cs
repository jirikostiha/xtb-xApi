﻿
using System;
using System.Net;
using System.Net.Sockets;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Xtb.XApi.Client.Model;

namespace Xtb.XApi.Client;

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
    /// Event raised when a command is being executed.
    /// </summary>
    public event EventHandler<CommandEventArgs>? CommandExecuting;

    /// <summary>
    /// Event raised when a streaming data is being received.
    /// </summary>
    public event EventHandler<DataReceivedEventArgs>? DataReceived;

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
    private async ValueTask ReadStreamMessageAsync(CancellationToken cancellationToken = default)
    {
        string? dataType = null;
        try
        {
            var message = await Connector.ReadMessageAsync(cancellationToken).ConfigureAwait(false)
                ?? throw new InvalidOperationException("Incoming streaming message is null.");

            var responseBody = JsonNode.Parse(message)
                ?? throw new InvalidOperationException("Result of incoming parsed streaming message is null.");

            dataType = (responseBody["command"]?.ToString())
                ?? throw new InvalidOperationException("Incoming streaming command is null.");

            var jsonSubnode = responseBody["data"]
                ?? throw new InvalidOperationException("Parsed json data object is null.");

            var jsonDataObject = jsonSubnode.AsObject();

            DataReceived?.Invoke(this, new(dataType, responseBody));

            if (dataType == StreamingDataType.TickPrices)
            {
                var tickRecord = new StreamingTickRecord();
                tickRecord.FieldsFromJsonObject(jsonDataObject);

                if (StreamingListener != null)
                    await StreamingListener.ReceiveTickRecordAsync(tickRecord, cancellationToken).ConfigureAwait(false);

                TickReceived?.Invoke(this, new(tickRecord));
            }
            else if (dataType == StreamingDataType.Trade)
            {
                var tradeRecord = new StreamingTradeRecord();
                tradeRecord.FieldsFromJsonObject(jsonDataObject);

                if (StreamingListener != null)
                    await StreamingListener.ReceiveTradeRecordAsync(tradeRecord, cancellationToken).ConfigureAwait(false);

                TradeReceived?.Invoke(this, new(tradeRecord));
            }
            else if (dataType == StreamingDataType.Balance)
            {
                var balanceRecord = new StreamingBalanceRecord();
                balanceRecord.FieldsFromJsonObject(jsonDataObject);

                if (StreamingListener != null)
                    await StreamingListener.ReceiveBalanceRecordAsync(balanceRecord, cancellationToken).ConfigureAwait(false);

                BalanceReceived?.Invoke(this, new(balanceRecord));
            }
            else if (dataType == StreamingDataType.TradeStatus)
            {
                var tradeStatusRecord = new StreamingTradeStatusRecord();
                tradeStatusRecord.FieldsFromJsonObject(jsonDataObject);

                if (StreamingListener != null)
                    await StreamingListener.ReceiveTradeStatusRecordAsync(tradeStatusRecord, cancellationToken).ConfigureAwait(false);

                TradeStatusReceived?.Invoke(this, new(tradeStatusRecord));
            }
            else if (dataType == StreamingDataType.Profit)
            {
                var profitRecord = new StreamingProfitRecord();
                profitRecord.FieldsFromJsonObject(jsonDataObject);

                if (StreamingListener != null)
                    await StreamingListener.ReceiveProfitRecordAsync(profitRecord, cancellationToken).ConfigureAwait(false);

                ProfitReceived?.Invoke(this, new(profitRecord));
            }
            else if (dataType == StreamingDataType.News)
            {
                var newsRecord = new StreamingNewsRecord();
                newsRecord.FieldsFromJsonObject(jsonDataObject);

                if (StreamingListener != null)
                    await StreamingListener.ReceiveNewsRecordAsync(newsRecord, cancellationToken).ConfigureAwait(false);

                NewsReceived?.Invoke(this, new(newsRecord));
            }
            else if (dataType == StreamingDataType.KeepAlive)
            {
                var keepAliveRecord = new StreamingKeepAliveRecord();
                keepAliveRecord.FieldsFromJsonObject(jsonDataObject);

                if (StreamingListener != null)
                    await StreamingListener.ReceiveKeepAliveRecordAsync(keepAliveRecord, cancellationToken).ConfigureAwait(false);

                KeepAliveReceived?.Invoke(this, new(keepAliveRecord));
            }
            else if (dataType == StreamingDataType.Candle)
            {
                var candleRecord = new StreamingCandleRecord();
                candleRecord.FieldsFromJsonObject(jsonDataObject);

                if (StreamingListener != null)
                    await StreamingListener.ReceiveCandleRecordAsync(candleRecord, cancellationToken).ConfigureAwait(false);

                CandleReceived?.Invoke(this, new(candleRecord));
            }
            else
            {
                throw new APICommunicationException($"Unknown streaming record received. command:'{dataType}'");
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
            OnStreamingErrorOccurred(ex, dataType);
        }
    }

    /// <summary>
    /// Executes given command without response.
    /// </summary>
    /// <param name="command">Command to execute</param>
    public void ExecuteCommand(ICommand command)
    {
        try
        {
            var request = command.ToString();

            CommandExecuting?.Invoke(this, new(command));
            Connector.SendMessage(request);
        }
        catch (Exception ex)
        {
            throw new APICommunicationException($"Problem with executing command:'{command.CommandName}'", ex);
        }
    }

    /// <summary>
    /// Executes given command without response.
    /// </summary>
    /// <param name="command">Command to execute</param>
    /// <param name="cancellationToken">Token to cancel operation.</param>
    public async Task ExecuteCommandAsync(ICommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = command.ToString();

            CommandExecuting?.Invoke(this, new(command));
            await Connector.SendMessageAsync(request, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            throw new APICommunicationException($"Problem with executing command:'{command.CommandName}'", ex);
        }
    }

    #region subscribe, unsubscribe

    public void SubscribePrice(string symbol, DateTimeOffset? minArrivalTime = null, int? maxLevel = null)
    {
        var tickPricesSubscribe = new TickPricesSubscribe(symbol, GetVerifiedSessionId(), minArrivalTime, maxLevel);
        ExecuteCommand(tickPricesSubscribe);
    }

    public void UnsubscribePrice(string symbol)
    {
        var tickPricesStop = new TickPricesStop(symbol);
        ExecuteCommand(tickPricesStop);
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
        ExecuteCommand(tradeRecordsSubscribe);
    }

    public void UnsubscribeTrades()
    {
        var tradeRecordsStop = new TradeRecordsStop();
        ExecuteCommand(tradeRecordsStop);
    }

    public void SubscribeBalance()
    {
        var balanceRecordsSubscribe = new BalanceRecordsSubscribe(GetVerifiedSessionId());
        ExecuteCommand(balanceRecordsSubscribe);
    }

    public void UnsubscribeBalance()
    {
        var balanceRecordsStop = new BalanceRecordsStop();
        ExecuteCommand(balanceRecordsStop);
    }

    public void SubscribeTradeStatus()
    {
        var tradeStatusRecordsSubscribe = new TradeStatusRecordsSubscribe(GetVerifiedSessionId());
        ExecuteCommand(tradeStatusRecordsSubscribe);
    }

    public void UnsubscribeTradeStatus()
    {
        var tradeStatusRecordsStop = new TradeRecordsSubscribe(GetVerifiedSessionId());
        ExecuteCommand(tradeStatusRecordsStop);
    }

    public void SubscribeProfits()
    {
        var profitsSubscribe = new ProfitsSubscribe(GetVerifiedSessionId());
        ExecuteCommand(profitsSubscribe);
    }

    public void UnsubscribeProfits()
    {
        var profitsStop = new ProfitsStop();
        ExecuteCommand(profitsStop);
    }

    public void SubscribeNews()
    {
        var newsSubscribe = new NewsSubscribe(GetVerifiedSessionId());
        ExecuteCommand(newsSubscribe);
    }

    public void UnsubscribeNews()
    {
        var newsStop = new NewsStop();
        ExecuteCommand(newsStop);
    }

    public void SubscribeKeepAlive()
    {
        var keepAliveSubscribe = new KeepAliveSubscribe(GetVerifiedSessionId());
        ExecuteCommand(keepAliveSubscribe);
    }

    public void UnsubscribeKeepAlive()
    {
        var keepAliveStop = new KeepAliveStop();
        ExecuteCommand(keepAliveStop);
    }

    public void SubscribeCandles(string symbol)
    {
        var candleRecordsSubscribe = new CandleRecordsSubscribe(symbol, GetVerifiedSessionId());
        ExecuteCommand(candleRecordsSubscribe);
    }

    public void UnsubscribeCandles(string symbol)
    {
        var candleRecordsStop = new CandleRecordsStop(symbol);
        ExecuteCommand(candleRecordsStop);
    }

    public async Task SubscribePriceAsync(string symbol, DateTimeOffset? minArrivalTime = null, int? maxLevel = null, CancellationToken cancellationToken = default)
    {
        var tickPricesSubscribe = new TickPricesSubscribe(symbol, GetVerifiedSessionId(), minArrivalTime, maxLevel);
        await ExecuteCommandAsync(tickPricesSubscribe, cancellationToken);
    }

    public async Task UnsubscribePriceAsync(string symbol, CancellationToken cancellationToken = default)
    {
        var tickPricesStop = new TickPricesStop(symbol);
        await ExecuteCommandAsync(tickPricesStop, cancellationToken);
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
        await ExecuteCommandAsync(tradeRecordsSubscribe, cancellationToken);
    }

    public async Task UnsubscribeTradesAsync(CancellationToken cancellationToken = default)
    {
        var tradeRecordsStop = new TradeRecordsStop();
        await ExecuteCommandAsync(tradeRecordsStop, cancellationToken);
    }

    public async Task SubscribeBalanceAsync(CancellationToken cancellationToken = default)
    {
        var balanceRecordsSubscribe = new BalanceRecordsSubscribe(GetVerifiedSessionId());
        await ExecuteCommandAsync(balanceRecordsSubscribe, cancellationToken);
    }

    public async Task UnsubscribeBalanceAsync(CancellationToken cancellationToken = default)
    {
        var balanceRecordsStop = new BalanceRecordsStop();
        await ExecuteCommandAsync(balanceRecordsStop, cancellationToken);
    }

    public async Task SubscribeTradeStatusAsync(CancellationToken cancellationToken = default)
    {
        var tradeStatusRecordsSubscribe = new TradeStatusRecordsSubscribe(GetVerifiedSessionId());
        await ExecuteCommandAsync(tradeStatusRecordsSubscribe, cancellationToken);
    }

    public async Task UnsubscribeTradeStatusAsync(CancellationToken cancellationToken = default)
    {
        var tradeStatusRecordsStop = new TradeStatusRecordsStop();
        await ExecuteCommandAsync(tradeStatusRecordsStop, cancellationToken);
    }

    public async Task SubscribeProfitsAsync(CancellationToken cancellationToken = default)
    {
        var profitsSubscribe = new ProfitsSubscribe(GetVerifiedSessionId());
        await ExecuteCommandAsync(profitsSubscribe, cancellationToken);
    }

    public async Task UnsubscribeProfitsAsync(CancellationToken cancellationToken = default)
    {
        var profitsStop = new ProfitsStop();
        await ExecuteCommandAsync(profitsStop, cancellationToken);
    }

    public async Task SubscribeNewsAsync(CancellationToken cancellationToken = default)
    {
        var newsSubscribe = new NewsSubscribe(GetVerifiedSessionId());
        await ExecuteCommandAsync(newsSubscribe, cancellationToken);
    }

    public async Task UnsubscribeNewsAsync(CancellationToken cancellationToken = default)
    {
        var newsStop = new NewsStop();
        await ExecuteCommandAsync(newsStop, cancellationToken);
    }

    public async Task SubscribeKeepAliveAsync(CancellationToken cancellationToken = default)
    {
        var keepAliveSubscribe = new KeepAliveSubscribe(GetVerifiedSessionId());
        await ExecuteCommandAsync(keepAliveSubscribe, cancellationToken);
    }

    public async Task UnsubscribeKeepAliveAsync(CancellationToken cancellationToken = default)
    {
        var keepAliveStop = new KeepAliveStop();
        await ExecuteCommandAsync(keepAliveStop, cancellationToken);
    }

    public async Task SubscribeCandlesAsync(string symbol, CancellationToken cancellationToken = default)
    {
        var candleRecordsSubscribe = new CandleRecordsSubscribe(symbol, GetVerifiedSessionId());
        await ExecuteCommandAsync(candleRecordsSubscribe, cancellationToken);
    }

    public async Task UnsubscribeCandlesAsync(string symbol, CancellationToken cancellationToken = default)
    {
        var candleRecordsStop = new CandleRecordsStop(symbol);
        await ExecuteCommandAsync(candleRecordsStop, cancellationToken);
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