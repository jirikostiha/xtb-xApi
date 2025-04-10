﻿using System;
using System.Globalization;
using System.Linq;
using Xtb.XApi.Client.Model;

namespace Xtb.XApi.Client.SystemTests;

public sealed class SyncTest : XApiClientTestBase
{
    public SyncTest(XApiClient client, string user, string password)
        : base(client, user, password)
    {
    }

    public void Run()
    {
        if (ShallLogTime)
            Time.Start();

        ConnectionStage();
        AuthenticationStage();
        AccountInfoStage();
        MarketDataStage();
        GlobalDataStage();
        StreamingSubscriptionStage();
        TradingStage();
        TradingStage();
        TradingHistoryStage();

        if (ShallLogTime)
            Time.Stop();
    }

    public void ConnectionStage()
    {
        Stage("Connection");

        Action($"Establishing connection");
        try
        {
            Client.Connect();
            Pass();
            Detail($"endpoint:{Client.ApiConnector.Endpoint}");
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
            Detail($"endpoint:{Client.ApiConnector.Endpoint}");
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
            Detail(response.CurrentUserDataRecord?.Currency);
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
            Detail(response?.MarginLevelRecord?.MarginLevel?.ToString(CultureInfo.InvariantCulture) ?? "-");
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
            Detail(response?.SymbolRecords?.Length.ToString(CultureInfo.InvariantCulture) ?? "-");
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
            Detail(response?.SymbolRecord?.Bid?.ToString(CultureInfo.InvariantCulture) ?? "-");
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
            Detail(response?.TradingHoursRecords?.Length.ToString(CultureInfo.InvariantCulture) ?? "-");
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
            Detail(response?.TickRecords?.FirstOrDefault()?.High?.ToString(CultureInfo.InvariantCulture) ?? "-");
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
            Detail(response?.RateInfoRecords?.Length.ToString(CultureInfo.InvariantCulture) ?? "-");
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
            Detail(response?.RateInfoRecords?.Length.ToString(CultureInfo.InvariantCulture) ?? "-");
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
            Detail(response?.NewsTopicRecords?.Length.ToString(CultureInfo.InvariantCulture) ?? "-");
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
            Client.StreamingConnector.Connect();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe keep alive");
        try
        {
            Client.StreamingConnector.SubscribeKeepAlive();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe keep alive");
        try
        {
            Client.StreamingConnector.UnsubscribeKeepAlive();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe balance");
        try
        {
            Client.StreamingConnector.SubscribeBalance();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe balance");
        try
        {
            Client.StreamingConnector.UnsubscribeBalance();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe news");
        try
        {
            Client.StreamingConnector.SubscribeNews();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe news");
        try
        {
            Client.StreamingConnector.UnsubscribeNews();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe profits");
        try
        {
            Client.StreamingConnector.SubscribeProfits();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe profits");
        try
        {
            Client.StreamingConnector.UnsubscribeProfits();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe trades");
        try
        {
            Client.StreamingConnector.SubscribeTrades();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe trades");
        try
        {
            Client.StreamingConnector.UnsubscribeTrades();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe trade status");
        try
        {
            Client.StreamingConnector.SubscribeTradeStatus();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe trade status");
        try
        {
            Client.StreamingConnector.UnsubscribeTradeStatus();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe candles");
        try
        {
            Client.StreamingConnector.SubscribeCandles("US500");
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe candles");
        try
        {
            Client.StreamingConnector.UnsubscribeCandles("US500");
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe price");
        try
        {
            Client.StreamingConnector.SubscribePrice("US500");
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe price");
        try
        {
            Client.StreamingConnector.UnsubscribePrice("US500");
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe prices");
        try
        {
            Client.StreamingConnector.SubscribePrices(["US500"]);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe prices");
        try
        {
            Client.StreamingConnector.UnsubscribePrices(["US500"]);
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
                    orderId: null,
                    customComment: "opened by test",
                    expiration: null);

                // Warning: Opening trade. Make sure you have set up demo account!
                var response = Client.SetTradeTransaction(trade);
                Pass(response);
                Detail(response?.OrderId?.ToString(CultureInfo.InvariantCulture) ?? "-");
                orderId = response?.OrderId;
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
                    orderId: orderId,
                    customComment: "modified by test",
                    expiration: null);

                // Warning: Make sure you have set up demo account!
                var response = Client.SetTradeTransaction(trade);
                Pass(response);
                Detail(response?.OrderId?.ToString(CultureInfo.InvariantCulture) ?? "-");
            }
            catch (Exception ex)
            {
                Fail(ex);
            }
        }

        Action($"Getting trades for orders");
        try
        {
            var response = Client.GetTradeRecords([orderId]);
            Pass(response);
            Detail(response?.TradeRecords?.Length.ToString(CultureInfo.InvariantCulture) ?? "-");
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
                    orderId: orderId,
                    customComment: "closed by test",
                    expiration: null);

                // Warning: Make sure you have set up demo account!
                var response = Client.SetTradeTransaction(trade);
                Pass(response);
                Detail(response?.OrderId?.ToString(CultureInfo.InvariantCulture) ?? "-");
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
            Detail(response?.TradeRecords?.Length.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }
}