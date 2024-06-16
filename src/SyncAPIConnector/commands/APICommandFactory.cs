using System;
using System.Collections.Generic;
using xAPI.Codes;
using xAPI.Errors;
using xAPI.Sync;
using xAPI.Records;
using xAPI.Responses;
using System.Threading.Tasks;
using System.Text.Json.Nodes;
using System.Globalization;

namespace xAPI.Commands
{

    public static class APICommandFactory
    {
        /// <summary>
        /// Counts redirections.
        /// </summary>
        private static int redirectCounter;

        #region Command creators
        public static LoginCommand CreateLoginCommand(string userId, string password, bool prettyPrint = false)
        {
            JsonObject args = new JsonObject();
            args.Add("userId", userId);
            args.Add("password", password);
            args.Add("type", "dotNET");
            args.Add("version", SyncAPIConnector.VERSION);
            return new LoginCommand(args, prettyPrint);
        }

        [Obsolete("Up from 2.3.3 login is not a long, but string")]
        public static LoginCommand CreateLoginCommand(long? userId, string password, bool prettyPrint = false)
        {
            return CreateLoginCommand(userId?.ToString(CultureInfo.InvariantCulture) ?? string.Empty, password, prettyPrint);
        }

        public static LoginCommand CreateLoginCommand(Credentials credentials, bool prettyPrint = false)
        {
            JsonObject jsonObj = CreateLoginJsonObject(credentials);
            return new LoginCommand(jsonObj, prettyPrint);
        }

