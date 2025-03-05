using System;
using System.Collections.Generic;
using System.Net;
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
    /// <summary>
    /// Asynchronously connects the client to the API.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task ConnectAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously disconnects the client from the API.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DisconnectAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously sends a ping command to the API.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response from the ping command.</returns>
    Task<PingResponse> PingAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves the version of the API.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the version information.</returns>
    Task<VersionResponse> GetVersionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves the current server time.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the server time information.</returns>
    Task<ServerTimeResponse> GetServerTimeAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously logs in to the API using the specified credentials.
    /// </summary>
    /// <param name="credentials">The credentials used to log in.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response from the login command.</returns>
    Task<LoginResponse> LoginAsync(Credentials credentials, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously logs in to the API.
    /// </summary>
    /// <param name="userId">The user ID used to log in.</param>
    /// <param name="password">The password used to log in.</param>
    /// <param name="appId">Optional. The application ID for the login request.</param>
    /// <param name="appName">Optional. The application name for the login request.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    Task<LoginResponse> LoginAsync(string userId, string password, string? appId = null, string? appName = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously logs out from the API.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response from the logout command.</returns>
    Task<LogoutResponse> LogoutAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves the current user data from the API.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the current user's data.</returns>
    Task<CurrentUserDataResponse> GetCurrentUserDataAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves commission definition for a given symbol and volume.
    /// </summary>
    /// <param name="symbol">The symbol for which the commission is requested.</param>
    /// <param name="volume">The volume to be traded.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing commission details.</returns>
    Task<CommissionDefResponse> GetCommissionDefAsync(string symbol, double? volume, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves the margin level from the API.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing margin level details.</returns>
    Task<MarginLevelResponse> GetMarginLevelAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves margin trade details for a specified symbol and volume.
    /// </summary>
    /// <param name="symbol">The symbol to retrieve margin trade details for.</param>
    /// <param name="volume">The volume to be traded.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing margin trade details.</returns>
    Task<MarginTradeResponse> GetMarginTradeAsync(string symbol, double? volume, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously calculates the profit for a trade based on various parameters.
    /// </summary>
    /// <param name="symbol">The symbol for the trade.</param>
    /// <param name="volume">The volume to be traded.</param>
    /// <param name="tradeOperation">The type of trade operation (buy/sell).</param>
    /// <param name="openPrice">The opening price of the trade.</param>
    /// <param name="closePrice">The closing price of the trade.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the profit calculation details.</returns>
    Task<ProfitCalculationResponse> GetProfitCalculationAsync(string symbol, double? volume, TRADE_OPERATION_TYPE tradeOperation, double? openPrice, double? closePrice, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves market information for a given symbol.
    /// </summary>
    /// <param name="symbol">The symbol to retrieve market information for.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing market information for the specified symbol.</returns>
    Task<SymbolResponse> GetMarketInfoAsync(string symbol, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves information for all symbols available on the API.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing information for all available symbols.</returns>
    Task<AllSymbolsResponse> GetAllSymbolsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves symbol information for a specific symbol.
    /// </summary>
    /// <param name="symbol">The symbol to retrieve information for.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the symbol's information.</returns>
    Task<SymbolResponse> GetSymbolAsync(string symbol, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves trading hours information for a list of symbols.
    /// </summary>
    /// <param name="symbols">An array of symbols to retrieve trading hours for.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing trading hours information.</returns>
    Task<TradingHoursResponse> GetTradingHoursAsync(string[] symbols, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves tick prices for a given set of symbols.
    /// </summary>
    /// <param name="symbols">An array of symbols to retrieve tick prices for.</param>
    /// <param name="level">The level of detail for the tick prices.</param>
    /// <param name="time">Optional parameter to specify the time for retrieving tick prices.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing tick prices.</returns>
    Task<TickPricesResponse> GetTickPricesAsync(string[] symbols, int level, DateTimeOffset? time = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves the chart range data for the specified chart range information.
    /// </summary>
    /// <param name="rangeInfoRecord">The chart range information.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the chart range data.</returns>
    Task<ChartRangeResponse> GetChartRangeAsync(ChartRangeInfoRecord rangeInfoRecord, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves the chart range data for a specific symbol, period, and date range.
    /// </summary>
    /// <param name="symbol">The symbol for which to retrieve chart range data.</param>
    /// <param name="period">The period for the chart range.</param>
    /// <param name="since">The starting date and time for the chart range.</param>
    /// <param name="until">The ending date and time for the chart range.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the chart range data.</returns>
    Task<ChartRangeResponse> GetChartRangeAsync(string symbol, PERIOD period, DateTimeOffset since, DateTimeOffset until, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves the chart range data for a specific symbol, period, and number of ticks.
    /// </summary>
    /// <param name="symbol">The symbol for which to retrieve chart range data.</param>
    /// <param name="period">The period for the chart range.</param>
    /// <param name="since">The starting date and time for the chart range.</param>
    /// <param name="ticks">The number of ticks to retrieve for the chart range.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the chart range data.</returns>
    Task<ChartRangeResponse> GetChartRangeAsync(string symbol, PERIOD period, DateTimeOffset since, int ticks, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves the last chart data for the specified chart range information.
    /// </summary>
    /// <param name="rangeInfoRecord">The chart range information.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the last chart data.</returns>
    Task<ChartLastResponse> GetChartLastAsync(ChartLastInfoRecord rangeInfoRecord, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves the last chart data for a specific symbol and period since a specified date and time.
    /// </summary>
    /// <param name="symbol">The symbol for which to retrieve the last chart data.</param>
    /// <param name="period">The period for the chart data.</param>
    /// <param name="since">The starting date and time for the chart data.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the last chart data.</returns>
    Task<ChartLastResponse> GetChartLastAsync(string symbol, PERIOD period, DateTimeOffset since, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves the list of trades, filtered by whether they are open only or include closed trades.
    /// </summary>
    /// <param name="openOnly">If true, retrieves only open trades. If false, retrieves all trades.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the list of trades.</returns>
    Task<TradesResponse> GetTradesAsync(bool openOnly, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously open, close, set or modify trade transaction based on the provided trade transaction information.
    /// </summary>
    /// <param name="tradeTransInfoRecord">The trade transaction information record.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the trade transaction data.</returns>
    Task<TradeTransactionResponse> SetTradeTransactionAsync(TradeTransInfoRecord tradeTransInfoRecord, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves trade records for the provided list of order ids.
    /// </summary>
    /// <param name="orderIds">A linked list of order IDs for which to retrieve trade records.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the trade records.</returns>
    Task<TradeRecordsResponse> GetTradeRecordsAsync(long?[] orderIds, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves the trade history within a specified time range.
    /// </summary>
    /// <param name="since">The start date and time of the trade history period. If null, the history will start from the earliest available point.</param>
    /// <param name="until">The end date and time of the trade history period. If null, the history will end at the latest available point.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the trade history within the specified time range.</returns>
    Task<TradesHistoryResponse> GetTradesHistoryAsync(DateTimeOffset? since, DateTimeOffset? until = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves the calendar events from the API.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the calendar events.</returns>
    Task<CalendarResponse> GetCalendarAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Asynchronously retrieves news articles within a specified time range.
    /// </summary>
    /// <param name="since">The start date and time for retrieving news. If null, the news will be retrieved from the earliest available point.</param>
    /// <param name="until">The end date and time for retrieving news. If null, the news will be retrieved until the latest available point.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the news articles within the specified time range.</returns>
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
    /// Logs in to the API.
    /// </summary>
    /// <param name="userId">The user ID used to log in.</param>
    /// <param name="password">The password used to log in.</param>
    /// <param name="appId">Optional. The application ID for the login request.</param>
    /// <param name="appName">Optional. The application name for the login request.</param>
    /// <returns>The response from the login command.</returns>
    LoginResponse Login(string userId, string password, string? appId = null, string? appName = null);

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
    /// Open, close, set or modify a trade transaction based on the provided trade transaction information.
    /// </summary>
    /// <param name="tradeTransInfoRecord">The trade transaction information record.</param>
    /// <returns>The response containing the trade transaction data.</returns>
    TradeTransactionResponse SetTradeTransaction(TradeTransInfoRecord tradeTransInfoRecord);

    /// <summary>
    /// Retrieves trade records for the provided list of order ids.
    /// </summary>
    /// <param name="orderIds">A linked list of order ids for which to retrieve trade records.</param>
    /// <returns>The response containing the trade records.</returns>
    TradeRecordsResponse GetTradeRecords(long?[] orderIds);

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

/// <summary>
/// Base Xtb XApi client interface for handling connections and performing API requests.
/// </summary>
public interface IXApiClientBase
{
    /// <summary>
    /// Occurs when the client is connected to the API.
    /// </summary>
    event EventHandler<EndpointEventArgs>? Connected;

    /// <summary>
    /// Occurs when the client is disconnected from the API.
    /// </summary>
    event EventHandler? Disconnected;

    /// <summary>
    /// Account id.
    /// </summary>
    string? AccountId { get; }

    /// <summary>
    /// Api connector for handling requests and responses.
    /// </summary>
    ApiConnector ApiConnector { get; }

    /// <summary>
    /// Streaming api connector for handling streaming data.
    /// </summary>
    StreamingApiConnector StreamingConnector { get; }

    /// <summary>
    /// Endpoint for requesting data.
    /// </summary>
    IPEndPoint RequestingEndpoint { get; }

    /// <summary>
    /// Endpoint for streaming data.
    /// </summary>
    IPEndPoint StreamingEndpoint { get; }
}