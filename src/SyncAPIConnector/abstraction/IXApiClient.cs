using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xtb.XApi.Codes;
using Xtb.XApi.Records;
using Xtb.XApi.Responses;

namespace Xtb.XApi;

/// <summary>
/// Xtb XApi client for handling connections and performing API requests.
/// </summary>
public interface IXApiClient : IXApiClientSync, IXApiClientAsync
{ }

/// <summary>
/// Asynchronous version of xtb XApi client.
/// </summary>
public interface IXApiClientAsync : IXApiClientBase
{
    Task ConnectAsync(CancellationToken cancellationToken = default);

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

    Task<TradesHistoryResponse> GetTradesHistoryAsync(DateTimeOffset? since, DateTimeOffset? until = null, CancellationToken cancellationToken = default);

    Task<CalendarResponse> GetCalendarAsync(CancellationToken cancellationToken = default);

    Task<NewsResponse> GetNewsAsync(DateTimeOffset? since, DateTimeOffset? until, CancellationToken cancellationToken = default);
}


/// <summary>
/// Synchronous version of xtb XApi client.
/// </summary>
public interface IXApiClientSync : IXApiClientBase
{
    /// <summary>
    /// Connects the client to the API.
    /// </summary>
    void Connect();

    /// <summary>
    /// Disconnects the client from the API.
    /// </summary>
    void Disconnect();


    /// <summary>
    /// Sends a ping command to the API.
    /// </summary>
    /// <returns>The response from the ping command.</returns>
    PingResponse Ping();

    /// <summary>
    /// Retrieves the version of the API.
    /// </summary>
    /// <returns>The response containing the version information.</returns>
    VersionResponse GetVersion();

    /// <summary>
    /// Retrieves the current server time.
    /// </summary>
    /// <returns>The response containing the server time information.</returns>
    ServerTimeResponse GetServerTime();

    /// <summary>
    /// Logs in to the API using the specified credentials.
    /// </summary>
    /// <param name="credentials">The credentials used to log in.</param>
    /// <returns>The response from the login command.</returns>
    LoginResponse Login(Credentials credentials);

    /// <summary>
    /// Logs out from the API.
    /// </summary>
    /// <returns>The response from the logout command.</returns>
    LogoutResponse Logout();

    /// <summary>
    /// Retrieves the current user data from the API.
    /// </summary>
    /// <returns>The response containing the current user's data.</returns>
    CurrentUserDataResponse GetCurrentUserData();

    /// <summary>
    /// Retrieves commission definition for a given symbol and volume.
    /// </summary>
    /// <param name="symbol">The symbol for which the commission is requested.</param>
    /// <param name="volume">The volume to be traded.</param>
    /// <returns>The response containing commission details.</returns>
    CommissionDefResponse GetCommissionDef(string symbol, double? volume);

    /// <summary>
    /// Retrieves the margin level from the API.
    /// </summary>
    /// <returns>The response containing margin level details.</returns>
    MarginLevelResponse GetMarginLevel();

    /// <summary>
    /// Retrieves margin trade details for a specified symbol and volume.
    /// </summary>
    /// <param name="symbol">The symbol to retrieve margin trade details for.</param>
    /// <param name="volume">The volume to be traded.</param>
    /// <returns>The response containing margin trade details.</returns>
    MarginTradeResponse GetMarginTrade(string symbol, double? volume);

    /// <summary>
    /// Calculates the profit for a trade based on various parameters.
    /// </summary>
    /// <param name="symbol">The symbol for the trade.</param>
    /// <param name="volume">The volume to be traded.</param>
    /// <param name="tradeOperation">The type of trade operation (buy/sell).</param>
    /// <param name="openPrice">The opening price of the trade.</param>
    /// <param name="closePrice">The closing price of the trade.</param>
    /// <returns>The response containing the profit calculation details.</returns>
    ProfitCalculationResponse GetProfitCalculation(string symbol, double? volume, TRADE_OPERATION_TYPE tradeOperation, double? openPrice, double? closePrice);

    /// <summary>
    /// Retrieves market information for a given symbol.
    /// </summary>
    /// <param name="symbol">The symbol to retrieve market information for.</param>
    /// <returns>The response containing market information for the specified symbol.</returns>
    SymbolResponse GetMarketInfo(string symbol);

    /// <summary>
    /// Retrieves information for all symbols available on the API.
    /// </summary>
    /// <returns>The response containing information for all available symbols.</returns>
    AllSymbolsResponse GetAllSymbols();

    /// <summary>
    /// Retrieves symbol information for a specific symbol.
    /// </summary>
    /// <param name="symbol">The symbol to retrieve information for.</param>
    /// <returns>The response containing the symbol's information.</returns>
    SymbolResponse GetSymbol(string symbol);

    /// <summary>
    /// Retrieves trading hours information for a list of symbols.
    /// </summary>
    /// <param name="symbols">An array of symbols to retrieve trading hours for.</param>
    /// <returns>The response containing trading hours information.</returns>
    TradingHoursResponse GetTradingHours(string[] symbols);

