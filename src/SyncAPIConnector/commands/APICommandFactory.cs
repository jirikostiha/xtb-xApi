using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using XApi.Codes;
using XApi.Records;
using XApi.Responses;

namespace XApi.Commands;

public static class APICommandFactory
{
    /// <summary> Api version. </summary>
    public const string Version = "2.5.0";

    /// <summary> Application type. </summary>
    public const string AppType = "dotNET";

    /// <summary>
    /// Maximum number of redirects (to avoid redirection loops).
    /// </summary>
    public const int MAX_REDIRECTS = 3;

    #region Command creators

    public static LoginCommand CreateLoginCommand(string userId, string password, string? appId = null, string? appName = null, bool prettyPrint = false)
    {
        JsonObject args = new()
        {
            { "userId", userId },
            { "password", password },
            { "type", AppType },
            { "version", Version },
        };

        if (appId != null)
            args.Add("appId", appId);

        if (appName != null)
            args.Add("appName", appName);

        return new LoginCommand(args, prettyPrint);
    }

    public static LoginCommand CreateLoginCommand(Credentials credentials, bool prettyPrint = false)
        => CreateLoginCommand(credentials.Login, credentials.Password, credentials.AppId, credentials.AppName, prettyPrint);

    public static ChartLastCommand CreateChartLastCommand(string symbol, PERIOD period, DateTimeOffset? start, bool prettyPrint = false)
    {
        JsonObject args = new()
        {
            { "info", new ChartLastInfoRecord(symbol, period, start).ToJsonObject() }
        };

        return new ChartLastCommand(args, prettyPrint);
    }

    public static ChartLastCommand CreateChartLastCommand(ChartLastInfoRecord info, bool prettyPrint = false)
    {
        JsonObject args = new()
        {
            { "info", info.ToJsonObject() }
        };

        return new ChartLastCommand(args, prettyPrint);
    }

    public static ChartRangeCommand CreateChartRangeCommand(ChartRangeInfoRecord info, bool prettyPrint = false)
    {
        JsonObject args = new()
        {
            { "info", info.ToJsonObject() }
        };

        return new ChartRangeCommand(args, prettyPrint);
    }

    public static ChartRangeCommand CreateChartRangeCommand(string symbol,
        PERIOD period,
        DateTimeOffset? start,
        DateTimeOffset? end,
        int? ticks,
        bool prettyPrint = false)
    {
        JsonObject args = new()
        {
            { "info", new ChartRangeInfoRecord(symbol, period, start, end, ticks).ToJsonObject() }
        };

        return new ChartRangeCommand(args, prettyPrint);
    }

    public static CommissionDefCommand CreateCommissionDefCommand(string symbol, double? volume, bool prettyPrint = false)
    {
        JsonObject args = new()
        {
            { "symbol", symbol },
            { "volume", volume }
        };

        return new CommissionDefCommand(args, prettyPrint);
    }

    public static MarginTradeCommand CreateMarginTradeCommand(string symbol, double? volume, bool prettyPrint = false)
    {
        JsonObject args = new()
        {
            { "symbol", symbol },
            { "volume", volume }
        };

        return new MarginTradeCommand(args, prettyPrint);
    }

    public static NewsCommand CreateNewsCommand(DateTimeOffset? start, DateTimeOffset? end, bool prettyPrint = false)
    {
        JsonObject args = new()
        {
            { "start", start?.ToUnixTimeMilliseconds() },
            { "end", end?.ToUnixTimeMilliseconds() ?? 0 }
        };

        return new NewsCommand(args, prettyPrint);
    }

    public static ProfitCalculationCommand CreateProfitCalculationCommand(string symbol,
        double? volume,
        TRADE_OPERATION_TYPE tradeOperation,
        double? openPrice,
        double? closePrice,
        bool prettyPrint = false)
    {
        JsonObject args = new()
        {
            { "symbol", symbol },
            { "volume", volume },
            { "cmd", tradeOperation.Code },
            { "openPrice", openPrice },
            { "closePrice", closePrice }
        };

        return new ProfitCalculationCommand(args, prettyPrint);
    }

    public static SymbolCommand CreateSymbolCommand(string symbol, bool prettyPrint = false)
    {
        JsonObject args = new()
        {
            { "symbol", symbol }
        };

        return new SymbolCommand(args, prettyPrint);
    }

