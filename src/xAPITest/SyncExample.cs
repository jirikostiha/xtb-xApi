using System;
using System.Globalization;
using System.Linq;
using Xtb.XApi;
using Xtb.XApi.Codes;
using Xtb.XApi.Records;

namespace Xtb.XApiTest;

public sealed class SyncExample : ExampleBase
{
    public SyncExample(XApiClient client, string user, string password, string? messageFolder = null)
     : base(client, user, password, messageFolder)
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
            Client.Connect();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex, true);
        }

        Action($"Dropping connection");
        try
        {
            Client.Disconnect();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Reestablishing connection");
        try
        {
            Client.Connect();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex, true);
        }

        Action("Ping");
        try
        {
            var response = Client.Ping();
            Pass(response);
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action("Getting version");
        try
        {
            var response = Client.GetVersion();
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
            var response = Client.Login(Credentials);
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
            var response = Client.Logout();
            Pass(response);
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Logging in again as '{Credentials.Login}'");
        try
        {
            Client.Connect();
            var response = Client.Login(Credentials);
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
            var response = Client.GetServerTime();
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
            var response = Client.GetCurrentUserData();
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
            var response = Client.GetMarginLevel();
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
            var response = Client.GetAllSymbols();
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
            var response = Client.GetSymbol("US500");
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
            var response = Client.GetTradingHours(["US500"]);
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
            var response = Client.GetTickPrices(["US500"], 0,
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
            var response = Client.GetChartLast("US500", PERIOD.H1,
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
            var response = Client.GetChartRange("US500", PERIOD.H1,
                TimeProvider.System.GetUtcNow().AddDays(-20),
                TimeProvider.System.GetUtcNow().AddDays(-10));
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
            var response = Client.GetCommissionDef("US500", 1);
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
            var response = Client.GetMarginTrade("US500", 1); //??
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
            var response = Client.GetProfitCalculation("US500", 1, TRADE_OPERATION_TYPE.BUY, 5000, 5100);
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
            var response = Client.GetNews(TimeProvider.System.GetUtcNow().AddDays(-10));
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
            var response = Client.GetCalendar();
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
            Client.Streaming.Connect();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe keep alive");
        try
        {
            Client.Streaming.SubscribeKeepAlive();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe keep alive");
        try
        {
            Client.Streaming.UnsubscribeKeepAlive();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe balance");
        try
        {
            Client.Streaming.SubscribeBalance();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe balance");
        try
        {
            Client.Streaming.UnsubscribeBalance();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe news");
        try
        {
            Client.Streaming.SubscribeNews();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe news");
        try
        {
            Client.Streaming.UnsubscribeNews();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe profits");
        try
        {
            Client.Streaming.SubscribeProfits();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe profits");
        try
        {
            Client.Streaming.UnsubscribeProfits();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe trades");
        try
        {
            Client.Streaming.SubscribeTrades();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe trades");
        try
        {
            Client.Streaming.UnsubscribeTrades();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe trade status");
        try
        {
            Client.Streaming.SubscribeTradeStatus();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe trade status");
        try
        {
            Client.Streaming.UnsubscribeTradeStatus();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe candles");
        try
        {
            Client.Streaming.SubscribeCandles("US500");
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe candles");
        try
        {
            Client.Streaming.UnsubscribeCandles("US500");
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe price");
        try
        {
            Client.Streaming.SubscribePrice("US500");
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe price");
        try
        {
            Client.Streaming.UnsubscribePrice("US500");
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe prices");
        try
        {
            Client.Streaming.SubscribePrices(["US500"]);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe prices");
        try
        {
            Client.Streaming.UnsubscribePrices(["US500"]);
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
            var response = Client.GetTrades(false);
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
                var response = Client.GetTradeTransaction(trade);
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
            var response = Client.GetTrades(true);
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
                var response = Client.GetTradeTransaction(trade);
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
            var response = Client.GetTradeRecords(new([orderId]));
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
                var response = Client.GetTradeTransaction(trade);
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
            var response = Client.GetTradesHistory(TimeProvider.System.GetUtcNow().AddDays(-10));
            Pass(response);
            Detail(response?.TradeRecords?.Count.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }
}