    /// <summary>
    /// Retrieves tick prices for a given set of symbols.
    /// </summary>
    /// <param name="symbols">An array of symbols to retrieve tick prices for.</param>
    /// <param name="level">The level of detail for the tick prices.</param>
    /// <param name="time">Optional parameter to specify the time for retrieving tick prices.</param>
    /// <returns>The response containing tick prices.</returns>
    TickPricesResponse GetTickPrices(string[] symbols, int level, DateTimeOffset? time = null);

    /// <summary>
    /// Retrieves the chart range data for the specified chart range information.
    /// </summary>
    /// <param name="rangeInfoRecord">The chart range information.</param>
    /// <returns>The response containing the chart range data.</returns>
    ChartRangeResponse GetChartRange(ChartRangeInfoRecord rangeInfoRecord);

    /// <summary>
    /// Retrieves the chart range data for a specific symbol, period, and date range.
    /// </summary>
    /// <param name="symbol">The symbol for which to retrieve chart range data.</param>
    /// <param name="period">The period for the chart range.</param>
    /// <param name="since">The starting date and time for the chart range.</param>
    /// <param name="until">The ending date and time for the chart range.</param>
    /// <returns>The response containing the chart range data.</returns>
    ChartRangeResponse GetChartRange(string symbol, PERIOD period, DateTimeOffset since, DateTimeOffset until);

    /// <summary>
    /// Retrieves the chart range data for a specific symbol, period, and number of ticks.
    /// </summary>
    /// <param name="symbol">The symbol for which to retrieve chart range data.</param>
    /// <param name="period">The period for the chart range.</param>
    /// <param name="since">The starting date and time for the chart range.</param>
    /// <param name="ticks">The number of ticks to retrieve for the chart range.</param>
    /// <returns>The response containing the chart range data.</returns>
    ChartRangeResponse GetChartRange(string symbol, PERIOD period, DateTimeOffset since, int ticks);


    /// <summary>
    /// Retrieves the last chart data for the specified chart range information.
    /// </summary>
    /// <param name="rangeInfoRecord">The chart range information.</param>
    /// <returns>The response containing the last chart data.</returns>
    ChartLastResponse GetChartLast(ChartLastInfoRecord rangeInfoRecord);

    /// <summary>
    /// Retrieves the last chart data for a specific symbol and period since a specified date and time.
    /// </summary>
    /// <param name="symbol">The symbol for which to retrieve the last chart data.</param>
    /// <param name="period">The period for the chart data.</param>
    /// <param name="since">The starting date and time for the chart data.</param>
    /// <returns>The response containing the last chart data.</returns>
    ChartLastResponse GetChartLast(string symbol, PERIOD period, DateTimeOffset since);

    /// <summary>
    /// Retrieves the list of trades, filtered by whether they are open only or include closed trades.
    /// </summary>
    /// <param name="openOnly">If true, retrieves only open trades. If false, retrieves all trades.</param>
    /// <returns>The response containing the list of trades.</returns>
    TradesResponse GetTrades(bool openOnly);

    /// <summary>
    /// Retrieves a trade transaction based on the provided trade transaction information.
    /// </summary>
    /// <param name="tradeTransInfoRecord">The trade transaction information record.</param>
    /// <returns>The response containing the trade transaction data.</returns>
    TradeTransactionResponse GetTradeTransaction(TradeTransInfoRecord tradeTransInfoRecord);

    /// <summary>
    /// Retrieves trade records for the provided list of orders.
    /// </summary>
    /// <param name="orders">A linked list of order IDs for which to retrieve trade records.</param>
    /// <returns>The response containing the trade records.</returns>
    TradeRecordsResponse GetTradeRecords(LinkedList<long?> orders);

    /// <summary>
    /// Retrieves the trade history within a specified time range.
    /// </summary>
    /// <param name="since">The start date and time of the trade history period. If null, the history will start from the earliest available point.</param>
    /// <param name="until">The end date and time of the trade history period. If null, the history will end at the latest available point.</param>
    /// <returns>The response containing the trade history within the specified time range.</returns>
    TradesHistoryResponse GetTradesHistory(DateTimeOffset? since, DateTimeOffset? until = null);

    /// <summary>
    /// Retrieves the calendar events from the API.
    /// </summary>
    /// <returns>The response containing the calendar events.</returns>
    CalendarResponse GetCalendar();

    /// <summary>
    /// Retrieves news articles within a specified time range.
    /// </summary>
    /// <param name="since">The start date and time for retrieving news. If null, the news will be retrieved from the earliest available point.</param>
    /// <param name="until">The end date and time for retrieving news. If null, the news will be retrieved until the latest available point.</param>
    /// <returns>The response containing the news articles within the specified time range.</returns>
    NewsResponse GetNews(DateTimeOffset? since, DateTimeOffset? until = null);
}

public interface IXApiClientBase
{
    /// <summary>
    /// Occurs when the client is connected to the API.
    /// </summary>
    event EventHandler<EndpointEventArgs>? Connected;

    /// <summary>
    /// Occurs when the client is redirected to a new endpoint.
    /// </summary>
    event EventHandler<EndpointEventArgs>? Redirected;

    /// <summary>
    /// Occurs when the client is disconnected from the API.
    /// </summary>
    event EventHandler? Disconnected;

    /// <summary>
    /// Account id.
    /// </summary>
    string? AccountId { get; }

    /// <summary>
    /// Streaming api connector for handling streaming data.
    /// </summary>
    StreamingApiConnector Streaming { get; }
}
