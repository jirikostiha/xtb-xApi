using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using xAPI.Codes;
using xAPI.Commands;
using xAPI.Records;
using xAPI.Sync;

namespace xAPITest;

public sealed class AsyncExample : ExampleBase
{
    public AsyncExample(ApiConnector connector, string user, string password, string? messageFolder = null)
        : base(connector, user, password, messageFolder)
    {
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        await ConnectionStage(cancellationToken);
        await AuthenticationStage(cancellationToken);
        await AccountInfoStage(cancellationToken);
        await MarketDataStage(cancellationToken);
        await GlobalDataStage(cancellationToken);
        await StreamingSubscriptionStage(cancellationToken);
        await TradingStage(cancellationToken);
        await TradingStage(cancellationToken);
        await TradingHistoryStage(cancellationToken);
    }

    public async Task ConnectionStage(CancellationToken cancellationToken)
    {
        Stage("Connection");

        Action($"Establishing connection");
        try
        {
            await Connector.ConnectAsync(true, cancellationToken);
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
            await Connector.ConnectAsync(true, cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex, true);
        }

        Action("Ping");
        try
        {
            var response = await APICommandFactory.ExecutePingCommandAsync(Connector, cancellationToken);
            Pass(response);
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action("Getting version");
        try
        {
            var response = await APICommandFactory.ExecuteVersionCommandAsync(Connector, cancellationToken);
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
            var response = await APICommandFactory.ExecuteLoginCommandAsync(Connector, Credentials, cancellationToken);
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
            var response = await APICommandFactory.ExecuteLogoutCommandAsync(Connector, cancellationToken);
            Pass(response);
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Logging in again as '{Credentials.Login}'");
        try
        {
            await Connector.ConnectAsync(true, cancellationToken);
            var response = await APICommandFactory.ExecuteLoginCommandAsync(Connector, Credentials, cancellationToken);
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
            var response = await APICommandFactory.ExecuteServerTimeCommandAsync(Connector, cancellationToken);
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
            var response = await APICommandFactory.ExecuteCurrentUserDataCommandAsync(Connector, cancellationToken);
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
            var response = await APICommandFactory.ExecuteMarginLevelCommandAsync(Connector, cancellationToken);
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
            var response = await APICommandFactory.ExecuteAllSymbolsCommandAsync(Connector, cancellationToken);
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
            var response = await APICommandFactory.ExecuteSymbolCommandAsync(Connector, "US500", cancellationToken);
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
            var response = await APICommandFactory.ExecuteTradingHoursCommandAsync(Connector, ["US500"], cancellationToken);
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
            var response = await APICommandFactory.ExecuteTickPricesCommandAsync(Connector, ["US500"],
                TimeProvider.System.GetUtcNow().ToUnixTimeMilliseconds(), cancellationToken);
            Pass(response);
            Detail(response?.Ticks?.FirstOrDefault()?.High?.ToString(CultureInfo.InvariantCulture) ?? "-");
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
            var response = await APICommandFactory.ExecuteChartLastCommandAsync(Connector, "US500", PERIOD_CODE.PERIOD_H1,
                TimeProvider.System.GetUtcNow().AddDays(-10).ToUnixTimeMilliseconds(), cancellationToken);
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
            var response = await APICommandFactory.ExecuteChartRangeCommandAsync(Connector, "US500", PERIOD_CODE.PERIOD_H1,
                TimeProvider.System.GetUtcNow().AddDays(-20).ToUnixTimeMilliseconds(),
                TimeProvider.System.GetUtcNow().AddDays(-10).ToUnixTimeMilliseconds(),
                0,
                cancellationToken);
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
            var response = await APICommandFactory.ExecuteCommissionDefCommandAsync(Connector, "US500", 1, cancellationToken);
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
            var response = await APICommandFactory.ExecuteMarginTradeCommandAsync(Connector, "US500", 1, cancellationToken);
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
            var response = await APICommandFactory.ExecuteProfitCalculationCommandAsync(Connector, "US500",
                1,
                TRADE_OPERATION_CODE.BUY,
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
            var response = await APICommandFactory.ExecuteNewsCommandAsync(Connector,
                TimeProvider.System.GetUtcNow().AddDays(-10).ToUnixTimeMilliseconds(),
                0,
                cancellationToken);
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
            var response = await APICommandFactory.ExecuteCalendarCommandAsync(Connector, cancellationToken);
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
            await Connector.Streaming.ConnectAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe keep alive");
        try
        {
            await Connector.Streaming.SubscribeKeepAliveAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe keep alive");
        try
        {
            await Connector.Streaming.UnsubscribeKeepAliveAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe balance");
        try
        {
            await Connector.Streaming.SubscribeBalanceAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe balance");
        try
        {
            await Connector.Streaming.UnsubscribeBalanceAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe news");
        try
        {
            await Connector.Streaming.SubscribeNewsAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe news");
        try
        {
            await Connector.Streaming.UnsubscribeNewsAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe profits");
        try
        {
            await Connector.Streaming.SubscribeProfitsAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe profits");
        try
        {
            await Connector.Streaming.UnsubscribeProfitsAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe trades");
        try
        {
            await Connector.Streaming.SubscribeTradesAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe trades");
        try
        {
            await Connector.Streaming.UnsubscribeTradesAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe trade status");
        try
        {
            await Connector.Streaming.SubscribeTradeStatusAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe trade status");
        try
        {
            await Connector.Streaming.UnsubscribeTradeStatusAsync(cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe candles");
        try
        {
            await Connector.Streaming.SubscribeCandlesAsync("US500", cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe candles");
        try
        {
            await Connector.Streaming.UnsubscribeCandlesAsync("US500", cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe price");
        try
        {
            await Connector.Streaming.SubscribePriceAsync("US500", null, null, cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe price");
        try
        {
            await Connector.Streaming.UnsubscribePriceAsync("US500", cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe prices");
        try
        {
            await Connector.Streaming.SubscribePricesAsync(["US500"], cancellationToken);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe prices");
        try
        {
            await Connector.Streaming.UnsubscribePricesAsync(["US500"], cancellationToken);
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
            var response = await APICommandFactory.ExecuteTradesCommandAsync(Connector, false, cancellationToken);
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
                    TRADE_OPERATION_CODE.BUY,
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
                var response = await APICommandFactory.ExecuteTradeTransactionCommandAsync(Connector, trade, cancellationToken);
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
            var response = await APICommandFactory.ExecuteTradesCommandAsync(Connector, true, cancellationToken);
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
            var response = await APICommandFactory.ExecuteTradeRecordsCommandAsync(Connector, new([orderId]), cancellationToken);
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
                var response = await APICommandFactory.ExecuteTradeTransactionCommandAsync(Connector, trade, cancellationToken);
                Pass(response);
                Detail(response?.Order?.ToString(CultureInfo.InvariantCulture) ?? "-");
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
            var response = await APICommandFactory.ExecuteTradesHistoryCommandAsync(Connector,
                TimeProvider.System.GetUtcNow().AddDays(-10).ToUnixTimeMilliseconds(),
                0,
                cancellationToken);
            Pass(response);
            Detail(response?.TradeRecords?.Count.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }
}