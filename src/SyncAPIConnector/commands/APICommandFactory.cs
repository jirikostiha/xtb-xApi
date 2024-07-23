using System.Collections.Generic;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using xAPI.Codes;
using xAPI.Errors;
using xAPI.Records;
using xAPI.Responses;
using xAPI.Sync;

namespace xAPI.Commands;

public static class APICommandFactory
{
    #region Command creators

    public static LoginCommand CreateLoginCommand(string userId, string password, bool prettyPrint = false)
    {
        JsonObject args = new()
        {
            { "userId", userId },
            { "password", password },
            { "type", "dotNET" },
            { "version", ApiConnector.VERSION }
        };

        return new LoginCommand(args, prettyPrint);
    }

    public static LoginCommand CreateLoginCommand(Credentials credentials, bool prettyPrint = false)
    {
        JsonObject jsonObj = CreateLoginJsonObject(credentials);

        return new LoginCommand(jsonObj, prettyPrint);
    }

    private static JsonObject CreateLoginJsonObject(Credentials credentials)
    {
        JsonObject response = [];
        if (credentials != null)
        {
            response.Add("userId", credentials.Login);
            response.Add("password", credentials.Password);
            response.Add("type", "dotNET");
            response.Add("version", ApiConnector.VERSION);

            if (credentials.AppId != null)
            {
                response.Add("appId", credentials.AppId);
            }

            if (credentials.AppName != null)
            {
                response.Add("appName", credentials.AppName);
            }
        }

        return response;
    }

