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
/// Represents the Xtb XApi client for handling connections and performing various API requests.
/// </summary>
public class XApiClient : IXApiClient, IDisposable
{
    /// <summary>
    /// Creates a new <see cref="XApiClient"/> instance using the provided endpoint address and ports.
    /// </summary>
    /// <param name="address">The endpoint address for API requests.</param>
    /// <param name="requestingPort">The port used for requesting data.</param>
    /// <param name="streamingPort">The port used for streaming data.</param>
    /// <param name="streamingListener">Optional streaming listener for handling streamed data.</param>
    /// <returns>A new instance of <see cref="XApiClient"/>.</returns>
    public static XApiClient Create(string address, int requestingPort, int streamingPort, IStreamingListener? streamingListener = null)
    {
        var requestingEndpoint = new IPEndPoint(IPAddress.Parse(address), requestingPort);
        var streamingEndpoint = new IPEndPoint(IPAddress.Parse(address), streamingPort);

        return Create(requestingEndpoint, streamingEndpoint, streamingListener);
    }

    /// <summary>
    /// Creates a new <see cref="XApiClient"/> instance using the provided endpoints.
    /// </summary>
    /// <param name="requestingEndpoint">The endpoint for requesting data.</param>
    /// <param name="streamingEndpoint">The endpoint for streaming data.</param>
    /// <param name="streamingListener">Optional streaming listener for handling streamed data.</param>
    /// <returns>A new instance of <see cref="XApiClient"/>.</returns>
    public static XApiClient Create(IPEndPoint requestingEndpoint, IPEndPoint streamingEndpoint, IStreamingListener? streamingListener = null)
    {
        var apiConnector = ApiConnector.Create(requestingEndpoint, streamingEndpoint, streamingListener);

        return new XApiClient(apiConnector)
        {
            IsApiConnectorOwner = true
        };
    }

    private Credentials? _credentials;

    /// Create a new instance.
    /// <summary>
    /// Create a new instance.
    /// </summary>
    /// <param name="apiConnector">An instance of <see cref="ApiConnector"/> to manage the connection.</param>
    public XApiClient(ApiConnector apiConnector)
    {
        ApiConnector = apiConnector;
        IsApiConnectorOwner = false;
    }

    #region Events

    /// <summary>
    /// Occurs when the client is connected to the API.
    /// </summary>
    public event EventHandler<EndpointEventArgs>? Connected
    {
        add { ApiConnector.Connected += value; }
        remove { ApiConnector.Connected -= value; }
    }

    /// <summary>
    /// Occurs when the client is disconnected from the API.
    /// </summary>
    public event EventHandler? Disconnected
    {
        add { ApiConnector.Disconnected += value; }
        remove { ApiConnector.Disconnected -= value; }
    }

    #endregion Events

    /// <summary>
    /// Gets the API connector used by the client.
    /// </summary>
    public ApiConnector ApiConnector { get; }

    /// <summary>
    /// Gets a value indicating whether the client owns the API connector.
    /// </summary>
    internal bool IsApiConnectorOwner { get; init; }

    /// <summary>
    /// Gets the streaming API connector for handling streaming data.
    /// </summary>
    public StreamingApiConnector StreamingConnector => ApiConnector.Streaming;

    /// <summary>
    /// Gets the account ID after a successful login.
    /// </summary>
    public string? AccountId => _credentials?.Login;

    /// <inheritdoc/>
    public IPEndPoint RequestingEndpoint => ApiConnector.Endpoint;

    /// <inheritdoc/>
    public IPEndPoint StreamingEndpoint => StreamingConnector.Endpoint;

    /// <inheritdoc/>
    public void Connect()
    {
        ApiConnector.Connect();
    }

    /// <inheritdoc/>
    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        await ApiConnector.ConnectAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public void Disconnect()
    {
        ApiConnector.Disconnect();
    }