    public static TickPricesCommand CreateTickPricesCommand(string[] symbols, int level, DateTimeOffset? timestamp, bool prettyPrint = false)
    {
        JsonObject args = [];
        JsonArray arr = [.. symbols];
        args.Add("level", level);
        args.Add("symbols", arr);
        args.Add("timestamp", timestamp?.ToUnixTimeMilliseconds());

        return new TickPricesCommand(args, prettyPrint);
    }

    public static TradeRecordsCommand CreateTradeRecordsCommand(LinkedList<long?> orders, bool prettyPrint = false)
    {
        JsonObject args = [];
        JsonArray arr = [.. orders];
        args.Add("orders", arr);

        return new TradeRecordsCommand(args, prettyPrint);
    }

    public static TradeTransactionCommand CreateTradeTransactionCommand(TradeTransInfoRecord tradeTransInfo, bool prettyPrint = false)
    {
        JsonObject args = new()
        {
            { "tradeTransInfo", tradeTransInfo.ToJsonObject() }
        };

        return new TradeTransactionCommand(args, prettyPrint);
    }

    public static TradeTransactionCommand CreateTradeTransactionCommand(TRADE_OPERATION_TYPE tradeOperation,
        TRADE_TRANSACTION_TYPE transactionType,
        double? price,
        double? sl,
        double? tp,
        string symbol,
        double? volume,
        long? order,
        string customComment,
        DateTimeOffset? expiration,
        bool prettyPrint = false)
    {
        JsonObject args = new()
        {
            { "tradeTransInfo", new TradeTransInfoRecord(tradeOperation, transactionType, price, sl, tp, symbol, volume, order, customComment, expiration).ToJsonObject() }
        };

        return new TradeTransactionCommand(args, prettyPrint);
    }

    public static TradeTransactionStatusCommand CreateTradeTransactionStatusCommand(long? order, bool prettyPrint = false)
    {
        JsonObject args = new()
        {
            { "order", order }
        };

        return new TradeTransactionStatusCommand(args, prettyPrint);
    }

    public static TradesCommand CreateTradesCommand(bool openedOnly, bool prettyPrint = false)
    {
        JsonObject args = new()
        {
            { "openedOnly", openedOnly }
        };

        return new TradesCommand(args, prettyPrint);
    }

    public static TradesHistoryCommand CreateTradesHistoryCommand(DateTimeOffset? start, DateTimeOffset? end, bool prettyPrint = false)
    {
        JsonObject args = new()
        {
            { "start", start?.ToUnixTimeMilliseconds() },
            { "end", end?.ToUnixTimeMilliseconds() ?? 0 }
        };

        return new TradesHistoryCommand(args, prettyPrint);
    }

    public static TradingHoursCommand CreateTradingHoursCommand(string[] symbols, bool prettyPrint = false)
    {
        JsonObject args = [];
        JsonArray arr = [.. symbols];
        args.Add("symbols", arr);

        return new TradingHoursCommand(args, prettyPrint);
    }

    #endregion Command creators

    #region Command executors

    public static AllSymbolsResponse ExecuteAllSymbolsCommand(ApiConnector connector, bool prettyPrint = false)
    {
        var commnad = new AllSymbolsCommand();
        var jsonObj = connector.ExecuteCommand(commnad);

        return new AllSymbolsResponse(jsonObj.ToString());
    }

    public static async Task<AllSymbolsResponse> ExecuteAllSymbolsCommandAsync(ApiConnector connector, CancellationToken cancellationToken = default)
    {
        var commnad = new AllSymbolsCommand();
        var jsonObj = await connector.ExecuteCommandAsync(commnad, cancellationToken).ConfigureAwait(false);

        return new AllSymbolsResponse(jsonObj.ToString());
    }

