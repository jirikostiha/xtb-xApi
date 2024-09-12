using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using xAPI.Codes;
using xAPI.Records;
using xAPI.Responses;
using xAPI.Sync;

namespace xAPI;

public interface IXApiClientBase
{
    event EventHandler<ServerEventArgs>? Connected;
    event EventHandler<ServerEventArgs>? Redirected;
    event EventHandler? Disconnected;

    string AccountId { get; }

    StreamingApiConnector Streaming { get; }
}

public interface IXApiClientAsync : IXApiClientBase
{
    Task ConnectAsync(Server endpoint, CancellationToken cancellationToken = default);

    Task DisconnectAsync(CancellationToken cancellationToken = default);

    Task<PingResponse> PingAsync(CancellationToken cancellationToken = default);

    Task<VersionResponse> GetVersionAsync(CancellationToken cancellationToken = default);

    Task<ServerTimeResponse> GetServerTimeAsync(CancellationToken cancellationToken = default);

    Task<LoginResponse> LoginAsync(Credentials credentials, CancellationToken cancellationToken = default);

    Task<LogoutResponse> LogoutAsync(CancellationToken cancellationToken = default);

    Task<CurrentUserDataResponse> GetCurrentUserDataAsync(CancellationToken cancellationToken = default);

    Task<CommissionDefResponse> GetCommissionDefAsync(string symbol, double? volume, CancellationToken cancellationToken = default);

    Task<MarginLevelResponse> GetMarginLevelAsync(CancellationToken cancellationToken = default);

    Task<ProfitCalculationResponse> GetProfitCalculationAsync(string symbol, double? volume, TRADE_OPERATION_TYPE tradeOperation, double? openPrice, double? closePrice, CancellationToken cancellationToken = default);

    Task<MarginTradeResponse> GetMarginTradeAsync(string symbol, double? volume, CancellationToken cancellationToken = default);

    Task<SymbolResponse> GetMarketInfoAsync(string symbol, CancellationToken cancellationToken = default);

    Task<AllSymbolsResponse> GetAllSymbolsAsync(CancellationToken cancellationToken = default);

    Task<SymbolResponse> GetSymbolAsync(string symbol, CancellationToken cancellationToken = default);

    Task<TradingHoursResponse> GetTradingHoursAsync(string[] symbols, CancellationToken cancellationToken = default);

    Task<TickPricesResponse> GetTickPricesAsync(string[] symbols, int level, DateTimeOffset? time = null, CancellationToken cancellationToken = default);

    Task<ChartRangeResponse> GetChartRangeAsync(ChartRangeInfoRecord rangeInfoRecord, CancellationToken cancellationToken = default);

    Task<ChartRangeResponse> GetChartRangeAsync(string symbol, PERIOD period, DateTimeOffset since, DateTimeOffset until, CancellationToken cancellationToken = default);

    Task<ChartRangeResponse> GetChartRangeAsync(string symbol, PERIOD period, DateTimeOffset since, int ticks, CancellationToken cancellationToken = default);

    Task<ChartLastResponse> GetChartLastAsync(ChartLastInfoRecord rangeInfoRecord, CancellationToken cancellationToken = default);

    Task<ChartLastResponse> GetChartLastAsync(string symbol, PERIOD period, DateTimeOffset since, CancellationToken cancellationToken = default);

    Task<TradesResponse> GetTradesAsync(bool openOnly, CancellationToken cancellationToken = default);

    Task<TradeTransactionResponse> GetTradeTransactionAsync(TradeTransInfoRecord tradeTransInfoRecord, CancellationToken cancellationToken = default);

    Task<TradeRecordsResponse> GetTradeRecordsAsync(LinkedList<long?> orders, CancellationToken cancellationToken = default);

    Task<TradesHistoryResponse> GetTradesHistoryAsync(DateTimeOffset? start, DateTimeOffset? end = null, CancellationToken cancellationToken = default);

    Task<CalendarResponse> GetCalendarAsync(CancellationToken cancellationToken = default);

    Task<NewsResponse> GetNewsAsync(DateTimeOffset? since, DateTimeOffset? until, CancellationToken cancellationToken = default);
}

public interface IXApiClientSync : IXApiClientBase
{
    void Connect(Server endpoint);

    void Disconnect();

    PingResponse Ping();

    VersionResponse GetVersion();

    ServerTimeResponse GetServerTime();

    LoginResponse Login(Credentials credentials);

    LogoutResponse Logout();

    CurrentUserDataResponse GetCurrentUserData();

    CommissionDefResponse GetCommissionDef(string symbol, double? volume);

    MarginLevelResponse GetMarginLevel();

    MarginTradeResponse GetMarginTrade(string symbol, double? volume);

    ProfitCalculationResponse GetProfitCalculation(string symbol, double? volume, TRADE_OPERATION_TYPE tradeOperation, double? openPrice, double? closePrice);

    SymbolResponse GetMarketInfo(string symbol);

    AllSymbolsResponse GetAllSymbols();

    SymbolResponse GetSymbol(string symbol);

    TradingHoursResponse GetTradingHours(string[] symbols);

    TickPricesResponse GetTickPrices(string[] symbols, int level, DateTimeOffset? time = null);

    ChartRangeResponse GetChartRange(ChartRangeInfoRecord rangeInfoRecord);

    ChartRangeResponse GetChartRange(string symbol, PERIOD period, DateTimeOffset since, DateTimeOffset until);

    ChartRangeResponse GetChartRange(string symbol, PERIOD period, DateTimeOffset since, int ticks);

    ChartLastResponse GetChartLast(ChartLastInfoRecord rangeInfoRecord);

    ChartLastResponse GetChartLast(string symbol, PERIOD period, DateTimeOffset since);

    TradesResponse GetTrades(bool openOnly);

    TradeTransactionResponse GetTradeTransaction(TradeTransInfoRecord tradeTransInfoRecord);

    TradeRecordsResponse GetTradeRecords(LinkedList<long?> orders);

    TradesHistoryResponse GetTradesHistory(DateTimeOffset? since, DateTimeOffset? until = null);

    CalendarResponse GetCalendar();

    NewsResponse GetNews(DateTimeOffset? since, DateTimeOffset? until = null);
}