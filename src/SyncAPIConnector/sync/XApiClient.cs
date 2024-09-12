using System;
using System.Threading;
using System.Threading.Tasks;
using xAPI.Codes;
using xAPI.Commands;
using xAPI.Records;
using xAPI.Responses;
using xAPI.Sync;

namespace xAPI;

/// <summary>
/// Xtb xapi client.
/// </summary>
public class XApiClient : IXApiClientSync, IXApiClientAsync, IDisposable
{
    private Credentials? _credentials;

    #region Events
    public event EventHandler<ServerEventArgs>? Connected
    {
        add { ApiConnector.Connected += value; }
        remove { ApiConnector.Connected -= value; }
    }

    public event EventHandler<ServerEventArgs>? Redirected
    {
        add { ApiConnector.Redirected += value; }
        remove { ApiConnector.Redirected -= value; }
    }

    public event EventHandler? Disconnected
    {
        add { ApiConnector.Disconnected += value; }
        remove { ApiConnector.Disconnected -= value; }
    }
    #endregion

    public ApiConnector ApiConnector { get; set; }

    public StreamingApiConnector? Streaming => ApiConnector?.Streaming;

    public string AccountId => _credentials?.Login ?? string.Empty;

    public void Connect(Server endpoint)
    {
        ApiConnector = new ApiConnector(endpoint);
    }

    public Task ConnectAsync(Server endpoint, CancellationToken cancellationToken = default)
    {
        ApiConnector = new ApiConnector(endpoint);
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
    public Task<ChartRangeResponse> GetChartRangeAsync(string symbol, PERIOD period, DateTimeOffset since, DateTimeOffset until, int ticks, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteChartRangeCommandAsync(ApiConnector, symbol, period, since, until, ticks, cancellationToken);

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

    public CalendarResponse GetCalendar() => APICommandFactory.ExecuteCalendarCommand(ApiConnector);
    public Task<CalendarResponse> GetCalendarAsync(CancellationToken cancellationToken = default)
         => APICommandFactory.ExecuteCalendarCommandAsync(ApiConnector, cancellationToken);

    public NewsResponse GetNews(DateTimeOffset? since, DateTimeOffset? until)
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