    /// <inheritdoc/>
    public Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        return ApiConnector.DisconnectAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public PingResponse Ping() => APICommandFactory.ExecutePingCommand(ApiConnector);

    /// <inheritdoc/>
    public Task<PingResponse> PingAsync(CancellationToken cancellationToken = default)
        => APICommandFactory.ExecutePingCommandAsync(ApiConnector, cancellationToken);

    /// <inheritdoc/>
    public VersionResponse GetVersion()
        => APICommandFactory.ExecuteVersionCommand(ApiConnector);

    /// <inheritdoc/>
    public Task<VersionResponse> GetVersionAsync(CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteVersionCommandAsync(ApiConnector, cancellationToken);

    /// <inheritdoc/>
    public ServerTimeResponse GetServerTime()
        => APICommandFactory.ExecuteServerTimeCommand(ApiConnector);

    /// <inheritdoc/>
    public Task<ServerTimeResponse> GetServerTimeAsync(CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteServerTimeCommandAsync(ApiConnector, cancellationToken);

    /// <inheritdoc/>
    public LoginResponse Login(Credentials credentials)
    {
        _credentials = credentials;
        return APICommandFactory.ExecuteLoginCommand(ApiConnector, credentials);
    }

    /// <inheritdoc/>
    public LoginResponse Login(string userId, string password, string? appId = null, string? appName = null)
        => Login(new Credentials(userId, password, appId, appName));

    /// <inheritdoc/>
    public Task<LoginResponse> LoginAsync(Credentials credentials, CancellationToken cancellationToken = default)
    {
        _credentials = credentials;
        return APICommandFactory.ExecuteLoginCommandAsync(ApiConnector, credentials, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<LoginResponse> LoginAsync(string userId, string password, string? appId = null, string? appName = null, CancellationToken cancellationToken = default)
        => LoginAsync(new Credentials(userId, password, appId, appName), cancellationToken);

    /// <inheritdoc/>
    public LogoutResponse Logout() => APICommandFactory.ExecuteLogoutCommand(ApiConnector);

    /// <inheritdoc/>
    public Task<LogoutResponse> LogoutAsync(CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteLogoutCommandAsync(ApiConnector, cancellationToken);

    /// <inheritdoc/>
    public CurrentUserDataResponse GetCurrentUserData() => APICommandFactory.ExecuteCurrentUserDataCommand(ApiConnector);

    /// <inheritdoc/>
    public Task<CurrentUserDataResponse> GetCurrentUserDataAsync(CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteCurrentUserDataCommandAsync(ApiConnector, cancellationToken);

    /// <inheritdoc/>
    public CommissionDefResponse GetCommissionDef(string symbol, double? volume)
        => APICommandFactory.ExecuteCommissionDefCommand(ApiConnector, symbol, volume);

    /// <inheritdoc/>
    public Task<CommissionDefResponse> GetCommissionDefAsync(string symbol, double? volume, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteCommissionDefCommandAsync(ApiConnector, symbol, volume, cancellationToken);

    /// <inheritdoc/>
    public MarginLevelResponse GetMarginLevel() => APICommandFactory.ExecuteMarginLevelCommand(ApiConnector);

    /// <inheritdoc/>
    public Task<MarginLevelResponse> GetMarginLevelAsync(CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteMarginLevelCommandAsync(ApiConnector, cancellationToken);

    /// <inheritdoc/>
    public MarginTradeResponse GetMarginTrade(string symbol, double? volume)
        => APICommandFactory.ExecuteMarginTradeCommand(ApiConnector, symbol, volume);

    /// <inheritdoc/>
    public Task<MarginTradeResponse> GetMarginTradeAsync(string symbol, double? volume, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteMarginTradeCommandAsync(ApiConnector, symbol, volume, cancellationToken);

    /// <inheritdoc/>
    public ProfitCalculationResponse GetProfitCalculation(string symbol, double? volume, TRADE_OPERATION_TYPE tradeOperation, double? openPrice, double? closePrice)
        => APICommandFactory.ExecuteProfitCalculationCommand(ApiConnector, symbol, volume, tradeOperation, openPrice, closePrice);

    /// <inheritdoc/>
    public Task<ProfitCalculationResponse> GetProfitCalculationAsync(string symbol, double? volume, TRADE_OPERATION_TYPE tradeOperation, double? openPrice, double? closePrice, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteProfitCalculationCommandAsync(ApiConnector, symbol, volume, tradeOperation, openPrice, closePrice, cancellationToken);

    /// <inheritdoc/>
    public SymbolResponse GetMarketInfo(string symbol) => APICommandFactory.ExecuteSymbolCommand(ApiConnector, symbol);

    /// <inheritdoc/>
    public Task<SymbolResponse> GetMarketInfoAsync(string symbol, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteSymbolCommandAsync(ApiConnector, symbol, cancellationToken);

    /// <inheritdoc/>
    public AllSymbolsResponse GetAllSymbols() => APICommandFactory.ExecuteAllSymbolsCommand(ApiConnector);

    /// <inheritdoc/>
    public Task<AllSymbolsResponse> GetAllSymbolsAsync(CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteAllSymbolsCommandAsync(ApiConnector, cancellationToken);

    /// <inheritdoc/>
    public SymbolResponse GetSymbol(string symbol) => APICommandFactory.ExecuteSymbolCommand(ApiConnector, symbol);

    /// <inheritdoc/>
    public Task<SymbolResponse> GetSymbolAsync(string symbol, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteSymbolCommandAsync(ApiConnector, symbol, cancellationToken);

    /// <inheritdoc/>
    public TradingHoursResponse GetTradingHours(string[] symbols) => APICommandFactory.ExecuteTradingHoursCommand(ApiConnector, symbols);

    /// <inheritdoc/>
    public Task<TradingHoursResponse> GetTradingHoursAsync(string[] symbols, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteTradingHoursCommandAsync(ApiConnector, symbols, cancellationToken);

    /// <inheritdoc/>
    public TickPricesResponse GetTickPrices(string[] symbols, int level, DateTimeOffset? time = null)
        => APICommandFactory.ExecuteTickPricesCommand(ApiConnector, symbols, level, time);

    /// <inheritdoc/>
    public Task<TickPricesResponse> GetTickPricesAsync(string[] symbols, int level, DateTimeOffset? time = null, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteTickPricesCommandAsync(ApiConnector, symbols, level, time, cancellationToken);

    /// <inheritdoc/>
    public ChartRangeResponse GetChartRange(ChartRangeInfoRecord rangeInfoRecord)
        => APICommandFactory.ExecuteChartRangeCommand(ApiConnector, rangeInfoRecord);

    /// <inheritdoc/>
    public ChartRangeResponse GetChartRange(string symbol, PERIOD period, DateTimeOffset since, DateTimeOffset until)
        => APICommandFactory.ExecuteChartRangeCommand(ApiConnector, symbol, period, since, until, 0);

    /// <inheritdoc/>
    public ChartRangeResponse GetChartRange(string symbol, PERIOD period, DateTimeOffset since, int ticks)
        => APICommandFactory.ExecuteChartRangeCommand(ApiConnector, symbol, period, since, default, ticks);

    /// <inheritdoc/>
    public Task<ChartRangeResponse> GetChartRangeAsync(ChartRangeInfoRecord rangeInfoRecord, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteChartRangeCommandAsync(ApiConnector, rangeInfoRecord, cancellationToken);

    /// <inheritdoc/>
    public Task<ChartRangeResponse> GetChartRangeAsync(string symbol, PERIOD period, DateTimeOffset since, DateTimeOffset until, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteChartRangeCommandAsync(ApiConnector, symbol, period, since, until, 0, cancellationToken);

    /// <inheritdoc/>
    public Task<ChartRangeResponse> GetChartRangeAsync(string symbol, PERIOD period, DateTimeOffset since, int ticks, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteChartRangeCommandAsync(ApiConnector, symbol, period, since, default, ticks, cancellationToken);

    /// <inheritdoc/>
    public ChartLastResponse GetChartLast(ChartLastInfoRecord rangeInfoRecord)
        => APICommandFactory.ExecuteChartLastCommand(ApiConnector, rangeInfoRecord);

    /// <inheritdoc/>
    public ChartLastResponse GetChartLast(string symbol, PERIOD period, DateTimeOffset since)
        => APICommandFactory.ExecuteChartLastCommand(ApiConnector, symbol, period, since);

    /// <inheritdoc/>
    public Task<ChartLastResponse> GetChartLastAsync(ChartLastInfoRecord rangeInfoRecord, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteChartLastCommandAsync(ApiConnector, rangeInfoRecord, cancellationToken);

    /// <inheritdoc/>
    public Task<ChartLastResponse> GetChartLastAsync(string symbol, PERIOD period, DateTimeOffset since, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteChartLastCommandAsync(ApiConnector, symbol, period, since, cancellationToken);

    /// <inheritdoc/>
    public TradesResponse GetTrades(bool openOnly) => APICommandFactory.ExecuteTradesCommand(ApiConnector, openOnly);

    /// <inheritdoc/>
    public Task<TradesResponse> GetTradesAsync(bool openOnly, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteTradesCommandAsync(ApiConnector, openOnly, cancellationToken);

    /// <inheritdoc/>
    public TradeTransactionResponse SetTradeTransaction(TradeTransInfoRecord tradeTransInfoRecord)
        => APICommandFactory.ExecuteTradeTransactionCommand(ApiConnector, tradeTransInfoRecord);

    /// <inheritdoc/>
    public Task<TradeTransactionResponse> SetTradeTransactionAsync(TradeTransInfoRecord tradeTransInfoRecord, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteTradeTransactionCommandAsync(ApiConnector, tradeTransInfoRecord, cancellationToken);

    /// <inheritdoc/>
    public TradeRecordsResponse GetTradeRecords(long?[] orderIds)
        => APICommandFactory.ExecuteTradeRecordsCommand(ApiConnector, orderIds);

    /// <inheritdoc/>
    public Task<TradeRecordsResponse> GetTradeRecordsAsync(long?[] orderIds, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteTradeRecordsCommandAsync(ApiConnector, orderIds, cancellationToken);

    /// <inheritdoc/>
    public TradesHistoryResponse GetTradesHistory(DateTimeOffset? since, DateTimeOffset? until = null)
        => APICommandFactory.ExecuteTradesHistoryCommand(ApiConnector, since, until);

    /// <inheritdoc/>
    public Task<TradesHistoryResponse> GetTradesHistoryAsync(DateTimeOffset? since, DateTimeOffset? until = null, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteTradesHistoryCommandAsync(ApiConnector, since, until, cancellationToken);

    /// <inheritdoc/>
    public CalendarResponse GetCalendar() => APICommandFactory.ExecuteCalendarCommand(ApiConnector);

    /// <inheritdoc/>
    public Task<CalendarResponse> GetCalendarAsync(CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteCalendarCommandAsync(ApiConnector, cancellationToken);

    /// <inheritdoc/>
    public NewsResponse GetNews(DateTimeOffset? since, DateTimeOffset? until = null)
        => APICommandFactory.ExecuteNewsCommand(ApiConnector, since, until);

    /// <inheritdoc/>
    public Task<NewsResponse> GetNewsAsync(DateTimeOffset? since, DateTimeOffset? until, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteNewsCommandAsync(ApiConnector, since, until, cancellationToken);

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
                if (IsApiConnectorOwner)
                    ApiConnector?.Dispose();
            }

            _disposed = true;
        }
    }

    ~XApiClient()
    {
        Dispose(false);
    }
}