    public static ChartLastCommand CreateChartLastCommand(string symbol, PERIOD_CODE period, long? start, bool prettyPrint = false)
    {
        JsonObject args = new()
        {
            { "info", (new ChartLastInfoRecord(symbol, period, start)).ToJsonObject() }
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

    public static ChartRangeCommand CreateChartRangeCommand(string symbol, PERIOD_CODE period, long? start, long? end, long? ticks, bool prettyPrint = false)
    {
        JsonObject args = new()
        {
            { "info", (new ChartRangeInfoRecord(symbol, period, start, end, ticks)).ToJsonObject() }
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

    public static NewsCommand CreateNewsCommand(long? start, long? end, bool prettyPrint = false)
    {
        JsonObject args = new()
        {
            { "start", start },
            { "end", end }
        };

        return new NewsCommand(args, prettyPrint);
    }

    public static ProfitCalculationCommand CreateProfitCalculationCommand(string symbol, double? volume, TRADE_OPERATION_CODE cmd, double? openPrice, double? closePrice, bool prettyPrint = false)
    {
        JsonObject args = new()
        {
            { "symbol", symbol },
            { "volume", volume },
            { "cmd", cmd.Code },
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

    public static TickPricesCommand CreateTickPricesCommand(string[] symbols, int level, long? timestamp, bool prettyPrint = false)
    {
        JsonObject args = [];
        JsonArray arr = [.. symbols];
        args.Add("level", level);
        args.Add("symbols", arr);
        args.Add("timestamp", timestamp);

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

    public static TradeTransactionCommand CreateTradeTransactionCommand(TRADE_OPERATION_CODE cmd, TRADE_TRANSACTION_TYPE type, double? price, double? sl, double? tp, string symbol, double? volume, long? order, string customComment, long? expiration, bool prettyPrint = false)
    {
        JsonObject args = new()
        {
            { "tradeTransInfo", new TradeTransInfoRecord(cmd, type, price, sl, tp, symbol, volume, order, customComment, expiration).ToJsonObject() }
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

    public static TradesHistoryCommand CreateTradesHistoryCommand(long? start, long? end, bool prettyPrint = false)
    {
        JsonObject args = new()
        {
            { "start", start },
            { "end", end }
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

    public static ChartLastResponse ExecuteChartLastCommand(ApiConnector connector, string symbol, PERIOD_CODE period, long? start, bool prettyPrint = false)
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

    public static async Task<ChartLastResponse> ExecuteChartLastCommandAsync(ApiConnector connector, string symbol, PERIOD_CODE period, long? start, CancellationToken cancellationToken = default)
    {
        var command = CreateChartLastCommand(symbol, period, start);
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new ChartLastResponse(jsonObj.ToString());
    }

    public static async Task<ChartLastResponse> ExecuteChartLastCommandAsync(ApiConnector connector, ChartLastInfoRecord info, CancellationToken cancellationToken = default)
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

    public static ChartRangeResponse ExecuteChartRangeCommand(ApiConnector connector, string symbol, PERIOD_CODE period, long? start, long? end, long? ticks, bool prettyPrint = false)
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

    public static async Task<ChartRangeResponse> ExecuteChartRangeCommandAsync(ApiConnector connector, string symbol, PERIOD_CODE period, long? start, long? end, long? ticks, CancellationToken cancellationToken = default)
    {
        var command = CreateChartRangeCommand(symbol, period, start, end, ticks);
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new ChartRangeResponse(jsonObj.ToString());
    }

    public static CommissionDefResponse ExecuteCommissionDefCommand(ApiConnector connector, string symbol, double? volume, bool prettyPrint = false)
    {
        var command = CreateCommissionDefCommand(symbol, volume, prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new CommissionDefResponse(jsonObj.ToString());
    }

    public static async Task<CommissionDefResponse> ExecuteCommissionDefCommandAsync(ApiConnector connector, string symbol, double? volume, CancellationToken cancellationToken = default)
    {
        var command = CreateCommissionDefCommand(symbol, volume);
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new CommissionDefResponse(jsonObj.ToString());
    }

    public static LoginResponse ExecuteLoginCommand(ApiConnector connector, string userId, string password, bool prettyPrint = false)
    {
        var credentials = new Credentials(userId, password);
        var command = CreateLoginCommand(credentials, prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new LoginResponse(jsonObj.ToString());
    }

    public static LoginResponse ExecuteLoginCommand(ApiConnector connector, Credentials credentials, bool prettyPrint = false)
    {
        var loginCommand = CreateLoginCommand(credentials, prettyPrint);
        var loginResponse = new LoginResponse(connector.ExecuteCommand(loginCommand).ToString());

        var redirectCounter = 0;

        while (loginResponse.RedirectRecord != null)
        {
            if (redirectCounter >= ApiConnector.MAX_REDIRECTS)
                throw new APICommunicationException($"Too many redirects ({redirectCounter}).");

            var newServer = new Server(loginResponse.RedirectRecord.Address, loginResponse.RedirectRecord.MainPort, loginResponse.RedirectRecord.StreamingPort, true, "Redirected to: " + loginResponse.RedirectRecord.Address + ":" + loginResponse.RedirectRecord.MainPort + "/" + loginResponse.RedirectRecord.StreamingPort);
            connector.Redirect(newServer);
            redirectCounter++;
            loginResponse = new LoginResponse(connector.ExecuteCommand(loginCommand).ToString());
        }

        if (loginResponse.StreamSessionId != null)
        {
            connector.Streaming.StreamSessionId = loginResponse.StreamSessionId;
        }

        return loginResponse;
    }

    public static async Task<LoginResponse> ExecuteLoginCommandAsync(ApiConnector connector, string userId, string password, CancellationToken cancellationToken = default)
    {
        var credentials = new Credentials(userId, password);
        var command = CreateLoginCommand(credentials);
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new LoginResponse(jsonObj.ToString());
    }

    public static async Task<LoginResponse> ExecuteLoginCommandAsync(ApiConnector connector, Credentials credentials, CancellationToken cancellationToken = default)
    {
        var loginCommand = CreateLoginCommand(credentials);
        var jsonObj = await connector.ExecuteCommandAsync(loginCommand, cancellationToken).ConfigureAwait(false);
        var loginResponse = new LoginResponse(jsonObj.ToString());

        var redirectCounter = 0;

        while (loginResponse.RedirectRecord != null)
        {
            if (redirectCounter >= ApiConnector.MAX_REDIRECTS)
                throw new APICommunicationException($"Too many redirects ({redirectCounter}).");

            var newServer = new Server(loginResponse.RedirectRecord.Address, loginResponse.RedirectRecord.MainPort, loginResponse.RedirectRecord.StreamingPort, true, "Redirected to: " + loginResponse.RedirectRecord.Address + ":" + loginResponse.RedirectRecord.MainPort + "/" + loginResponse.RedirectRecord.StreamingPort);
            await connector.RedirectAsync(newServer, cancellationToken).ConfigureAwait(false);
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

    public static MarginTradeResponse ExecuteMarginTradeCommand(ApiConnector connector, string symbol, double? volume, bool prettyPrint = false)
    {
        var command = CreateMarginTradeCommand(symbol, volume, prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new MarginTradeResponse(jsonObj.ToString());
    }

    public static async Task<MarginTradeResponse> ExecuteMarginTradeCommandAsync(ApiConnector connector, string symbol, double? volume, CancellationToken cancellationToken = default)
    {
        var command = CreateMarginTradeCommand(symbol, volume);
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new MarginTradeResponse(jsonObj.ToString());
    }

    public static NewsResponse ExecuteNewsCommand(ApiConnector connector, long? start, long? end, bool prettyPrint = false)
    {
        var command = CreateNewsCommand(start, end, prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new NewsResponse(jsonObj.ToString());
    }

    public static async Task<NewsResponse> ExecuteNewsCommandAsync(ApiConnector connector, long? start, long? end, CancellationToken cancellationToken = default)
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

    public static ProfitCalculationResponse ExecuteProfitCalculationCommand(ApiConnector connector, string symbol, double? volume, TRADE_OPERATION_CODE cmd, double? openPrice, double? closePrice, bool prettyPrint = false)
    {
        var command = CreateProfitCalculationCommand(symbol, volume, cmd, openPrice, closePrice, prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new ProfitCalculationResponse(jsonObj.ToString());
    }

    public static async Task<ProfitCalculationResponse> ExecuteProfitCalculationCommandAsync(ApiConnector connector, string symbol, double? volume, TRADE_OPERATION_CODE cmd, double? openPrice, double? closePrice, CancellationToken cancellationToken = default)
    {
        var command = CreateProfitCalculationCommand(symbol, volume, cmd, openPrice, closePrice);
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

    public static TickPricesResponse ExecuteTickPricesCommand(ApiConnector connector, string[] symbols, int level, long? timestamp, bool prettyPrint = false)
    {
        var command = CreateTickPricesCommand(symbols, level, timestamp, prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new TickPricesResponse(jsonObj.ToString());
    }

    public static async Task<TickPricesResponse> ExecuteTickPricesCommandAsync(ApiConnector connector, string[] symbols, int level, long? timestamp, CancellationToken cancellationToken = default)
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

    public static TradeTransactionResponse ExecuteTradeTransactionCommand(ApiConnector connector, TRADE_OPERATION_CODE cmd, TRADE_TRANSACTION_TYPE type, double? price, double? sl, double? tp, string symbol, double? volume, long? order, string customComment, long? expiration, bool prettyPrint = false)
    {
        var command = CreateTradeTransactionCommand(cmd, type, price, sl, tp, symbol, volume, order, customComment, expiration, prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new TradeTransactionResponse(jsonObj.ToString());
    }

    public static async Task<TradeTransactionResponse> ExecuteTradeTransactionCommandAsync(ApiConnector connector, TradeTransInfoRecord tradeTransInfo, CancellationToken cancellationToken = default)
    {
        var command = CreateTradeTransactionCommand(tradeTransInfo);
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new TradeTransactionResponse(jsonObj.ToString());
    }

    public static async Task<TradeTransactionResponse> ExecuteTradeTransactionCommandAsync(ApiConnector connector, TRADE_OPERATION_CODE cmd, TRADE_TRANSACTION_TYPE type, double? price, double? sl, double? tp, string symbol, double? volume, long? order, string customComment, long? expiration, CancellationToken cancellationToken = default)
    {
        var command = CreateTradeTransactionCommand(cmd, type, price, sl, tp, symbol, volume, order, customComment, expiration);
        var jsonObj = await connector.ExecuteCommandAsync(command, cancellationToken).ConfigureAwait(false);

        return new TradeTransactionResponse(jsonObj.ToString());
    }

    public static TradeTransactionStatusResponse ExecuteTradeTransactionStatusCommand(ApiConnector connector, long? order, bool prettyPrint = false)
    {
        var command = CreateTradeTransactionStatusCommand(order, prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new TradeTransactionStatusResponse(jsonObj.ToString());
    }

    public static async Task<TradeTransactionStatusResponse> ExecuteTradeTransactionStatusCommandAsync(ApiConnector connector, long? order, CancellationToken cancellationToken = default)
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

    public static TradesHistoryResponse ExecuteTradesHistoryCommand(ApiConnector connector, long? start, long? end, bool prettyPrint = false)
    {
        var command = CreateTradesHistoryCommand(start, end, prettyPrint);
        var jsonObj = connector.ExecuteCommand(command);

        return new TradesHistoryResponse(jsonObj.ToString());
    }

    public static async Task<TradesHistoryResponse> ExecuteTradesHistoryCommandAsync(ApiConnector connector, long? start, long? end, CancellationToken cancellationToken = default)
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