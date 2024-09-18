using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Xtb.XApi.Codes;
using Xtb.XApi.Commands;
using Xtb.XApi.Records;
using Xtb.XApi.Responses;

namespace Xtb.XApi;

/// <summary>
/// Xtb xapi client.
/// </summary>
public class XApiClient : IXApiClientSync, IXApiClientAsync, IDisposable
{
    private Credentials? _credentials;

    /// <summary>
    /// Creates new instance.
    /// </summary>
    /// <param name="address">Endpoint address.</param>
    /// <param name="requestingPort">Port for requesting data.</param>
    /// <param name="streamingPort">Port for streaming data.</param>
    /// <param name="streamingListener">Streaming listener.</param>
    public XApiClient(string address, int requestingPort, int streamingPort, IStreamingListener? streamingListener = null)
        : this(
              new IPEndPoint(IPAddress.Parse(address), requestingPort),
              new IPEndPoint(IPAddress.Parse(address), streamingPort),
              streamingListener)
    {
    }

    /// <summary>
    /// Creates new instance.
    /// </summary>
    /// <param name="endpoint">Endpoint for requesting data.</param>
    /// <param name="streamingEndpoint">Endpoint for streaming data.</param>
    /// <param name="streamingListener">Streaming listener.</param>
    public XApiClient(IPEndPoint endpoint, IPEndPoint streamingEndpoint, IStreamingListener? streamingListener = null)
        : this(new ApiConnector(endpoint, streamingEndpoint, streamingListener))
    {
    }

    public XApiClient(ApiConnector apiConnector)
    {
        ApiConnector = apiConnector;
    }

    #region Events

    public event EventHandler<EndpointEventArgs>? Connected
    {
        add { ApiConnector.Connected += value; }
        remove { ApiConnector.Connected -= value; }
    }

    public event EventHandler<EndpointEventArgs>? Redirected
    {
        add { ApiConnector.Redirected += value; }
        remove { ApiConnector.Redirected -= value; }
    }

    public event EventHandler? Disconnected
    {
        add { ApiConnector.Disconnected += value; }
        remove { ApiConnector.Disconnected -= value; }
    }

    public event EventHandler<MessageEventArgs>? MessageReceived
    {
        add { ApiConnector.MessageReceived += value; }
        remove { ApiConnector.MessageReceived -= value; }
    }

    public event EventHandler<MessageEventArgs>? MessageSent
    {
        add { ApiConnector.MessageSent += value; }
        remove { ApiConnector.MessageSent -= value; }
    }

    #endregion Events

    public ApiConnector ApiConnector { get; set; }

    public StreamingApiConnector? Streaming => ApiConnector.Streaming;

    public string AccountId => _credentials?.Login ?? string.Empty;

