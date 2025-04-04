﻿using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xtb.XApi.Client.Model;

namespace Xtb.XApi.Client.SystemTests;

public sealed class AsyncTest : XApiClientTestBase
{
    public AsyncTest(XApiClient client, string user, string password)
        : base(client, user, password)
    {
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        if (ShallLogTime)
            Time.Start();

        await ConnectionStage(cancellationToken);
        await AuthenticationStage(cancellationToken);
        await AccountInfoStage(cancellationToken);
        await MarketDataStage(cancellationToken);
        await GlobalDataStage(cancellationToken);
        await StreamingSubscriptionStage(cancellationToken);
        await TradingStage(cancellationToken);
        await TradingStage(cancellationToken);
        await TradingHistoryStage(cancellationToken);

        if (ShallLogTime)
            Time.Stop();
    }

    public async Task ConnectionStage(CancellationToken cancellationToken)
    {
        Stage("Connection");

        Action($"Establishing connection");
        try
        {
            await Client.ConnectAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex, true);
        }

        Action("Ping");
        try
        {
            var response = await Client.PingAsync(cancellationToken);
            Pass(response);
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Dropping connection");
        try
        {
            await Client.DisconnectAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Reestablishing connection");
        try
        {
            await Client.ConnectAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex, true);
        }

        Action("Ping");
        try
        {
            var response = await Client.PingAsync(cancellationToken);
            Pass(response);
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action("Getting version");
        try
        {
            var response = await Client.GetVersionAsync(cancellationToken);
            Pass(response);
            Detail(response.Version);
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }

    public async Task AuthenticationStage(CancellationToken cancellationToken)
    {
        Stage("Authentication");

        Action($"Logging in as '{Credentials.Login}'");
        try
        {
            var response = await Client.LoginAsync(Credentials, cancellationToken);
            Pass(response);
            Detail(response.StreamSessionId);
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Logging out");
        try
        {
            var response = await Client.LogoutAsync(cancellationToken);
            Pass(response);
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Logging in again as '{Credentials.Login}'");
        try
        {
            await Client.ConnectAsync(cancellationToken);
            var response = await Client.LoginAsync(Credentials, cancellationToken);
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
            var response = await Client.GetServerTimeAsync(cancellationToken);
            Pass(response);
            Detail(response.TimeString);
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }

    public async Task AccountInfoStage(CancellationToken cancellationToken)
    {
        Stage("Account information");

        Action($"Getting user data");
        try
        {
            var response = await Client.GetCurrentUserDataAsync(cancellationToken);
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
            var response = await Client.GetMarginLevelAsync(cancellationToken);
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
            var response = await Client.GetAllSymbolsAsync(cancellationToken);
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
            var response = await Client.GetSymbolAsync("US500", cancellationToken);
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
            var response = await Client.GetTradingHoursAsync(["US500"], cancellationToken);
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
            var response = await Client.GetTickPricesAsync(["US500"], 0,
                TimeProvider.System.GetUtcNow(), cancellationToken);
            Pass(response);
            Detail(response?.TickRecords?.FirstOrDefault()?.High?.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }

    public async Task MarketDataStage(CancellationToken cancellationToken)
    {
        Stage("Market data");

        Action($"Getting latest candles");
        try
        {
            var response = await Client.GetChartLastAsync("US500", PERIOD.H1,
                TimeProvider.System.GetUtcNow().AddDays(-10), cancellationToken);
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
            var response = await Client.GetChartRangeAsync("US500", PERIOD.H1,
                TimeProvider.System.GetUtcNow().AddDays(-20),
                TimeProvider.System.GetUtcNow().AddDays(-10),
                cancellationToken);
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
            var response = await Client.GetCommissionDefAsync("US500", 1, cancellationToken);
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
            var response = await Client.GetMarginTradeAsync("US500", 1, cancellationToken);
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
            var response = await Client.GetProfitCalculationAsync("US500",
                1,
                TRADE_OPERATION_TYPE.BUY,
                5000,
                5100,
                cancellationToken);
            Pass(response);
            Detail(response?.Profit?.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }

    public async Task GlobalDataStage(CancellationToken cancellationToken)
    {
        Stage("Global data");

        Action($"Getting news");
        try
        {
            var response = await Client.GetNewsAsync(
                TimeProvider.System.GetUtcNow().AddDays(-10),
                default,
                cancellationToken);
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
            var response = await Client.GetCalendarAsync(cancellationToken);
            Pass(response);
            Detail(response?.CalendarRecords?.Count.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }

    public async Task StreamingSubscriptionStage(CancellationToken cancellationToken)
    {
        Stage("Streaming subscriptions");

        Action($"Connecting to streaming");
        try
        {
            await Client.StreamingConnector.ConnectAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe keep alive");
        try
        {
            await Client.StreamingConnector.SubscribeKeepAliveAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe keep alive");
        try
        {
            await Client.StreamingConnector.UnsubscribeKeepAliveAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe balance");
        try
        {
            await Client.StreamingConnector.SubscribeBalanceAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe balance");
        try
        {
            await Client.StreamingConnector.UnsubscribeBalanceAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe news");
        try
        {
            await Client.StreamingConnector.SubscribeNewsAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe news");
        try
        {
            await Client.StreamingConnector.UnsubscribeNewsAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe profits");
        try
        {
            await Client.StreamingConnector.SubscribeProfitsAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe profits");
        try
        {
            await Client.StreamingConnector.UnsubscribeProfitsAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe trades");
        try
        {
            await Client.StreamingConnector.SubscribeTradesAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe trades");
        try
        {
            await Client.StreamingConnector.UnsubscribeTradesAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe trade status");
        try
        {
            await Client.StreamingConnector.SubscribeTradeStatusAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe trade status");
        try
        {
            await Client.StreamingConnector.UnsubscribeTradeStatusAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe candles");
        try
        {
            await Client.StreamingConnector.SubscribeCandlesAsync("US500", cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe candles");
        try
        {
            await Client.StreamingConnector.UnsubscribeCandlesAsync("US500", cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe price");
        try
        {
            await Client.StreamingConnector.SubscribePriceAsync("US500", null, null, cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe price");
        try
        {
            await Client.StreamingConnector.UnsubscribePriceAsync("US500", cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe prices");
        try
        {
            await Client.StreamingConnector.SubscribePricesAsync(["US500"], cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe prices");
        try
        {
            await Client.StreamingConnector.UnsubscribePricesAsync(["US500"], cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }

    public async Task TradingStage(CancellationToken cancellationToken)
    {
        Stage("Trading");

        Action($"Getting all trades");
        try
        {
            var response = await Client.GetTradesAsync(false, cancellationToken);
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
                var response = await Client.SetTradeTransactionAsync(trade, cancellationToken);
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
            var response = await Client.GetTradesAsync(true, cancellationToken);
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
                var response = await Client.SetTradeTransactionAsync(trade, cancellationToken);
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
            var response = await Client.GetTradeRecordsAsync([orderId], cancellationToken);
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
                var response = await Client.SetTradeTransactionAsync(trade, cancellationToken);
                Pass(response);
                Detail(response?.OrderId?.ToString(CultureInfo.InvariantCulture) ?? "-");
            }
            catch (Exception ex)
            {
                Fail(ex);
            }
        }
    }

    public async Task TradingHistoryStage(CancellationToken cancellationToken)
    {
        Stage("Trading history");

        Action($"Getting passed trades");
        try
        {
            var response = await Client.GetTradesHistoryAsync(
                TimeProvider.System.GetUtcNow().AddDays(-10),
                default,
                cancellationToken);
            Pass(response);
            Detail(response?.TradeRecords?.Length.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }
}