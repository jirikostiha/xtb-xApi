using System;
using System.Globalization;
using System.Linq;
using xAPI.Codes;
using xAPI.Commands;
using xAPI.Records;
using xAPI.Sync;

namespace xAPITest;

public sealed class SyncExample : ExampleBase
{
    public SyncExample(ApiConnector connector, string user, string password, string? messageFolder = null)
     : base(connector, user, password, messageFolder)
    {
    }
    public void Run()
    {
        ConnectionStage();
        AuthenticationStage();
        AccountInfoStage();
        MarketDataStage();
        GlobalDataStage();
        StreamingSubscriptionStage();
        TradingStage();
        TradingStage();
        TradingHistoryStage();
    }

    public void ConnectionStage()
    {
        Stage("Connection");

        Action($"Establishing connection");
        try
        {
            Connector.Connect();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex, true);
        }

        Action($"Dropping connection");
        try
        {
            Connector.Disconnect();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Reestablishing connection");
        try
        {
            Connector.Connect();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex, true);
        }

        Action("Ping");
        try
        {
            var response = APICommandFactory.ExecutePingCommand(Connector);
            Pass(response);
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action("Getting version");
        try
        {
            var response = APICommandFactory.ExecuteVersionCommand(Connector);
            Pass(response);
            Detail(response.Version);
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }

    public void AuthenticationStage()
    {
        Stage("Authentication");

        Action($"Logging in as '{Credentials.Login}'");
        try
        {
            var response = APICommandFactory.ExecuteLoginCommand(Connector, Credentials);
            Pass(response);
            Detail(response.StreamSessionId);
        }
        catch (Exception ex)
        {
            Fail(ex, true);
        }

        Action($"Logging out");
        try
        {
            var response = APICommandFactory.ExecuteLogoutCommand(Connector);
            Pass(response);
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Logging in again as '{Credentials.Login}'");
        try
        {
            Connector.Connect();
            var response = APICommandFactory.ExecuteLoginCommand(Connector, Credentials);
            Pass(response);
            Detail(response.StreamSessionId);
        }
        catch (Exception ex)
        {
            Fail(ex, true);
        }

        Action("Getting server time");
        try
        {
            var response = APICommandFactory.ExecuteServerTimeCommand(Connector);
            Pass(response);
            Detail(response.TimeString);
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }

    public void AccountInfoStage()
    {
        Stage("Account information");

        Action($"Getting user data");
        try
        {
            var response = APICommandFactory.ExecuteCurrentUserDataCommand(Connector);
            Pass(response);
            Detail(response.Currency);
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Getting margin level");
        try
        {
            var response = APICommandFactory.ExecuteMarginLevelCommand(Connector);
            Pass(response);
            Detail(response?.MarginLevel?.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Getting all symbols");
        try
        {
            var response = APICommandFactory.ExecuteAllSymbolsCommand(Connector);
            Pass(response);
            Detail(response?.SymbolRecords?.Count.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Getting single symbol");
        try
        {
            var response = APICommandFactory.ExecuteSymbolCommand(Connector, "US500");
            Pass(response);
            Detail(response?.Symbol?.Bid?.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Getting trading hours");
        try
        {
            var response = APICommandFactory.ExecuteTradingHoursCommand(Connector, ["US500"]);
            Pass(response);
            Detail(response?.TradingHoursRecords?.Count.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Getting tick prices");
        try
        {
            var response = APICommandFactory.ExecuteTickPricesCommand(Connector, ["US500"], 0,
                TimeProvider.System.GetUtcNow());
            Pass(response);
            Detail(response?.Ticks?.FirstOrDefault()?.High?.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }

    public void MarketDataStage()
    {
        Stage("Market data");

        Action($"Getting latest candles");
        try
        {
            var response = APICommandFactory.ExecuteChartLastCommand(Connector, "US500", PERIOD.H1,
                TimeProvider.System.GetUtcNow().AddDays(-10));
            Pass(response);
            Detail(response?.RateInfos?.Count.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Getting candles in interval");
        try
        {
            var response = APICommandFactory.ExecuteChartRangeCommand(Connector, "US500", PERIOD.H1,
                TimeProvider.System.GetUtcNow().AddDays(-20),
                TimeProvider.System.GetUtcNow().AddDays(-10),
                0);
            Pass(response);
            Detail(response?.RateInfos?.Count.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Getting commissions");
        try
        {
            var response = APICommandFactory.ExecuteCommissionDefCommand(Connector, "US500", 1);
            Pass(response);
            Detail(response?.Commission?.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Getting margin calculation");
        try
        {
            var response = APICommandFactory.ExecuteMarginTradeCommand(Connector, "US500", 1);
            Pass(response);
            Detail(response?.Margin?.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Getting profit calculation");
        try
        {
            var response = APICommandFactory.ExecuteProfitCalculationCommand(Connector, "US500", 1, TRADE_OPERATION_TYPE.BUY, 5000, 5100);
            Pass(response);
            Detail(response?.Profit?.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }

    public void GlobalDataStage()
    {
        Stage("Global data");

        Action($"Getting news");
        try
        {
            var response = APICommandFactory.ExecuteNewsCommand(Connector,
                TimeProvider.System.GetUtcNow().AddDays(-10));
            Pass(response);
            Detail(response?.NewsTopicRecords?.Count.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Getting calendar events");
        try
        {
            var response = APICommandFactory.ExecuteCalendarCommand(Connector);
            Pass(response);
            Detail(response?.CalendarRecords?.Count.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }

    public void StreamingSubscriptionStage()
    {
        Stage("Streaming subscriptions");

        Action($"Connecting to streaming");
        try
        {
            Connector.Streaming.Connect();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe keep alive");
        try
        {
            Connector.Streaming.SubscribeKeepAlive();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe keep alive");
        try
        {
            Connector.Streaming.UnsubscribeKeepAlive();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe balance");
        try
        {
            Connector.Streaming.SubscribeBalance();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe balance");
        try
        {
            Connector.Streaming.UnsubscribeBalance();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe news");
        try
        {
            Connector.Streaming.SubscribeNews();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe news");
        try
        {
            Connector.Streaming.UnsubscribeNews();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe profits");
        try
        {
            Connector.Streaming.SubscribeProfits();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe profits");
        try
        {
            Connector.Streaming.UnsubscribeProfits();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe trades");
        try
        {
            Connector.Streaming.SubscribeTrades();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe trades");
        try
        {
            Connector.Streaming.UnsubscribeTrades();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe trade status");
        try
        {
            Connector.Streaming.SubscribeTradeStatus();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe trade status");
        try
        {
            Connector.Streaming.UnsubscribeTradeStatus();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe candles");
        try
        {
            Connector.Streaming.SubscribeCandles("US500");
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe candles");
        try
        {
            Connector.Streaming.UnsubscribeCandles("US500");
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe price");
        try
        {
            Connector.Streaming.SubscribePrice("US500");
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe price");
        try
        {
            Connector.Streaming.UnsubscribePrice("US500");
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe prices");
        try
        {
            Connector.Streaming.SubscribePrices(["US500"]);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe prices");
        try
        {
            Connector.Streaming.UnsubscribePrices(["US500"]);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }

    public void TradingStage()
    {
        Stage("Trading");

        Action($"Getting all trades");
        try
        {
            var response = APICommandFactory.ExecuteTradesCommand(Connector, false);
            Pass(response);
            Detail(response?.TradeRecords?.Count.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        long? orderId = null;
        if (ShallOpenTrades)
        {
            Action($"Opening long position.");
            try
            {
                var trade = new TradeTransInfoRecord(
                    TRADE_OPERATION_TYPE.BUY,
                    TRADE_TRANSACTION_TYPE.ORDER_OPEN,
                    price: null,
                    sl: null,
                    tp: null,
                    symbol: "US500",
                    volume: 0.1,
                    order: null,
                    customComment: "opened by test example",
                    expiration: null);

                // Warning: Opening trade. Make sure you have set up demo account!
                var response = APICommandFactory.ExecuteTradeTransactionCommand(Connector, trade, true);
                Pass(response);
                Detail(response?.Order?.ToString(CultureInfo.InvariantCulture) ?? "-");
                orderId = response?.Order;
            }
            catch (Exception ex)
            {
                Fail(ex);
            }
        }

        Action($"Getting opened only trades");
        try
        {
            var response = APICommandFactory.ExecuteTradesCommand(Connector, true);
            Pass(response);
            Detail(response?.TradeRecords?.Count.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }


        if (ShallOpenTrades)
        {
            Action($"Modifying position.");
            try
            {
                var trade = new TradeTransInfoRecord(
                    tradeOperation: null,
                    transactionType: TRADE_TRANSACTION_TYPE.ORDER_MODIFY,
                    price: null,
                    sl: null,
                    tp: null,
                    symbol: "US500",
                    volume: 0.2,
                    order: orderId,
                    customComment: "modified by test example",
                    expiration: null);

                // Warning: Make sure you have set up demo account!
                var response = APICommandFactory.ExecuteTradeTransactionCommand(Connector, trade, true);
                Pass(response);
                Detail(response?.Order?.ToString(CultureInfo.InvariantCulture) ?? "-");
            }
            catch (Exception ex)
            {
                Fail(ex);
            }
        }

        Action($"Getting trades for orders");
        try
        {
            var response = APICommandFactory.ExecuteTradeRecordsCommand(Connector, new([orderId]), true);
            Pass(response);
            Detail(response?.TradeRecords?.Count.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        if (ShallOpenTrades)
        {
            Action($"Closing position.");
            try
            {
                var trade = new TradeTransInfoRecord(
                    tradeOperation: null,
                    transactionType: TRADE_TRANSACTION_TYPE.ORDER_CLOSE,
                    price: null,
                    sl: null,
                    tp: null,
                    symbol: "US500",
                    volume: null,
                    order: orderId,
                    customComment: "closed by test example",
                    expiration: null);

                // Warning: Make sure you have set up demo account!
                var response = APICommandFactory.ExecuteTradeTransactionCommand(Connector, trade, true);
                Pass(response);
                Detail(response?.Order?.ToString(CultureInfo.InvariantCulture) ?? "-");
            }
            catch (Exception ex)
            {
                Fail(ex);
            }
        }
    }

    public void TradingHistoryStage()
    {
        Stage("Trading history");

        Action($"Getting passed trades");
        try
        {
            var response = APICommandFactory.ExecuteTradesHistoryCommand(Connector,
                TimeProvider.System.GetUtcNow().AddDays(-10));
            Pass(response);
            Detail(response?.TradeRecords?.Count.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }
}