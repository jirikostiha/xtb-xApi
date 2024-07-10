using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using xAPI.Codes;
using xAPI.Commands;
using xAPI.Sync;

namespace xAPITest;

public sealed class AsyncExample : ExampleBase
{
    private readonly Credentials _credentials;
    private readonly ApiConnector _connector;

    public AsyncExample(ApiConnector connector, string user, string password)
    {
        _connector = connector;
        _credentials = new Credentials(user, password);
    }

    public async Task Run()
    {
        await ConnectionStage();
        await AuthenticationStage();
        await AccountInfoStage();
        await MarketDataStage();
        await TradingStage();
        await TradingHistoryStage();
        await GlobalDataStage();
        await StreamingSubscriptionStage();
    }

    public async Task ConnectionStage()
    {
        Stage("Connection");

        Action($"Establishing connection");
        try
        {
            _connector.Connect();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex, true);
        }

        Action($"Dropping connection");
        try
        {
            _connector.Disconnect();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Reestablishing connection");
        try
        {
            _connector.Connect();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex, true);
        }

        Action("Ping");
        try
        {
            var response = await APICommandFactory.ExecutePingCommandAsync(_connector);
            Pass(response);
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action("Getting version");
        try
        {
            var response = await APICommandFactory.ExecuteVersionCommandAsync(_connector);
            Pass(response);
            Detail(response.Version);
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }

    public async Task AuthenticationStage()
    {
        Stage("Authentication");

        Action($"Logging in as '{_credentials.Login}'");
        try
        {
            var response = await APICommandFactory.ExecuteLoginCommandAsync(_connector, _credentials);
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
            var response = await APICommandFactory.ExecuteLogoutCommandAsync(_connector);
            Pass(response);
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Logging in again as '{_credentials.Login}'");
        try
        {
            _connector.Connect();
            var response = await APICommandFactory.ExecuteLoginCommandAsync(_connector, _credentials);
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
            var response = await APICommandFactory.ExecuteServerTimeCommandAsync(_connector);
            Pass(response);
            Detail(response.TimeString);
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }

    public async Task AccountInfoStage()
    {
        Stage("Account information");

        Action($"Getting user data");
        try
        {
            var response = await APICommandFactory.ExecuteCurrentUserDataCommandAsync(_connector);
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
            var response = await APICommandFactory.ExecuteMarginLevelCommandAsync(_connector);
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
            var response = await APICommandFactory.ExecuteAllSymbolsCommandAsync(_connector);
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
            var response = await APICommandFactory.ExecuteSymbolCommandAsync(_connector, "US500");
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
            var response = await APICommandFactory.ExecuteTradingHoursCommandAsync(_connector, ["US500"]);
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
            var response = await APICommandFactory.ExecuteTickPricesCommandAsync(_connector, ["US500"],
                TimeProvider.System.GetUtcNow().ToUnixTimeMilliseconds());
            Pass(response);
            Detail(response?.Ticks?.FirstOrDefault()?.High?.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }

    public async Task MarketDataStage()
    {
        Stage("Market data");

        Action($"Getting latest candles");
        try
        {
            var response = await APICommandFactory.ExecuteChartLastCommandAsync(_connector, "US500", PERIOD_CODE.PERIOD_H1,
                TimeProvider.System.GetUtcNow().AddDays(-10).ToUnixTimeMilliseconds());
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
            var response = await APICommandFactory.ExecuteChartRangeCommandAsync(_connector, "US500", PERIOD_CODE.PERIOD_H1,
                TimeProvider.System.GetUtcNow().AddDays(-20).ToUnixTimeMilliseconds(),
                TimeProvider.System.GetUtcNow().AddDays(-10).ToUnixTimeMilliseconds(),
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
            var response = await APICommandFactory.ExecuteCommissionDefCommandAsync(_connector, "US500", 1);
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
            var response = await APICommandFactory.ExecuteMarginTradeCommandAsync(_connector, "US500", 1);
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
            var response = await APICommandFactory.ExecuteProfitCalculationCommandAsync(_connector, "US500", 1, TRADE_OPERATION_CODE.BUY, 5000, 5100);
            Pass(response);
            Detail(response?.Profit?.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }

    public async Task TradingStage()
    {
        Stage("Trading");

        Action($"Getting all trades");
        try
        {
            var response = await APICommandFactory.ExecuteTradesCommandAsync(_connector, false);
            Pass(response);
            Detail(response?.TradeRecords?.Count.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Getting opened only trades");
        try
        {
            var response = await APICommandFactory.ExecuteTradesCommandAsync(_connector, true);
            Pass(response);
            Detail(response?.TradeRecords?.Count.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Getting trades for orders");
        try
        {
            var response = await APICommandFactory.ExecuteTradeRecordsCommandAsync(_connector, []);
            Pass(response);
            Detail(response?.TradeRecords?.Count.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }

    public async Task TradingHistoryStage()
    {
        Stage("Trading history");

        Action($"Getting passed trades");
        try
        {
            var response = await APICommandFactory.ExecuteTradesHistoryCommandAsync(_connector,
                TimeProvider.System.GetUtcNow().AddDays(-10).ToUnixTimeMilliseconds(),
                0);
            Pass(response);
            Detail(response?.TradeRecords?.Count.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }

    public async Task GlobalDataStage()
    {
        Stage("Global data");

        Action($"Getting news");
        try
        {
            var response = await APICommandFactory.ExecuteNewsCommandAsync(_connector,
                TimeProvider.System.GetUtcNow().AddDays(-10).ToUnixTimeMilliseconds(),
                0);
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
            var response = await APICommandFactory.ExecuteCalendarCommandAsync(_connector);
            Pass(response);
            Detail(response?.CalendarRecords?.Count.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }

    public async Task StreamingSubscriptionStage()
    {
        Stage("Streaming subscriptions");

        Action($"Connecting to streaming");
        try
        {
            _connector.Streaming.Connect(CancellationToken.None);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe keep alive");
        try
        {
            await _connector.Streaming.SubscribeKeepAliveAsync();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe keep alive");
        try
        {
            await _connector.Streaming.UnsubscribeKeepAliveAsync();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe balance");
        try
        {
            await _connector.Streaming.SubscribeBalanceAsync();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe balance");
        try
        {
            await _connector.Streaming.UnsubscribeBalanceAsync();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe news");
        try
        {
            await _connector.Streaming.SubscribeNewsAsync();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe news");
        try
        {
            await _connector.Streaming.UnsubscribeNewsAsync();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe profits");
        try
        {
            await _connector.Streaming.SubscribeProfitsAsync();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe profits");
        try
        {
            await _connector.Streaming.UnsubscribeProfitsAsync();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe trades");
        try
        {
            await _connector.Streaming.SubscribeTradesAsync();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe trades");
        try
        {
            await _connector.Streaming.UnsubscribeTradesAsync();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe trade status");
        try
        {
            await _connector.Streaming.SubscribeTradeStatusAsync();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe trade status");
        try
        {
            await _connector.Streaming.UnsubscribeTradeStatusAsync();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe candles");
        try
        {
            await _connector.Streaming.SubscribeCandlesAsync("US500");
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe candles");
        try
        {
            await _connector.Streaming.UnsubscribeCandlesAsync("US500");
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe price");
        try
        {
            await _connector.Streaming.SubscribePriceAsync("US500");
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe price");
        try
        {
            await _connector.Streaming.UnsubscribePriceAsync("US500");
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Subscribe prices");
        try
        {
            await _connector.Streaming.SubscribePricesAsync(["US500"]);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Unsubscribe prices");
        try
        {
            await _connector.Streaming.UnsubscribePricesAsync(["US500"]);
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }
}