    public void Connect()
    {
        ApiConnector.Connect();
    }

    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        await ApiConnector.ConnectAsync(true, cancellationToken);
    }

    public void Disconnect()
    {
        ApiConnector.Disconnect();
    }

    public Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        ApiConnector.Disconnect();
        return Task.CompletedTask;
    }

    public PingResponse Ping() => APICommandFactory.ExecutePingCommand(ApiConnector);

    public Task<PingResponse> PingAsync(CancellationToken cancellationToken = default)
        => APICommandFactory.ExecutePingCommandAsync(ApiConnector, cancellationToken);

    public VersionResponse GetVersion() => APICommandFactory.ExecuteVersionCommand(ApiConnector);

    public Task<VersionResponse> GetVersionAsync(CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteVersionCommandAsync(ApiConnector, cancellationToken);

    public ServerTimeResponse GetServerTime() => APICommandFactory.ExecuteServerTimeCommand(ApiConnector);

    public Task<ServerTimeResponse> GetServerTimeAsync(CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteServerTimeCommandAsync(ApiConnector, cancellationToken);

    public LoginResponse Login(Credentials credentials)
    {
        _credentials = credentials;
        return APICommandFactory.ExecuteLoginCommand(ApiConnector, credentials);
    }

    public Task<LoginResponse> LoginAsync(Credentials credentials, CancellationToken cancellationToken = default)
    {
        _credentials = credentials;
        return APICommandFactory.ExecuteLoginCommandAsync(ApiConnector, credentials, cancellationToken);
    }

    public LogoutResponse Logout() => APICommandFactory.ExecuteLogoutCommand(ApiConnector);

    public Task<LogoutResponse> LogoutAsync(CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteLogoutCommandAsync(ApiConnector, cancellationToken);

    public CurrentUserDataResponse GetCurrentUserData() => APICommandFactory.ExecuteCurrentUserDataCommand(ApiConnector);

    public Task<CurrentUserDataResponse> GetCurrentUserDataAsync(CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteCurrentUserDataCommandAsync(ApiConnector, cancellationToken);

    public CommissionDefResponse GetCommissionDef(string symbol, double? volume)
        => APICommandFactory.ExecuteCommissionDefCommand(ApiConnector, symbol, volume);

    public Task<CommissionDefResponse> GetCommissionDefAsync(string symbol, double? volume, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteCommissionDefCommandAsync(ApiConnector, symbol, volume, cancellationToken);

    public MarginLevelResponse GetMarginLevel() => APICommandFactory.ExecuteMarginLevelCommand(ApiConnector);

    public Task<MarginLevelResponse> GetMarginLevelAsync(CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteMarginLevelCommandAsync(ApiConnector, cancellationToken);

    public MarginTradeResponse GetMarginTrade(string symbol, double? volume)
        => APICommandFactory.ExecuteMarginTradeCommand(ApiConnector, symbol, volume);

    public Task<MarginTradeResponse> GetMarginTradeAsync(string symbol, double? volume, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteMarginTradeCommandAsync(ApiConnector, symbol, volume, cancellationToken);

    public ProfitCalculationResponse GetProfitCalculation(string symbol, double? volume, TRADE_OPERATION_TYPE tradeOperation, double? openPrice, double? closePrice)
        => APICommandFactory.ExecuteProfitCalculationCommand(ApiConnector, symbol, volume, tradeOperation, openPrice, closePrice);

    public Task<ProfitCalculationResponse> GetProfitCalculationAsync(string symbol, double? volume, TRADE_OPERATION_TYPE tradeOperation, double? openPrice, double? closePrice, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteProfitCalculationCommandAsync(ApiConnector, symbol, volume, tradeOperation, openPrice, closePrice, cancellationToken);

    public SymbolResponse GetMarketInfo(string symbol) => APICommandFactory.ExecuteSymbolCommand(ApiConnector, symbol);

    public Task<SymbolResponse> GetMarketInfoAsync(string symbol, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteSymbolCommandAsync(ApiConnector, symbol, cancellationToken);

    public AllSymbolsResponse GetAllSymbols() => APICommandFactory.ExecuteAllSymbolsCommand(ApiConnector);

    public Task<AllSymbolsResponse> GetAllSymbolsAsync(CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteAllSymbolsCommandAsync(ApiConnector, cancellationToken);

    public SymbolResponse GetSymbol(string symbol) => APICommandFactory.ExecuteSymbolCommand(ApiConnector, symbol);

    public Task<SymbolResponse> GetSymbolAsync(string symbol, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteSymbolCommandAsync(ApiConnector, symbol, cancellationToken);

    public TradingHoursResponse GetTradingHours(string[] symbols) => APICommandFactory.ExecuteTradingHoursCommand(ApiConnector, symbols);

    public Task<TradingHoursResponse> GetTradingHoursAsync(string[] symbols, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteTradingHoursCommandAsync(ApiConnector, symbols, cancellationToken);

    public TickPricesResponse GetTickPrices(string[] symbols, int level, DateTimeOffset? time = null)
        => APICommandFactory.ExecuteTickPricesCommand(ApiConnector, symbols, level, time);

    public Task<TickPricesResponse> GetTickPricesAsync(string[] symbols, int level, DateTimeOffset? time = null, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteTickPricesCommandAsync(ApiConnector, symbols, level, time, cancellationToken);

    public ChartRangeResponse GetChartRange(ChartRangeInfoRecord rangeInfoRecord)
        => APICommandFactory.ExecuteChartRangeCommand(ApiConnector, rangeInfoRecord);

    public Task<ChartRangeResponse> GetChartRangeAsync(ChartRangeInfoRecord rangeInfoRecord, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteChartRangeCommandAsync(ApiConnector, rangeInfoRecord, cancellationToken);

    public ChartRangeResponse GetChartRange(string symbol, PERIOD period, DateTimeOffset since, DateTimeOffset until)
        => APICommandFactory.ExecuteChartRangeCommand(ApiConnector, symbol, period, since, until, 0);

    public Task<ChartRangeResponse> GetChartRangeAsync(string symbol, PERIOD period, DateTimeOffset since, DateTimeOffset until, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteChartRangeCommandAsync(ApiConnector, symbol, period, since, until, 0, cancellationToken);

    public ChartRangeResponse GetChartRange(string symbol, PERIOD period, DateTimeOffset since, int ticks)
        => APICommandFactory.ExecuteChartRangeCommand(ApiConnector, symbol, period, since, default, ticks);

    public Task<ChartRangeResponse> GetChartRangeAsync(string symbol, PERIOD period, DateTimeOffset since, int ticks, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteChartRangeCommandAsync(ApiConnector, symbol, period, since, default, ticks, cancellationToken);

    public ChartLastResponse GetChartLast(ChartLastInfoRecord rangeInfoRecord)
        => APICommandFactory.ExecuteChartLastCommand(ApiConnector, rangeInfoRecord);

    public Task<ChartLastResponse> GetChartLastAsync(ChartLastInfoRecord rangeInfoRecord, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteChartLastCommandAsync(ApiConnector, rangeInfoRecord, cancellationToken);

    public ChartLastResponse GetChartLast(string symbol, PERIOD period, DateTimeOffset since)
        => APICommandFactory.ExecuteChartLastCommand(ApiConnector, symbol, period, since);

    public Task<ChartLastResponse> GetChartLastAsync(string symbol, PERIOD period, DateTimeOffset since, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteChartLastCommandAsync(ApiConnector, symbol, period, since, cancellationToken);

    public TradesResponse GetTrades(bool openOnly) => APICommandFactory.ExecuteTradesCommand(ApiConnector, openOnly);

    public Task<TradesResponse> GetTradesAsync(bool openOnly, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteTradesCommandAsync(ApiConnector, openOnly, cancellationToken);

    public TradeTransactionResponse GetTradeTransaction(TradeTransInfoRecord tradeTransInfoRecord)
        => APICommandFactory.ExecuteTradeTransactionCommand(ApiConnector, tradeTransInfoRecord);

    public Task<TradeTransactionResponse> GetTradeTransactionAsync(TradeTransInfoRecord tradeTransInfoRecord, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteTradeTransactionCommandAsync(ApiConnector, tradeTransInfoRecord, cancellationToken);

    public TradeRecordsResponse GetTradeRecords(LinkedList<long?> orders)
        => APICommandFactory.ExecuteTradeRecordsCommand(ApiConnector, orders);

    public Task<TradeRecordsResponse> GetTradeRecordsAsync(LinkedList<long?> orders, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteTradeRecordsCommandAsync(ApiConnector, orders, cancellationToken);

    public TradesHistoryResponse GetTradesHistory(DateTimeOffset? start, DateTimeOffset? end = null)
        => APICommandFactory.ExecuteTradesHistoryCommand(ApiConnector, start, end);

    public Task<TradesHistoryResponse> GetTradesHistoryAsync(DateTimeOffset? start, DateTimeOffset? end = null, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteTradesHistoryCommandAsync(ApiConnector, start, end, cancellationToken);

    public CalendarResponse GetCalendar() => APICommandFactory.ExecuteCalendarCommand(ApiConnector);

    public Task<CalendarResponse> GetCalendarAsync(CancellationToken cancellationToken = default)
         => APICommandFactory.ExecuteCalendarCommandAsync(ApiConnector, cancellationToken);

    public NewsResponse GetNews(DateTimeOffset? since, DateTimeOffset? until = null)
        => APICommandFactory.ExecuteNewsCommand(ApiConnector, since, until);

    public Task<NewsResponse> GetNewsAsync(DateTimeOffset? since, DateTimeOffset? until, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteNewsCommandAsync(ApiConnector, since, until, cancellationToken);

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            ApiConnector?.Dispose();
        }
    }
}