        private static JsonObject CreateLoginJsonObject(Credentials credentials)
        {
            JsonObject response = new JsonObject();
            if (credentials != null)
            {
                response.Add("userId", credentials.Login);
                response.Add("password", credentials.Password);
                response.Add("type", "dotNET");
                response.Add("version", SyncAPIConnector.VERSION);

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

        public static AllSymbolsCommand CreateAllSymbolsCommand(bool prettyPrint = false)
        {
            return new AllSymbolsCommand(prettyPrint);
        }

        public static CalendarCommand CreateCalendarCommand(bool prettyPrint = false)
        {
            return new CalendarCommand(prettyPrint);
        }

        public static ChartLastCommand CreateChartLastCommand(string symbol, PERIOD_CODE period, long? start, bool prettyPrint = false)
        {
            JsonObject args = new JsonObject();
            args.Add("info", (new ChartLastInfoRecord(symbol, period, start)).toJsonObject());
            return new ChartLastCommand(args, prettyPrint);
        }

        public static ChartLastCommand CreateChartLastCommand(ChartLastInfoRecord info, bool prettyPrint = false)
        {
            JsonObject args = new JsonObject();
            args.Add("info", info.toJsonObject());
            return new ChartLastCommand(args, prettyPrint);
        }

        public static ChartRangeCommand CreateChartRangeCommand(ChartRangeInfoRecord info, bool prettyPrint = false)
        {
            JsonObject args = new JsonObject();
            args.Add("info", info.toJsonObject());
            return new ChartRangeCommand(args, prettyPrint);

        }

        public static ChartRangeCommand CreateChartRangeCommand(string symbol, PERIOD_CODE period, long? start, long? end, long? ticks, bool prettyPrint = false)
        {
            JsonObject args = new JsonObject();
            args.Add("info", (new ChartRangeInfoRecord(symbol, period, start, end, ticks)).toJsonObject());
            return new ChartRangeCommand(args, prettyPrint);
        }

        public static CommissionDefCommand CreateCommissionDefCommand(string symbol, double? volume, bool prettyPrint = false)
        {
            JsonObject args = new JsonObject();
            args.Add("symbol", symbol);
            args.Add("volume", volume);
            return new CommissionDefCommand(args, prettyPrint);
        }

        public static LogoutCommand CreateLogoutCommand()
        {
            return new LogoutCommand();
        }

        public static MarginLevelCommand CreateMarginLevelCommand(bool prettyPrint = false)
        {
            return new MarginLevelCommand(prettyPrint);
        }

        public static MarginTradeCommand CreateMarginTradeCommand(string symbol, double? volume, bool prettyPrint = false)
        {
            JsonObject args = new JsonObject();
            args.Add("symbol", symbol);
            args.Add("volume", volume);
            return new MarginTradeCommand(args, prettyPrint);
        }

        public static NewsCommand CreateNewsCommand(long? start, long? end, bool prettyPrint = false)
        {
            JsonObject args = new JsonObject();
            args.Add("start", start);
            args.Add("end", end);
            return new NewsCommand(args, prettyPrint);
        }

        public static ServerTimeCommand CreateServerTimeCommand(bool prettyPrint = false)
        {
            return new ServerTimeCommand(prettyPrint);
        }

        public static CurrentUserDataCommand CreateCurrentUserDataCommand(bool prettyPrint = false)
        {
            return new CurrentUserDataCommand(prettyPrint);
        }

        public static IbsHistoryCommand CreateGetIbsHistoryCommand(long start, long end, bool prettyPrint = false)
        {
            JsonObject args = new JsonObject();
            args.Add("start", start);
            args.Add("end", end);
            return new IbsHistoryCommand(args, prettyPrint);
        }

        public static PingCommand CreatePingCommand(bool prettyPrint = false)
        {
            return new PingCommand(prettyPrint);
        }

        public static ProfitCalculationCommand CreateProfitCalculationCommand(string symbol, double? volume, TRADE_OPERATION_CODE cmd, double? openPrice, double? closePrice, bool prettyPrint = false)
        {
            JsonObject args = new JsonObject();
            args.Add("symbol", symbol);
            args.Add("volume", volume);
            args.Add("cmd", cmd.Code);
            args.Add("openPrice", openPrice);
            args.Add("closePrice", closePrice);
            return new ProfitCalculationCommand(args, prettyPrint);
        }

        [Obsolete("Command not available in API any more")]
        public static AllSymbolGroupsCommand CreateSymbolGroupsCommand(bool prettyPrint = false)
        {
            return null;
        }

        public static SymbolCommand CreateSymbolCommand(string symbol, bool prettyPrint = false)
        {
            JsonObject args = new JsonObject();
            args.Add("symbol", symbol);
            return new SymbolCommand(args, prettyPrint);
        }

        public static StepRulesCommand CreateStepRulesCommand(bool prettyPrint = false)
        {
            return new StepRulesCommand();
        }

        public static TickPricesCommand CreateTickPricesCommand(string[] symbols, long? timestamp, bool prettyPrint = false)
        {
            JsonObject args = new JsonObject();
            JsonArray arr = new JsonArray();
            foreach (string symbol in symbols)
            {
                arr.Add(symbol);
            }

            args.Add("symbols", arr);
            args.Add("timestamp", timestamp);
            return new TickPricesCommand(args, prettyPrint);
        }

        public static TradeRecordsCommand CreateTradeRecordsCommand(LinkedList<long?> orders, bool prettyPrint = false)
        {
            JsonObject args = new JsonObject();
            JsonArray arr = new JsonArray();
            foreach (long? order in orders)
            {
                arr.Add(order);
            }
            args.Add("orders", arr);
            return new TradeRecordsCommand(args, prettyPrint);
        }

        public static TradeTransactionCommand CreateTradeTransactionCommand(TradeTransInfoRecord tradeTransInfo, bool prettyPrint = false)
        {
            JsonObject args = new JsonObject();
            args.Add("tradeTransInfo", tradeTransInfo.toJsonObject());
            return new TradeTransactionCommand(args, prettyPrint);
        }

        public static TradeTransactionCommand CreateTradeTransactionCommand(TRADE_OPERATION_CODE cmd, TRADE_TRANSACTION_TYPE type, double? price, double? sl, double? tp, string symbol, double? volume, long? order, string customComment, long? expiration, bool prettyPrint = false)
        {
            JsonObject args = new JsonObject();
            args.Add("tradeTransInfo", (new TradeTransInfoRecord(cmd, type, price, sl, tp, symbol, volume, order, customComment, expiration)).toJsonObject());
            return new TradeTransactionCommand(args, prettyPrint);
        }

        [Obsolete("Method outdated. ie_deviation and comment are not available any more")]
        public static TradeTransactionCommand CreateTradeTransactionCommand(TRADE_OPERATION_CODE cmd, TRADE_TRANSACTION_TYPE type, double? price, double? sl, double? tp, string symbol, double? volume, long? ie_deviation, long? order, string comment, long? expiration, bool prettyPrint = false)
        {
            return CreateTradeTransactionCommand(cmd, type, price, sl, tp, symbol, volume, order, "", expiration);
        }

        public static TradeTransactionStatusCommand CreateTradeTransactionStatusCommand(long? order, bool prettyPrint = false)
        {
            JsonObject args = new JsonObject();
            args.Add("order", order);
            return new TradeTransactionStatusCommand(args, prettyPrint);
        }

        public static TradesCommand CreateTradesCommand(bool openedOnly, bool prettyPrint = false)
        {
            JsonObject args = new JsonObject();
            args.Add("openedOnly", openedOnly);
            return new TradesCommand(args, prettyPrint);
        }

        public static TradesHistoryCommand CreateTradesHistoryCommand(long? start, long? end, bool prettyPrint = false)
        {
            JsonObject args = new JsonObject();
            args.Add("start", start);
            args.Add("end", end);
            return new TradesHistoryCommand(args, prettyPrint);
        }

        public static TradingHoursCommand CreateTradingHoursCommand(string[] symbols, bool prettyPrint = false)
        {
            JsonObject args = new JsonObject();
            JsonArray arr = new JsonArray();
            foreach (string symbol in symbols)
            {
                arr.Add(symbol);
            }
            args.Add("symbols", arr);
            return new TradingHoursCommand(args, prettyPrint);
        }

        public static VersionCommand CreateVersionCommand(bool prettyPrint = false)
        {
            JsonObject args = new JsonObject();
            return new VersionCommand(args, prettyPrint);
        }
        #endregion

        #region Command executors

        public static AllSymbolsResponse ExecuteAllSymbolsCommand(SyncAPIConnector connector, bool prettyPrint = false)
        {
            return new AllSymbolsResponse(connector.ExecuteCommand(CreateAllSymbolsCommand(prettyPrint)).ToString());
        }

        public static async Task<AllSymbolsResponse> ExecuteAllSymbolsCommandAsync(SyncAPIConnector connector, bool prettyPrint = false)
        {
            var jsonObj = await connector.ExecuteCommandAsync(CreateAllSymbolsCommand(prettyPrint)).ConfigureAwait(false);
            return new AllSymbolsResponse(jsonObj.ToString());
        }

        public static CalendarResponse ExecuteCalendarCommand(SyncAPIConnector connector, bool prettyPrint = false)
        {
            return new CalendarResponse(connector.ExecuteCommand(CreateCalendarCommand(prettyPrint)).ToString());
        }

        public static async Task<CalendarResponse> ExecuteCalendarCommandAsync(SyncAPIConnector connector, bool prettyPrint = false)
        {
            var jsonObj = await connector.ExecuteCommandAsync(CreateCalendarCommand(prettyPrint)).ConfigureAwait(false);
            return new CalendarResponse(jsonObj.ToString());
        }
        public static ChartLastResponse ExecuteChartLastCommand(SyncAPIConnector connector, string symbol, PERIOD_CODE period, long? start, bool prettyPrint = false)
        {
            return new ChartLastResponse(connector.ExecuteCommand(CreateChartLastCommand(symbol, period, start, prettyPrint)).ToString());
        }

        public static async Task<ChartLastResponse> ExecuteChartLastCommandAsync(SyncAPIConnector connector, string symbol, PERIOD_CODE period, long? start, bool prettyPrint = false)
        {
            var jsonObj = await connector.ExecuteCommandAsync(CreateChartLastCommand(symbol, period, start, prettyPrint)).ConfigureAwait(false);
            return new ChartLastResponse(jsonObj.ToString());
        }

        public static ChartLastResponse ExecuteChartLastCommand(SyncAPIConnector connector, ChartLastInfoRecord info, bool prettyPrint = false)
        {
            return new ChartLastResponse(connector.ExecuteCommand(CreateChartLastCommand(info, prettyPrint)).ToString());
        }

        public static async Task<ChartLastResponse> ExecuteChartLastCommandAsync(SyncAPIConnector connector, ChartLastInfoRecord info, bool prettyPrint = false)
        {
            var jsonObj = await connector.ExecuteCommandAsync(CreateChartLastCommand(info, prettyPrint)).ConfigureAwait(false);
            return new ChartLastResponse(jsonObj.ToString());
        }

        public static ChartRangeResponse ExecuteChartRangeCommand(SyncAPIConnector connector, ChartRangeInfoRecord info, bool prettyPrint = false)
        {
            return new ChartRangeResponse(connector.ExecuteCommand(CreateChartRangeCommand(info, prettyPrint)).ToString());
        }

        public static async Task<ChartRangeResponse> ExecuteChartRangeCommandAsync(SyncAPIConnector connector, ChartRangeInfoRecord info, bool prettyPrint = false)
        {
            var jsonObj = await connector.ExecuteCommandAsync(CreateChartRangeCommand(info, prettyPrint)).ConfigureAwait(false);
            return new ChartRangeResponse(jsonObj.ToString());
        }

        public static ChartRangeResponse ExecuteChartRangeCommand(SyncAPIConnector connector, string symbol, PERIOD_CODE period, long? start, long? end, long? ticks, bool prettyPrint = false)
        {
            return new ChartRangeResponse(connector.ExecuteCommand(CreateChartRangeCommand(symbol, period, start, end, ticks, prettyPrint)).ToString());
        }

        public static async Task<ChartRangeResponse> ExecuteChartRangeCommandAsync(SyncAPIConnector connector, string symbol, PERIOD_CODE period, long? start, long? end, long? ticks, bool prettyPrint = false)
        {
            var jsonObj = await connector.ExecuteCommandAsync(CreateChartRangeCommand(symbol, period, start, end, ticks, prettyPrint)).ConfigureAwait(false);
            return new ChartRangeResponse(jsonObj.ToString());
        }

        public static CommissionDefResponse ExecuteCommissionDefCommand(SyncAPIConnector connector, string symbol, double? volume, bool prettyPrint = false)
        {
            return new CommissionDefResponse(connector.ExecuteCommand(CreateCommissionDefCommand(symbol, volume, prettyPrint)).ToString());
        }

        public static async Task<CommissionDefResponse> ExecuteCommissionDefCommandAsync(SyncAPIConnector connector, string symbol, double? volume, bool prettyPrint = false)
        {
            var jsonObj = await connector.ExecuteCommandAsync(CreateCommissionDefCommand(symbol, volume, prettyPrint));
            return new CommissionDefResponse(jsonObj.ToString());
        }

        public static IbsHistoryResponse ExecuteIbsHistoryCommand(SyncAPIConnector connector, long start, long end, bool prettyPrint = false)
        {
            return new IbsHistoryResponse(connector.ExecuteCommand(CreateGetIbsHistoryCommand(start, end, prettyPrint)).ToString());
        }

        public static async Task<IbsHistoryResponse> ExecuteIbsHistoryCommandAsync(SyncAPIConnector connector, long start, long end, bool prettyPrint = false)
        {
            var jsonObj = await connector.ExecuteCommandAsync(CreateGetIbsHistoryCommand(start, end, prettyPrint)).ConfigureAwait(false);
            return new IbsHistoryResponse(jsonObj.ToString());
        }

        [Obsolete("Up from 2.3.3 login is not a long, but string")]
        public static LoginResponse ExecuteLoginCommand(SyncAPIConnector connector, long userId, string password, bool prettyPrint = false)
        {
            return ExecuteLoginCommand(connector, userId.ToString(CultureInfo.InvariantCulture), password, prettyPrint);
        }

        public static LoginResponse ExecuteLoginCommand(SyncAPIConnector connector, string userId, string password, bool prettyPrint = false)
        {
            var credentials = new Credentials(userId, password);
            return ExecuteLoginCommand(connector, credentials, prettyPrint);
        }

        public static async Task<LoginResponse> ExecuteLoginCommandAsync(SyncAPIConnector connector, string userId, string password, bool prettyPrint = false)
        {
            var credentials = new Credentials(userId, password);
            return await ExecuteLoginCommandAsync(connector, credentials, prettyPrint).ConfigureAwait(false);
        }

        public static LoginResponse ExecuteLoginCommand(SyncAPIConnector connector, Credentials credentials, bool prettyPrint = false)
        {
            var loginCommand = CreateLoginCommand(credentials, prettyPrint);
            var loginResponse = new LoginResponse(connector.ExecuteCommand(loginCommand).ToString());

            redirectCounter = 0;

            while (loginResponse.RedirectRecord != null)
            {
                if (redirectCounter >= SyncAPIConnector.MAX_REDIRECTS)
                    throw new APICommunicationException($"Too many redirects ({redirectCounter}).");

                Server newServer = new Server(loginResponse.RedirectRecord.Address, loginResponse.RedirectRecord.MainPort, loginResponse.RedirectRecord.StreamingPort, true, "Redirected to: " + loginResponse.RedirectRecord.Address + ":" + loginResponse.RedirectRecord.MainPort + "/" + loginResponse.RedirectRecord.StreamingPort);
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

        public static async Task<LoginResponse> ExecuteLoginCommandAsync(SyncAPIConnector connector, Credentials credentials, bool prettyPrint = false)
        {
            var loginCommand = CreateLoginCommand(credentials, prettyPrint);
            var jsonObj = await connector.ExecuteCommandAsync(loginCommand).ConfigureAwait(false);
            var loginResponse = new LoginResponse(jsonObj.ToString());

            redirectCounter = 0;

            while (loginResponse.RedirectRecord != null)
            {
                if (redirectCounter >= SyncAPIConnector.MAX_REDIRECTS)
                    throw new APICommunicationException($"Too many redirects ({redirectCounter}).");

                var newServer = new Server(loginResponse.RedirectRecord.Address, loginResponse.RedirectRecord.MainPort, loginResponse.RedirectRecord.StreamingPort, true, "Redirected to: " + loginResponse.RedirectRecord.Address + ":" + loginResponse.RedirectRecord.MainPort + "/" + loginResponse.RedirectRecord.StreamingPort);
                connector.Redirect(newServer);
                redirectCounter++;
                var jsonObj2 = await connector.ExecuteCommandAsync(loginCommand).ConfigureAwait(false);
                loginResponse = new LoginResponse(jsonObj2.ToString());
            }

            if (loginResponse.StreamSessionId != null)
            {
                connector.Streaming.StreamSessionId = loginResponse.StreamSessionId;
            }

            return loginResponse;
        }

        public static LogoutResponse ExecuteLogoutCommand(SyncAPIConnector connector)
        {
            return new LogoutResponse(connector.ExecuteCommand(CreateLogoutCommand()).ToString());
        }

        public static async Task<LogoutResponse> ExecuteLogoutCommandAsync(SyncAPIConnector connector)
        {
            var jsonObj = await connector.ExecuteCommandAsync(CreateLogoutCommand()).ConfigureAwait(false);
            return new LogoutResponse(jsonObj.ToString());
        }

        public static MarginLevelResponse ExecuteMarginLevelCommand(SyncAPIConnector connector, bool prettyPrint = false)
        {
            return new MarginLevelResponse(connector.ExecuteCommand(CreateMarginLevelCommand(prettyPrint)).ToString());
        }

        public static async Task<MarginLevelResponse> ExecuteMarginLevelCommandAsync(SyncAPIConnector connector, bool prettyPrint = false)
        {
            var jsonObj = await connector.ExecuteCommandAsync(CreateMarginLevelCommand(prettyPrint)).ConfigureAwait(false);
            return new MarginLevelResponse(jsonObj.ToString());
        }

        public static MarginTradeResponse ExecuteMarginTradeCommand(SyncAPIConnector connector, string symbol, double? volume, bool prettyPrint = false)
        {
            return new MarginTradeResponse(connector.ExecuteCommand(CreateMarginTradeCommand(symbol, volume, prettyPrint)).ToString());
        }

        public static async Task<MarginTradeResponse> ExecuteMarginTradeCommandAsync(SyncAPIConnector connector, string symbol, double? volume, bool prettyPrint = false)
        {
            var jsonObj = await connector.ExecuteCommandAsync(CreateMarginTradeCommand(symbol, volume, prettyPrint));
            return new MarginTradeResponse(jsonObj.ToString());
        }

        public static NewsResponse ExecuteNewsCommand(SyncAPIConnector connector, long? start, long? end, bool prettyPrint = false)
        {
            return new NewsResponse(connector.ExecuteCommand(CreateNewsCommand(start, end, prettyPrint)).ToString());
        }

        public static async Task<NewsResponse> ExecuteNewsCommandAsync(SyncAPIConnector connector, long? start, long? end, bool prettyPrint = false)
        {
            var jsonObj = await connector.ExecuteCommandAsync(CreateNewsCommand(start, end, prettyPrint)).ConfigureAwait(false);
            return new NewsResponse(jsonObj.ToString());
        }

        public static ServerTimeResponse ExecuteServerTimeCommand(SyncAPIConnector connector, bool prettyPrint = false)
        {
            return new ServerTimeResponse(connector.ExecuteCommand(CreateServerTimeCommand(prettyPrint)).ToString());
        }

        public static async Task<ServerTimeResponse> ExecuteServerTimeCommandAsync(SyncAPIConnector connector, bool prettyPrint = false)
        {
            var jsonObj = await connector.ExecuteCommandAsync(CreateServerTimeCommand(prettyPrint)).ConfigureAwait(false);
            return new ServerTimeResponse(jsonObj.ToString());
        }

        public static CurrentUserDataResponse ExecuteCurrentUserDataCommand(SyncAPIConnector connector, bool prettyPrint = false)
        {
            return new CurrentUserDataResponse(connector.ExecuteCommand(CreateCurrentUserDataCommand(prettyPrint)).ToString());
        }

        public static async Task<CurrentUserDataResponse> ExecuteCurrentUserDataCommandAsync(SyncAPIConnector connector, bool prettyPrint = false)
        {
            var jsonObj = await connector.ExecuteCommandAsync(CreateCurrentUserDataCommand(prettyPrint)).ConfigureAwait(false);
            return new CurrentUserDataResponse(jsonObj.ToString());
        }

        public static PingResponse ExecutePingCommand(SyncAPIConnector connector, bool prettyPrint = false)
        {
            return new PingResponse(connector.ExecuteCommand(CreatePingCommand(prettyPrint)).ToString());
        }

        public static async Task<PingResponse> ExecutePingCommandAsync(SyncAPIConnector connector, bool prettyPrint = false)
        {
            var jsonObj = await connector.ExecuteCommandAsync(CreatePingCommand(prettyPrint)).ConfigureAwait(false);
            return new PingResponse(jsonObj.ToString());
        }

        public static ProfitCalculationResponse ExecuteProfitCalculationCommand(SyncAPIConnector connector, string symbol, double? volume, TRADE_OPERATION_CODE cmd, double? openPrice, double? closePrice, bool prettyPrint = false)
        {
            return new ProfitCalculationResponse(connector.ExecuteCommand(CreateProfitCalculationCommand(symbol, volume, cmd, openPrice, closePrice, prettyPrint)).ToString());
        }

        public static async Task<ProfitCalculationResponse> ExecuteProfitCalculationCommandAsync(SyncAPIConnector connector, string symbol, double? volume, TRADE_OPERATION_CODE cmd, double? openPrice, double? closePrice, bool prettyPrint = false)
        {
            var jsonObj = await connector.ExecuteCommandAsync(CreateProfitCalculationCommand(symbol, volume, cmd, openPrice, closePrice, prettyPrint)).ConfigureAwait(false);
            return new ProfitCalculationResponse(jsonObj.ToString());
        }

        public static StepRulesResponse ExecuteStepRulesCommand(SyncAPIConnector connector, bool prettyPrint = false)
        {
            return new StepRulesResponse(connector.ExecuteCommand(CreateStepRulesCommand(prettyPrint)).ToString());
        }

        public static SymbolResponse ExecuteSymbolCommand(SyncAPIConnector connector, string symbol, bool prettyPrint = false)
        {
            return new SymbolResponse(connector.ExecuteCommand(CreateSymbolCommand(symbol, prettyPrint)).ToString());
        }

        public static async Task<SymbolResponse> ExecuteSymbolCommandAsync(SyncAPIConnector connector, string symbol, bool prettyPrint = false)
        {
            var jsonObj = await connector.ExecuteCommandAsync(CreateSymbolCommand(symbol, prettyPrint)).ConfigureAwait(false);
            return new SymbolResponse(jsonObj.ToString());
        }

        public static TickPricesResponse ExecuteTickPricesCommand(SyncAPIConnector connector, string[] symbols, long? timestamp, bool prettyPrint = false)
        {
            return new TickPricesResponse(connector.ExecuteCommand(CreateTickPricesCommand(symbols, timestamp, prettyPrint)).ToString());
        }

        public static async Task<TickPricesResponse> ExecuteTickPricesCommandAsync(SyncAPIConnector connector, string[] symbols, long? timestamp, bool prettyPrint = false)
        {
            var jsonObj = await connector.ExecuteCommandAsync(CreateTickPricesCommand(symbols, timestamp, prettyPrint)).ConfigureAwait(false);
            return new TickPricesResponse(jsonObj.ToString());
        }

        public static TradeRecordsResponse ExecuteTradeRecordsCommand(SyncAPIConnector connector, LinkedList<long?> orders, bool prettyPrint = false)
        {
            return new TradeRecordsResponse(connector.ExecuteCommand(CreateTradeRecordsCommand(orders, prettyPrint)).ToString());
        }

        public static async Task<TradeRecordsResponse> ExecuteTradeRecordsCommandAsync(SyncAPIConnector connector, LinkedList<long?> orders, bool prettyPrint = false)
        {
            var jsonObj = await connector.ExecuteCommandAsync(CreateTradeRecordsCommand(orders, prettyPrint)).ConfigureAwait(false);
            return new TradeRecordsResponse(jsonObj.ToString());
        }

        public static TradeTransactionResponse ExecuteTradeTransactionCommand(SyncAPIConnector connector, TradeTransInfoRecord tradeTransInfo, bool prettyPrint = false)
        {
            return new TradeTransactionResponse(connector.ExecuteCommand(CreateTradeTransactionCommand(tradeTransInfo, prettyPrint)).ToString());
        }

        public static async Task<TradeTransactionResponse> ExecuteTradeTransactionCommandAsync(SyncAPIConnector connector, TradeTransInfoRecord tradeTransInfo, bool prettyPrint = false)
        {
            var jsonObj = await connector.ExecuteCommandAsync(CreateTradeTransactionCommand(tradeTransInfo, prettyPrint)).ConfigureAwait(false);
            return new TradeTransactionResponse(jsonObj.ToString());
        }

        public static TradeTransactionResponse ExecuteTradeTransactionCommand(SyncAPIConnector connector, TRADE_OPERATION_CODE cmd, TRADE_TRANSACTION_TYPE type, double? price, double? sl, double? tp, string symbol, double? volume, long? order, string customComment, long? expiration, bool prettyPrint = false)
        {
            return new TradeTransactionResponse(connector.ExecuteCommand(CreateTradeTransactionCommand(cmd, type, price, sl, tp, symbol, volume, order, customComment, expiration, prettyPrint)).ToString());
        }

        public static async Task<TradeTransactionResponse> ExecuteTradeTransactionCommandAsync(SyncAPIConnector connector, TRADE_OPERATION_CODE cmd, TRADE_TRANSACTION_TYPE type, double? price, double? sl, double? tp, string symbol, double? volume, long? order, string customComment, long? expiration, bool prettyPrint = false)
        {
            var jsonObj = await connector.ExecuteCommandAsync(CreateTradeTransactionCommand(cmd, type, price, sl, tp, symbol, volume, order, customComment, expiration, prettyPrint))
                .ConfigureAwait(false);
            return new TradeTransactionResponse(jsonObj.ToString());
        }

        [Obsolete("Method outdated. ie_deviation is not available any more")]
        public static TradeTransactionResponse ExecuteTradeTransactionCommand(SyncAPIConnector connector, TRADE_OPERATION_CODE cmd, TRADE_TRANSACTION_TYPE type, double? price, double? sl, double? tp, string symbol, double? volume, long? ie_deviation, long? order, string comment, long? expiration, bool prettyPrint = false)
        {
            return new TradeTransactionResponse(connector.ExecuteCommand(CreateTradeTransactionCommand(cmd, type, price, sl, tp, symbol, volume, order, "", expiration, prettyPrint)).ToString());
        }

        public static TradeTransactionStatusResponse ExecuteTradeTransactionStatusCommand(SyncAPIConnector connector, long? order, bool prettyPrint = false)
        {
            return new TradeTransactionStatusResponse(connector.ExecuteCommand(CreateTradeTransactionStatusCommand(order, prettyPrint)).ToString());
        }

        public static async Task<TradeTransactionStatusResponse> ExecuteTradeTransactionStatusCommandAsync(SyncAPIConnector connector, long? order, bool prettyPrint = false)
        {
            var jsonObj = await connector.ExecuteCommandAsync(CreateTradeTransactionStatusCommand(order, prettyPrint)).ConfigureAwait(false);
            return new TradeTransactionStatusResponse(jsonObj.ToString());
        }

        public static TradesResponse ExecuteTradesCommand(SyncAPIConnector connector, bool openedOnly, bool prettyPrint = false)
        {
            return new TradesResponse(connector.ExecuteCommand(CreateTradesCommand(openedOnly, prettyPrint)).ToString());
        }

        public static async Task<TradesResponse> ExecuteTradesCommandAsync(SyncAPIConnector connector, bool openedOnly, bool prettyPrint = false)
        {
            var jsonObj = await connector.ExecuteCommandAsync(CreateTradesCommand(openedOnly, prettyPrint)).ConfigureAwait(false);
            return new TradesResponse(jsonObj.ToString());
        }

        public static TradesHistoryResponse ExecuteTradesHistoryCommand(SyncAPIConnector connector, long? start, long? end, bool prettyPrint = false)
        {
            return new TradesHistoryResponse(connector.ExecuteCommand(CreateTradesHistoryCommand(start, end, prettyPrint)).ToString());
        }

        public static async Task<TradesHistoryResponse> ExecuteTradesHistoryCommandAsync(SyncAPIConnector connector, long? start, long? end, bool prettyPrint = false)
        {
            var jsonObj = await connector.ExecuteCommandAsync(CreateTradesHistoryCommand(start, end, prettyPrint)).ConfigureAwait(false);
            return new TradesHistoryResponse(jsonObj.ToString());
        }

        public static TradingHoursResponse ExecuteTradingHoursCommand(SyncAPIConnector connector, string[] symbols, bool prettyPrint = false)
        {
            return new TradingHoursResponse(connector.ExecuteCommand(CreateTradingHoursCommand(symbols, prettyPrint)).ToString());
        }

        public static async Task<TradingHoursResponse> ExecuteTradingHoursCommandAsync(SyncAPIConnector connector, string[] symbols, bool prettyPrint = false)
        {
            var jsonObj = await connector.ExecuteCommandAsync(CreateTradingHoursCommand(symbols, prettyPrint)).ConfigureAwait(false);
            return new TradingHoursResponse(jsonObj.ToString());
        }

        public static VersionResponse ExecuteVersionCommand(SyncAPIConnector connector, bool prettyPrint = false)
        {
            return new VersionResponse(connector.ExecuteCommand(CreateVersionCommand(prettyPrint)).ToString());
        }

        public static async Task<VersionResponse> ExecuteVersionCommandAsync(SyncAPIConnector connector, bool prettyPrint = false)
        {
            var jsonObj = await connector.ExecuteCommandAsync(CreateVersionCommand(prettyPrint)).ConfigureAwait(false);
            return new VersionResponse(jsonObj.ToString());
        }
        #endregion
    }
}