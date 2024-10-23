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
        var apiConnector = ApiConnector.Create(address, requestingPort, streamingPort, streamingListener);
        return new XApiClient(apiConnector)
        {
            IsApiConnectorOwner = true
        };
    }

    /// <summary>
    /// Creates a new <see cref="XApiClient"/> instance using the provided endpoints.
    /// </summary>
    /// <param name="endpoint">The endpoint for requesting data.</param>
    /// <param name="streamingEndpoint">The endpoint for streaming data.</param>
    /// <param name="streamingListener">Optional streaming listener for handling streamed data.</param>
    /// <returns>A new instance of <see cref="XApiClient"/>.</returns>
    public static XApiClient Create(IPEndPoint endpoint, IPEndPoint streamingEndpoint, IStreamingListener? streamingListener = null)
    {
        var streamingApiConnector = new StreamingApiConnector(streamingEndpoint, streamingListener);
        var apiConnector = new ApiConnector(endpoint, streamingApiConnector);
        return new XApiClient(apiConnector)
        {
            IsApiConnectorOwner = true
        };
    }

    private Credentials? _credentials;

    /// <summary>
    /// Initializes a new instance of the <see cref="XApiClient"/> class using the specified <see cref="ApiConnector"/>.
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
    /// Occurs when the client is redirected to a new endpoint.
    /// </summary>
    public event EventHandler<EndpointEventArgs>? Redirected
    {
        add { ApiConnector.Redirected += value; }
        remove { ApiConnector.Redirected -= value; }
    }

    /// <summary>
    /// Occurs when the client is disconnected from the API.
    /// </summary>
    public event EventHandler? Disconnected
    {
        add { ApiConnector.Disconnected += value; }
        remove { ApiConnector.Disconnected -= value; }
    }

    /// <summary>
    /// Occurs when a message is received from the API.
    /// </summary>
    public event EventHandler<MessageEventArgs>? MessageReceived
    {
        add { ApiConnector.MessageReceived += value; }
        remove { ApiConnector.MessageReceived -= value; }
    }

    /// <summary>
    /// Occurs when a message is sent to the API.
    /// </summary>
    public event EventHandler<MessageEventArgs>? MessageSent
    {
        add { ApiConnector.MessageSent += value; }
        remove { ApiConnector.MessageSent -= value; }
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
    public StreamingApiConnector Streaming => ApiConnector.Streaming;

    /// <summary>
    /// Gets the account ID after a successful login.
    /// </summary>
    public string? AccountId => _credentials?.Login;

    /// <summary>
    /// Connects the client to the API.
    /// </summary>
    public void Connect()
    {
        ApiConnector.Connect();
    }

    /// <summary>
    /// Asynchronously connects the client to the API.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task ConnectAsync(CancellationToken cancellationToken = default)
    {
        await ApiConnector.ConnectAsync(cancellationToken);
    }

    /// <summary>
    /// Disconnects the client from the API.
    /// </summary>
    public void Disconnect()
    {
        ApiConnector.Disconnect();
    }

    /// <summary>
    /// Asynchronously disconnects the client from the API.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task DisconnectAsync(CancellationToken cancellationToken = default)
    {
        ApiConnector.Disconnect();
        return Task.CompletedTask;
    }

    /// <summary>
    /// Sends a ping command to the API.
    /// </summary>
    /// <returns>The response from the ping command.</returns>
    public PingResponse Ping() => APICommandFactory.ExecutePingCommand(ApiConnector);

    /// <summary>
    /// Asynchronously sends a ping command to the API.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response from the ping command.</returns>
    public Task<PingResponse> PingAsync(CancellationToken cancellationToken = default)
        => APICommandFactory.ExecutePingCommandAsync(ApiConnector, cancellationToken);

    /// <summary>
    /// Retrieves the version of the API.
    /// </summary>
    /// <returns>The response containing the version information.</returns>
    public VersionResponse GetVersion()
        => APICommandFactory.ExecuteVersionCommand(ApiConnector);

    /// <summary>
    /// Asynchronously retrieves the version of the API.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the version information.</returns>
    public Task<VersionResponse> GetVersionAsync(CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteVersionCommandAsync(ApiConnector, cancellationToken);

    /// <summary>
    /// Retrieves the current server time.
    /// </summary>
    /// <returns>The response containing the server time information.</returns>
    public ServerTimeResponse GetServerTime()
        => APICommandFactory.ExecuteServerTimeCommand(ApiConnector);

    /// <summary>
    /// Asynchronously retrieves the current server time.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the server time information.</returns>
    public Task<ServerTimeResponse> GetServerTimeAsync(CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteServerTimeCommandAsync(ApiConnector, cancellationToken);


    /// <summary>
    /// Logs in to the API using the specified credentials.
    /// </summary>
    /// <param name="credentials">The credentials used to log in.</param>
    /// <returns>The response from the login command.</returns>
    public LoginResponse Login(Credentials credentials)
    {
        _credentials = credentials;
        return APICommandFactory.ExecuteLoginCommand(ApiConnector, credentials);
    }

    /// <summary>
    /// Asynchronously logs in to the API using the specified credentials.
    /// </summary>
    /// <param name="credentials">The credentials used to log in.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response from the login command.</returns>
    public Task<LoginResponse> LoginAsync(Credentials credentials, CancellationToken cancellationToken = default)
    {
        _credentials = credentials;
        return APICommandFactory.ExecuteLoginCommandAsync(ApiConnector, credentials, cancellationToken);
    }

    /// <summary>
    /// Logs out from the API.
    /// </summary>
    /// <returns>The response from the logout command.</returns>
    public LogoutResponse Logout() => APICommandFactory.ExecuteLogoutCommand(ApiConnector);

    /// <summary>
    /// Asynchronously logs out from the API.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response from the logout command.</returns>
    public Task<LogoutResponse> LogoutAsync(CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteLogoutCommandAsync(ApiConnector, cancellationToken);

    /// <summary>
    /// Retrieves the current user data from the API.
    /// </summary>
    /// <returns>The response containing the current user's data.</returns>
    public CurrentUserDataResponse GetCurrentUserData() => APICommandFactory.ExecuteCurrentUserDataCommand(ApiConnector);

    /// <summary>
    /// Asynchronously retrieves the current user data from the API.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the current user's data.</returns>
    public Task<CurrentUserDataResponse> GetCurrentUserDataAsync(CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteCurrentUserDataCommandAsync(ApiConnector, cancellationToken);

    /// <summary>
    /// Retrieves commission definition for a given symbol and volume.
    /// </summary>
    /// <param name="symbol">The symbol for which the commission is requested.</param>
    /// <param name="volume">The volume to be traded.</param>
    /// <returns>The response containing commission details.</returns>
    public CommissionDefResponse GetCommissionDef(string symbol, double? volume)
        => APICommandFactory.ExecuteCommissionDefCommand(ApiConnector, symbol, volume);

    /// <summary>
    /// Asynchronously retrieves commission definition for a given symbol and volume.
    /// </summary>
    /// <param name="symbol">The symbol for which the commission is requested.</param>
    /// <param name="volume">The volume to be traded.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing commission details.</returns>
    public Task<CommissionDefResponse> GetCommissionDefAsync(string symbol, double? volume, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteCommissionDefCommandAsync(ApiConnector, symbol, volume, cancellationToken);

    /// <summary>
    /// Retrieves the margin level from the API.
    /// </summary>
    /// <returns>The response containing margin level details.</returns>
    public MarginLevelResponse GetMarginLevel() => APICommandFactory.ExecuteMarginLevelCommand(ApiConnector);

    /// <summary>
    /// Asynchronously retrieves the margin level from the API.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing margin level details.</returns>
    public Task<MarginLevelResponse> GetMarginLevelAsync(CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteMarginLevelCommandAsync(ApiConnector, cancellationToken);

    /// <summary>
    /// Retrieves margin trade details for a specified symbol and volume.
    /// </summary>
    /// <param name="symbol">The symbol to retrieve margin trade details for.</param>
    /// <param name="volume">The volume to be traded.</param>
    /// <returns>The response containing margin trade details.</returns>
    public MarginTradeResponse GetMarginTrade(string symbol, double? volume)
        => APICommandFactory.ExecuteMarginTradeCommand(ApiConnector, symbol, volume);

    /// <summary>
    /// Asynchronously retrieves margin trade details for a specified symbol and volume.
    /// </summary>
    /// <param name="symbol">The symbol to retrieve margin trade details for.</param>
    /// <param name="volume">The volume to be traded.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing margin trade details.</returns>
    public Task<MarginTradeResponse> GetMarginTradeAsync(string symbol, double? volume, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteMarginTradeCommandAsync(ApiConnector, symbol, volume, cancellationToken);

    /// <summary>
    /// Calculates the profit for a trade based on various parameters.
    /// </summary>
    /// <param name="symbol">The symbol for the trade.</param>
    /// <param name="volume">The volume to be traded.</param>
    /// <param name="tradeOperation">The type of trade operation (buy/sell).</param>
    /// <param name="openPrice">The opening price of the trade.</param>
    /// <param name="closePrice">The closing price of the trade.</param>
    /// <returns>The response containing the profit calculation details.</returns>
    public ProfitCalculationResponse GetProfitCalculation(string symbol, double? volume, TRADE_OPERATION_TYPE tradeOperation, double? openPrice, double? closePrice)
        => APICommandFactory.ExecuteProfitCalculationCommand(ApiConnector, symbol, volume, tradeOperation, openPrice, closePrice);

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
    public Task<ProfitCalculationResponse> GetProfitCalculationAsync(string symbol, double? volume, TRADE_OPERATION_TYPE tradeOperation, double? openPrice, double? closePrice, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteProfitCalculationCommandAsync(ApiConnector, symbol, volume, tradeOperation, openPrice, closePrice, cancellationToken);

    /// <summary>
    /// Retrieves market information for a given symbol.
    /// </summary>
    /// <param name="symbol">The symbol to retrieve market information for.</param>
    /// <returns>The response containing market information for the specified symbol.</returns>
    public SymbolResponse GetMarketInfo(string symbol) => APICommandFactory.ExecuteSymbolCommand(ApiConnector, symbol);

    /// <summary>
    /// Asynchronously retrieves market information for a given symbol.
    /// </summary>
    /// <param name="symbol">The symbol to retrieve market information for.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing market information for the specified symbol.</returns>
    public Task<SymbolResponse> GetMarketInfoAsync(string symbol, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteSymbolCommandAsync(ApiConnector, symbol, cancellationToken);

    /// <summary>
    /// Retrieves information for all symbols available on the API.
    /// </summary>
    /// <returns>The response containing information for all available symbols.</returns>
    public AllSymbolsResponse GetAllSymbols() => APICommandFactory.ExecuteAllSymbolsCommand(ApiConnector);

    /// <summary>
    /// Asynchronously retrieves information for all symbols available on the API.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing information for all available symbols.</returns>
    public Task<AllSymbolsResponse> GetAllSymbolsAsync(CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteAllSymbolsCommandAsync(ApiConnector, cancellationToken);

    /// <summary>
    /// Retrieves symbol information for a specific symbol.
    /// </summary>
    /// <param name="symbol">The symbol to retrieve information for.</param>
    /// <returns>The response containing the symbol's information.</returns>
    public SymbolResponse GetSymbol(string symbol) => APICommandFactory.ExecuteSymbolCommand(ApiConnector, symbol);

    /// <summary>
    /// Asynchronously retrieves symbol information for a specific symbol.
    /// </summary>
    /// <param name="symbol">The symbol to retrieve information for.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the symbol's information.</returns>
    public Task<SymbolResponse> GetSymbolAsync(string symbol, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteSymbolCommandAsync(ApiConnector, symbol, cancellationToken);

    /// <summary>
    /// Retrieves trading hours information for a list of symbols.
    /// </summary>
    /// <param name="symbols">An array of symbols to retrieve trading hours for.</param>
    /// <returns>The response containing trading hours information.</returns>
    public TradingHoursResponse GetTradingHours(string[] symbols) => APICommandFactory.ExecuteTradingHoursCommand(ApiConnector, symbols);

    /// <summary>
    /// Asynchronously retrieves trading hours information for a list of symbols.
    /// </summary>
    /// <param name="symbols">An array of symbols to retrieve trading hours for.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing trading hours information.</returns>
    public Task<TradingHoursResponse> GetTradingHoursAsync(string[] symbols, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteTradingHoursCommandAsync(ApiConnector, symbols, cancellationToken);

    /// <summary>
    /// Retrieves tick prices for a given set of symbols.
    /// </summary>
    /// <param name="symbols">An array of symbols to retrieve tick prices for.</param>
    /// <param name="level">The level of detail for the tick prices.</param>
    /// <param name="time">Optional parameter to specify the time for retrieving tick prices.</param>
    /// <returns>The response containing tick prices.</returns>
    public TickPricesResponse GetTickPrices(string[] symbols, int level, DateTimeOffset? time = null)
        => APICommandFactory.ExecuteTickPricesCommand(ApiConnector, symbols, level, time);

    /// <summary>
    /// Asynchronously retrieves tick prices for a given set of symbols.
    /// </summary>
    /// <param name="symbols">An array of symbols to retrieve tick prices for.</param>
    /// <param name="level">The level of detail for the tick prices.</param>
    /// <param name="time">Optional parameter to specify the time for retrieving tick prices.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing tick prices.</returns>
    public Task<TickPricesResponse> GetTickPricesAsync(string[] symbols, int level, DateTimeOffset? time = null, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteTickPricesCommandAsync(ApiConnector, symbols, level, time, cancellationToken);

    /// <summary>
    /// Retrieves the chart range data for the specified chart range information.
    /// </summary>
    /// <param name="rangeInfoRecord">The chart range information.</param>
    /// <returns>The response containing the chart range data.</returns>
    public ChartRangeResponse GetChartRange(ChartRangeInfoRecord rangeInfoRecord)
        => APICommandFactory.ExecuteChartRangeCommand(ApiConnector, rangeInfoRecord);

    /// <summary>
    /// Retrieves the chart range data for a specific symbol, period, and date range.
    /// </summary>
    /// <param name="symbol">The symbol for which to retrieve chart range data.</param>
    /// <param name="period">The period for the chart range.</param>
    /// <param name="since">The starting date and time for the chart range.</param>
    /// <param name="until">The ending date and time for the chart range.</param>
    /// <returns>The response containing the chart range data.</returns>
    public ChartRangeResponse GetChartRange(string symbol, PERIOD period, DateTimeOffset since, DateTimeOffset until)
        => APICommandFactory.ExecuteChartRangeCommand(ApiConnector, symbol, period, since, until, 0);

    /// <summary>
    /// Retrieves the chart range data for a specific symbol, period, and number of ticks.
    /// </summary>
    /// <param name="symbol">The symbol for which to retrieve chart range data.</param>
    /// <param name="period">The period for the chart range.</param>
    /// <param name="since">The starting date and time for the chart range.</param>
    /// <param name="ticks">The number of ticks to retrieve for the chart range.</param>
    /// <returns>The response containing the chart range data.</returns>
    public ChartRangeResponse GetChartRange(string symbol, PERIOD period, DateTimeOffset since, int ticks)
        => APICommandFactory.ExecuteChartRangeCommand(ApiConnector, symbol, period, since, default, ticks);

    /// <summary>
    /// Asynchronously retrieves the chart range data for the specified chart range information.
    /// </summary>
    /// <param name="rangeInfoRecord">The chart range information.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the chart range data.</returns>
    public Task<ChartRangeResponse> GetChartRangeAsync(ChartRangeInfoRecord rangeInfoRecord, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteChartRangeCommandAsync(ApiConnector, rangeInfoRecord, cancellationToken);

    /// <summary>
    /// Asynchronously retrieves the chart range data for a specific symbol, period, and date range.
    /// </summary>
    /// <param name="symbol">The symbol for which to retrieve chart range data.</param>
    /// <param name="period">The period for the chart range.</param>
    /// <param name="since">The starting date and time for the chart range.</param>
    /// <param name="until">The ending date and time for the chart range.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the chart range data.</returns>
    public Task<ChartRangeResponse> GetChartRangeAsync(string symbol, PERIOD period, DateTimeOffset since, DateTimeOffset until, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteChartRangeCommandAsync(ApiConnector, symbol, period, since, until, 0, cancellationToken);

    /// <summary>
    /// Asynchronously retrieves the chart range data for a specific symbol, period, and number of ticks.
    /// </summary>
    /// <param name="symbol">The symbol for which to retrieve chart range data.</param>
    /// <param name="period">The period for the chart range.</param>
    /// <param name="since">The starting date and time for the chart range.</param>
    /// <param name="ticks">The number of ticks to retrieve for the chart range.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the chart range data.</returns>
    public Task<ChartRangeResponse> GetChartRangeAsync(string symbol, PERIOD period, DateTimeOffset since, int ticks, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteChartRangeCommandAsync(ApiConnector, symbol, period, since, default, ticks, cancellationToken);

    /// <summary>
    /// Retrieves the last chart data for the specified chart range information.
    /// </summary>
    /// <param name="rangeInfoRecord">The chart range information.</param>
    /// <returns>The response containing the last chart data.</returns>
    public ChartLastResponse GetChartLast(ChartLastInfoRecord rangeInfoRecord)
        => APICommandFactory.ExecuteChartLastCommand(ApiConnector, rangeInfoRecord);

    /// <summary>
    /// Retrieves the last chart data for a specific symbol and period since a specified date and time.
    /// </summary>
    /// <param name="symbol">The symbol for which to retrieve the last chart data.</param>
    /// <param name="period">The period for the chart data.</param>
    /// <param name="since">The starting date and time for the chart data.</param>
    /// <returns>The response containing the last chart data.</returns>
    public ChartLastResponse GetChartLast(string symbol, PERIOD period, DateTimeOffset since)
        => APICommandFactory.ExecuteChartLastCommand(ApiConnector, symbol, period, since);

    /// <summary>
    /// Asynchronously retrieves the last chart data for the specified chart range information.
    /// </summary>
    /// <param name="rangeInfoRecord">The chart range information.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the last chart data.</returns>
    public Task<ChartLastResponse> GetChartLastAsync(ChartLastInfoRecord rangeInfoRecord, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteChartLastCommandAsync(ApiConnector, rangeInfoRecord, cancellationToken);

    /// <summary>
    /// Asynchronously retrieves the last chart data for a specific symbol and period since a specified date and time.
    /// </summary>
    /// <param name="symbol">The symbol for which to retrieve the last chart data.</param>
    /// <param name="period">The period for the chart data.</param>
    /// <param name="since">The starting date and time for the chart data.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the last chart data.</returns>
    public Task<ChartLastResponse> GetChartLastAsync(string symbol, PERIOD period, DateTimeOffset since, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteChartLastCommandAsync(ApiConnector, symbol, period, since, cancellationToken);

    /// <summary>
    /// Retrieves the list of trades, filtered by whether they are open only or include closed trades.
    /// </summary>
    /// <param name="openOnly">If true, retrieves only open trades. If false, retrieves all trades.</param>
    /// <returns>The response containing the list of trades.</returns>
    public TradesResponse GetTrades(bool openOnly) => APICommandFactory.ExecuteTradesCommand(ApiConnector, openOnly);

    /// <summary>
    /// Asynchronously retrieves the list of trades, filtered by whether they are open only or include closed trades.
    /// </summary>
    /// <param name="openOnly">If true, retrieves only open trades. If false, retrieves all trades.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the list of trades.</returns>
    public Task<TradesResponse> GetTradesAsync(bool openOnly, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteTradesCommandAsync(ApiConnector, openOnly, cancellationToken);

    /// <summary>
    /// Retrieves a trade transaction based on the provided trade transaction information.
    /// </summary>
    /// <param name="tradeTransInfoRecord">The trade transaction information record.</param>
    /// <returns>The response containing the trade transaction data.</returns>
    public TradeTransactionResponse GetTradeTransaction(TradeTransInfoRecord tradeTransInfoRecord)
        => APICommandFactory.ExecuteTradeTransactionCommand(ApiConnector, tradeTransInfoRecord);

    /// <summary>
    /// Asynchronously retrieves a trade transaction based on the provided trade transaction information.
    /// </summary>
    /// <param name="tradeTransInfoRecord">The trade transaction information record.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the trade transaction data.</returns>
    public Task<TradeTransactionResponse> GetTradeTransactionAsync(TradeTransInfoRecord tradeTransInfoRecord, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteTradeTransactionCommandAsync(ApiConnector, tradeTransInfoRecord, cancellationToken);

    /// <summary>
    /// Retrieves trade records for the provided list of orders.
    /// </summary>
    /// <param name="orders">A linked list of order IDs for which to retrieve trade records.</param>
    /// <returns>The response containing the trade records.</returns>
    public TradeRecordsResponse GetTradeRecords(LinkedList<long?> orders)
        => APICommandFactory.ExecuteTradeRecordsCommand(ApiConnector, orders);

    /// <summary>
    /// Asynchronously retrieves trade records for the provided list of orders.
    /// </summary>
    /// <param name="orders">A linked list of order IDs for which to retrieve trade records.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the trade records.</returns>
    public Task<TradeRecordsResponse> GetTradeRecordsAsync(LinkedList<long?> orders, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteTradeRecordsCommandAsync(ApiConnector, orders, cancellationToken);

    /// <summary>
    /// Retrieves the trade history within a specified time range.
    /// </summary>
    /// <param name="since">The start date and time of the trade history period. If null, the history will start from the earliest available point.</param>
    /// <param name="until">The end date and time of the trade history period. If null, the history will end at the latest available point.</param>
    /// <returns>The response containing the trade history within the specified time range.</returns>
    public TradesHistoryResponse GetTradesHistory(DateTimeOffset? since, DateTimeOffset? until = null)
        => APICommandFactory.ExecuteTradesHistoryCommand(ApiConnector, since, until);

    /// <summary>
    /// Asynchronously retrieves the trade history within a specified time range.
    /// </summary>
    /// <param name="since">The start date and time of the trade history period. If null, the history will start from the earliest available point.</param>
    /// <param name="until">The end date and time of the trade history period. If null, the history will end at the latest available point.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the trade history within the specified time range.</returns>
    public Task<TradesHistoryResponse> GetTradesHistoryAsync(DateTimeOffset? since, DateTimeOffset? until = null, CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteTradesHistoryCommandAsync(ApiConnector, since, until, cancellationToken);

    /// <summary>
    /// Retrieves the calendar events from the API.
    /// </summary>
    /// <returns>The response containing the calendar events.</returns>
    public CalendarResponse GetCalendar() => APICommandFactory.ExecuteCalendarCommand(ApiConnector);

    /// <summary>
    /// Asynchronously retrieves the calendar events from the API.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the calendar events.</returns>
    public Task<CalendarResponse> GetCalendarAsync(CancellationToken cancellationToken = default)
        => APICommandFactory.ExecuteCalendarCommandAsync(ApiConnector, cancellationToken);

    /// <summary>
    /// Retrieves news articles within a specified time range.
    /// </summary>
    /// <param name="since">The start date and time for retrieving news. If null, the news will be retrieved from the earliest available point.</param>
    /// <param name="until">The end date and time for retrieving news. If null, the news will be retrieved until the latest available point.</param>
    /// <returns>The response containing the news articles within the specified time range.</returns>
    public NewsResponse GetNews(DateTimeOffset? since, DateTimeOffset? until = null)
        => APICommandFactory.ExecuteNewsCommand(ApiConnector, since, until);

    /// <summary>
    /// Asynchronously retrieves news articles within a specified time range.
    /// </summary>
    /// <param name="since">The start date and time for retrieving news. If null, the news will be retrieved from the earliest available point.</param>
    /// <param name="until">The end date and time for retrieving news. If null, the news will be retrieved until the latest available point.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task representing the response containing the news articles within the specified time range.</returns>
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