    public static CalendarResponse ExecuteCalendarCommand(ApiConnector connector, bool prettyPrint = false)
    {
        var command = new CalendarCommand(prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new CalendarResponse(jsonObj.ToString());
    }

    public static async Task<CalendarResponse> ExecuteCalendarCommandAsync(ApiConnector connector, CancellationToken cancellationToken = default)
    {
        var command = new CalendarCommand();
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new CalendarResponse(jsonObj.ToString());
    }

    public static ChartLastResponse ExecuteChartLastCommand(ApiConnector connector,
        string symbol,
        PERIOD period,
        DateTimeOffset? start,
        bool prettyPrint = false)
    {
        var command = CreateChartLastCommand(symbol, period, start, prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new ChartLastResponse(jsonObj.ToString());
    }

    public static ChartLastResponse ExecuteChartLastCommand(ApiConnector connector, ChartLastInfoRecord info, bool prettyPrint = false)
    {
        var command = CreateChartLastCommand(info, prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new ChartLastResponse(jsonObj.ToString());
    }

    public static async Task<ChartLastResponse> ExecuteChartLastCommandAsync(ApiConnector connector,
        string symbol,
        PERIOD period,
        DateTimeOffset? start,
        CancellationToken cancellationToken = default)
    {
        var command = CreateChartLastCommand(symbol, period, start);
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new ChartLastResponse(jsonObj.ToString());
    }

    public static async Task<ChartLastResponse> ExecuteChartLastCommandAsync(ApiConnector connector,
        ChartLastInfoRecord info,
        CancellationToken cancellationToken = default)
    {
        var command = CreateChartLastCommand(info);
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new ChartLastResponse(jsonObj.ToString());
    }

    public static ChartRangeResponse ExecuteChartRangeCommand(ApiConnector connector, ChartRangeInfoRecord info, bool prettyPrint = false)
    {
        var command = CreateChartRangeCommand(info, prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new ChartRangeResponse(jsonObj.ToString());
    }

    public static ChartRangeResponse ExecuteChartRangeCommand(ApiConnector connector,
        string symbol,
        PERIOD period,
        DateTimeOffset? start,
        DateTimeOffset? end,
        int? ticks,
        bool prettyPrint = false)
    {
        var command = CreateChartRangeCommand(symbol, period, start, end, ticks, prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new ChartRangeResponse(jsonObj.ToString());
    }

    public static async Task<ChartRangeResponse> ExecuteChartRangeCommandAsync(ApiConnector connector, ChartRangeInfoRecord info, CancellationToken cancellationToken = default)
    {
        var command = CreateChartRangeCommand(info);
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new ChartRangeResponse(jsonObj.ToString());
    }

    public static async Task<ChartRangeResponse> ExecuteChartRangeCommandAsync(ApiConnector connector,
        string symbol,
        PERIOD period,
        DateTimeOffset? start,
        DateTimeOffset? end,
        int? ticks,
        CancellationToken cancellationToken = default)
    {
        var command = CreateChartRangeCommand(symbol, period, start, end, ticks);
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new ChartRangeResponse(jsonObj.ToString());
    }

    public static CommissionDefResponse ExecuteCommissionDefCommand(ApiConnector connector,
        string symbol,
        double? volume,
        bool prettyPrint = false)
    {
        var command = CreateCommissionDefCommand(symbol, volume, prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new CommissionDefResponse(jsonObj.ToString());
    }

    public static async Task<CommissionDefResponse> ExecuteCommissionDefCommandAsync(ApiConnector connector,
        string symbol,
        double? volume,
        CancellationToken cancellationToken = default)
    {
        var command = CreateCommissionDefCommand(symbol, volume);
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new CommissionDefResponse(jsonObj.ToString());
    }

    public static LoginResponse ExecuteLoginCommand(ApiConnector connector, Credentials credentials, bool prettyPrint = false)
        => ExecuteLoginCommand(connector, credentials.Login, credentials.Password, credentials.AppId, credentials.AppName, prettyPrint);

    public static LoginResponse ExecuteLoginCommand(ApiConnector connector, string userId, string password, string? appId = null, string? appName = null, bool prettyPrint = false)
    {
        var loginCommand = CreateLoginCommand(userId, password, appId, appName, prettyPrint);
        var jsonObj = connector.ExecuteCommand(loginCommand);
        var loginResponse = new LoginResponse(jsonObj.ToString());

        var redirectCounter = 0;

        while (loginResponse.RedirectRecord != null)
        {
            if (redirectCounter >= MAX_REDIRECTS)
                throw new APICommunicationException($"Too many redirects ({redirectCounter}).");

            connector.Redirect(new IPEndPoint(IPAddress.Parse(loginResponse.RedirectRecord.Address), loginResponse.RedirectRecord.MainPort));
            redirectCounter++;
            loginResponse = new LoginResponse(connector.ExecuteCommand(loginCommand).ToString());
        }

        if (loginResponse.StreamSessionId != null)
        {
            connector.Streaming.StreamSessionId = loginResponse.StreamSessionId;
        }

        return loginResponse;
    }

    public static Task<LoginResponse> ExecuteLoginCommandAsync(ApiConnector connector, Credentials credentials, CancellationToken cancellationToken = default)
        => ExecuteLoginCommandAsync(connector, credentials.Login, credentials.Password, credentials.AppId, credentials.AppName, cancellationToken);

    public static async Task<LoginResponse> ExecuteLoginCommandAsync(ApiConnector connector, string userId, string password, string? appId = null, string? appName = null, CancellationToken cancellationToken = default)
    {
        var loginCommand = CreateLoginCommand(userId, password, appId, appName, false);
        var jsonObj = await connector.ExecuteCommandAsync(loginCommand, cancellationToken).ConfigureAwait(false);
        var loginResponse = new LoginResponse(jsonObj.ToString());

        var redirectCounter = 0;

        while (loginResponse.RedirectRecord != null)
        {
            if (redirectCounter >= MAX_REDIRECTS)
                throw new APICommunicationException($"Too many redirects ({redirectCounter}).");

            await connector.RedirectAsync(
                new IPEndPoint(IPAddress.Parse(loginResponse.RedirectRecord.Address), loginResponse.RedirectRecord.MainPort),
                cancellationToken).ConfigureAwait(false);
            redirectCounter++;
            var jsonObj2 = await connector.ExecuteCommandAsync(loginCommand, cancellationToken).ConfigureAwait(false);
            loginResponse = new LoginResponse(jsonObj2.ToString());
        }

        if (loginResponse.StreamSessionId != null)
        {
            connector.Streaming.StreamSessionId = loginResponse.StreamSessionId;
        }

        return loginResponse;
    }

    public static LogoutResponse ExecuteLogoutCommand(ApiConnector connector)
    {
        var command = new LogoutCommand();
        var jsonObj = connector.ExecuteCommand(command);

        return new LogoutResponse(jsonObj.ToString());
    }

    public static async Task<LogoutResponse> ExecuteLogoutCommandAsync(ApiConnector connector, CancellationToken cancellationToken = default)
    {
        var command = new LogoutCommand();
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new LogoutResponse(jsonObj.ToString());
    }

    public static MarginLevelResponse ExecuteMarginLevelCommand(ApiConnector connector, bool prettyPrint = false)
    {
        var command = new MarginLevelCommand(prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new MarginLevelResponse(jsonObj.ToString());
    }

    public static async Task<MarginLevelResponse> ExecuteMarginLevelCommandAsync(ApiConnector connector, CancellationToken cancellationToken = default)
    {
        var command = new MarginLevelCommand();
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new MarginLevelResponse(jsonObj.ToString());
    }

    public static MarginTradeResponse ExecuteMarginTradeCommand(ApiConnector connector,
        string symbol,
        double? volume,
        bool prettyPrint = false)
    {
        var command = CreateMarginTradeCommand(symbol, volume, prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new MarginTradeResponse(jsonObj.ToString());
    }

    public static async Task<MarginTradeResponse> ExecuteMarginTradeCommandAsync(ApiConnector connector,
        string symbol,
        double? volume,
        CancellationToken cancellationToken = default)
    {
        var command = CreateMarginTradeCommand(symbol, volume);
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new MarginTradeResponse(jsonObj.ToString());
    }

    public static NewsResponse ExecuteNewsCommand(ApiConnector connector,
        DateTimeOffset? start,
        DateTimeOffset? end = null,
        bool prettyPrint = false)
    {
        var command = CreateNewsCommand(start, end, prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new NewsResponse(jsonObj.ToString());
    }

    public static async Task<NewsResponse> ExecuteNewsCommandAsync(ApiConnector connector,
        DateTimeOffset? start,
        DateTimeOffset? end,
        CancellationToken cancellationToken = default)
    {
        var command = CreateNewsCommand(start, end);
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new NewsResponse(jsonObj.ToString());
    }

    public static ServerTimeResponse ExecuteServerTimeCommand(ApiConnector connector)
    {
        var command = new ServerTimeCommand();
        var jsonObj = connector.ExecuteCommand(command);

        return new ServerTimeResponse(jsonObj.ToString());
    }

    public static async Task<ServerTimeResponse> ExecuteServerTimeCommandAsync(ApiConnector connector, CancellationToken cancellationToken = default)
    {
        var command = new ServerTimeCommand();
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new ServerTimeResponse(jsonObj.ToString());
    }

    public static CurrentUserDataResponse ExecuteCurrentUserDataCommand(ApiConnector connector, bool prettyPrint = false)
    {
        var command = new CurrentUserDataCommand(prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new CurrentUserDataResponse(jsonObj.ToString());
    }

    public static async Task<CurrentUserDataResponse> ExecuteCurrentUserDataCommandAsync(ApiConnector connector, CancellationToken cancellationToken = default)
    {
        var command = new CurrentUserDataCommand();
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new CurrentUserDataResponse(jsonObj.ToString());
    }

    public static PingResponse ExecutePingCommand(ApiConnector connector)
    {
        var command = new PingCommand();
        var jsonObj = connector.ExecuteCommand(command);

        return new PingResponse(jsonObj.ToString());
    }

    public static async Task<PingResponse> ExecutePingCommandAsync(ApiConnector connector, CancellationToken cancellationToken = default)
    {
        var command = new PingCommand();
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new PingResponse(jsonObj.ToString());
    }

    public static ProfitCalculationResponse ExecuteProfitCalculationCommand(ApiConnector connector,
        string symbol,
        double? volume,
        TRADE_OPERATION_TYPE tradeOperation,
        double? openPrice,
        double? closePrice,
        bool prettyPrint = false)
    {
        var command = CreateProfitCalculationCommand(symbol, volume, tradeOperation, openPrice, closePrice, prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new ProfitCalculationResponse(jsonObj.ToString());
    }

    public static async Task<ProfitCalculationResponse> ExecuteProfitCalculationCommandAsync(ApiConnector connector,
        string symbol,
        double? volume,
        TRADE_OPERATION_TYPE tradeOperation,
        double? openPrice,
        double? closePrice,
        CancellationToken cancellationToken = default)
    {
        var command = CreateProfitCalculationCommand(symbol, volume, tradeOperation, openPrice, closePrice);
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new ProfitCalculationResponse(jsonObj.ToString());
    }

    public static StepRulesResponse ExecuteStepRulesCommand(ApiConnector connector, bool prettyPrint = false)
    {
        var command = new StepRulesCommand();
        var jsonObj = connector.ExecuteCommand(command);

        return new StepRulesResponse(jsonObj.ToString());
    }

    public static async Task<StepRulesResponse> ExecuteStepRulesCommandAsync(ApiConnector connector, CancellationToken cancellationToken = default)
    {
        var command = new StepRulesCommand();
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new StepRulesResponse(jsonObj.ToString());
    }

    public static SymbolResponse ExecuteSymbolCommand(ApiConnector connector, string symbol, bool prettyPrint = false)
    {
        var command = CreateSymbolCommand(symbol, prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new SymbolResponse(jsonObj.ToString());
    }

    public static async Task<SymbolResponse> ExecuteSymbolCommandAsync(ApiConnector connector, string symbol, CancellationToken cancellationToken = default)
    {
        var command = CreateSymbolCommand(symbol);
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new SymbolResponse(jsonObj.ToString());
    }

    public static TickPricesResponse ExecuteTickPricesCommand(ApiConnector connector,
        string[] symbols,
        int level,
        DateTimeOffset? timestamp,
        bool prettyPrint = false)
    {
        var command = CreateTickPricesCommand(symbols, level, timestamp, prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new TickPricesResponse(jsonObj.ToString());
    }

    public static async Task<TickPricesResponse> ExecuteTickPricesCommandAsync(ApiConnector connector,
        string[] symbols,
        int level,
        DateTimeOffset? timestamp,
        CancellationToken cancellationToken = default)
    {
        var command = CreateTickPricesCommand(symbols, level, timestamp);
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new TickPricesResponse(jsonObj.ToString());
    }

    public static TradeRecordsResponse ExecuteTradeRecordsCommand(ApiConnector connector, LinkedList<long?> orders, bool prettyPrint = false)
    {
        var command = CreateTradeRecordsCommand(orders, prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new TradeRecordsResponse(jsonObj.ToString());
    }

    public static async Task<TradeRecordsResponse> ExecuteTradeRecordsCommandAsync(ApiConnector connector, LinkedList<long?> orders, CancellationToken cancellationToken = default)
    {
        var command = CreateTradeRecordsCommand(orders);
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new TradeRecordsResponse(jsonObj.ToString());
    }

    public static TradeTransactionResponse ExecuteTradeTransactionCommand(ApiConnector connector, TradeTransInfoRecord tradeTransInfo, bool prettyPrint = false)
    {
        var command = CreateTradeTransactionCommand(tradeTransInfo, prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new TradeTransactionResponse(jsonObj.ToString());
    }

    public static TradeTransactionResponse ExecuteTradeTransactionCommand(ApiConnector connector,
        TRADE_OPERATION_TYPE tradeOperation,
        TRADE_TRANSACTION_TYPE transactionType,
        double? price,
        double? sl,
        double? tp,
        string symbol,
        double? volume,
        long? order,
        string customComment,
        DateTimeOffset? expiration,
        bool prettyPrint = false)
    {
        var command = CreateTradeTransactionCommand(tradeOperation, transactionType, price, sl, tp, symbol, volume, order, customComment, expiration, prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new TradeTransactionResponse(jsonObj.ToString());
    }

    public static async Task<TradeTransactionResponse> ExecuteTradeTransactionCommandAsync(ApiConnector connector,
        TradeTransInfoRecord tradeTransInfo,
        CancellationToken cancellationToken = default)
    {
        var command = CreateTradeTransactionCommand(tradeTransInfo);
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new TradeTransactionResponse(jsonObj.ToString());
    }

    public static async Task<TradeTransactionResponse> ExecuteTradeTransactionCommandAsync(ApiConnector connector,
        TRADE_OPERATION_TYPE tradeOperation,
        TRADE_TRANSACTION_TYPE transactionType,
        double? price,
        double? sl,
        double? tp,
        string symbol,
        double? volume,
        long? order,
        string customComment,
        DateTimeOffset? expiration,
        CancellationToken cancellationToken = default)
    {
        var command = CreateTradeTransactionCommand(tradeOperation, transactionType, price, sl, tp, symbol, volume, order, customComment, expiration);
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new TradeTransactionResponse(jsonObj.ToString());
    }

    public static TradeTransactionStatusResponse ExecuteTradeTransactionStatusCommand(ApiConnector connector, long? order, bool prettyPrint = false)
    {
        var command = CreateTradeTransactionStatusCommand(order, prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new TradeTransactionStatusResponse(jsonObj.ToString());
    }

    public static async Task<TradeTransactionStatusResponse> ExecuteTradeTransactionStatusCommandAsync(ApiConnector connector,
        long? order,
        CancellationToken cancellationToken = default)
    {
        var command = CreateTradeTransactionStatusCommand(order);
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new TradeTransactionStatusResponse(jsonObj.ToString());
    }

    public static TradesResponse ExecuteTradesCommand(ApiConnector connector, bool openedOnly, bool prettyPrint = false)
    {
        var command = CreateTradesCommand(openedOnly, prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new TradesResponse(jsonObj.ToString());
    }

    public static async Task<TradesResponse> ExecuteTradesCommandAsync(ApiConnector connector, bool openedOnly, CancellationToken cancellationToken = default)
    {
        var command = CreateTradesCommand(openedOnly);
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new TradesResponse(jsonObj.ToString());
    }

    public static TradesHistoryResponse ExecuteTradesHistoryCommand(ApiConnector connector,
        DateTimeOffset? start,
        DateTimeOffset? end = null,
        bool prettyPrint = false)
    {
        var command = CreateTradesHistoryCommand(start, end, prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new TradesHistoryResponse(jsonObj.ToString());
    }

    public static async Task<TradesHistoryResponse> ExecuteTradesHistoryCommandAsync(ApiConnector connector,
        DateTimeOffset? start,
        DateTimeOffset? end,
        CancellationToken cancellationToken = default)
    {
        var command = CreateTradesHistoryCommand(start, end);
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new TradesHistoryResponse(jsonObj.ToString());
    }

    public static TradingHoursResponse ExecuteTradingHoursCommand(ApiConnector connector, string[] symbols, bool prettyPrint = false)
    {
        var command = CreateTradingHoursCommand(symbols, prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new TradingHoursResponse(jsonObj.ToString());
    }

    public static async Task<TradingHoursResponse> ExecuteTradingHoursCommandAsync(ApiConnector connector, string[] symbols, CancellationToken cancellationToken = default)
    {
        var command = CreateTradingHoursCommand(symbols);
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new TradingHoursResponse(jsonObj.ToString());
    }

    public static VersionResponse ExecuteVersionCommand(ApiConnector connector)
    {
        var command = new VersionCommand();
        var jsonObj = connector.ExecuteCommand(command);

        return new VersionResponse(jsonObj.ToString());
    }

    public static async Task<VersionResponse> ExecuteVersionCommandAsync(ApiConnector connector, CancellationToken cancellationToken = default)
    {
        var command = new VersionCommand();
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new VersionResponse(jsonObj.ToString());
    }

    #endregion Command executors
}