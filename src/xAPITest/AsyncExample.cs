using System;
using System.Linq;
using xAPI.Sync;
using xAPI.Commands;
using xAPI.Codes;
using System.Threading.Tasks;
using System.Globalization;

namespace xAPITest;

public sealed class AsyncExample : ExampleBase
{
    private readonly Credentials _credentials;
    private readonly ApiConnector _apiConnector;

    public AsyncExample(ApiConnector connector, string user, string password)
    {
        _credentials = new Credentials(user, password);
        _apiConnector = connector;
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
    }

    public async Task ConnectionStage()
    {
        Stage("Connection");

        Action($"Establishing connection to '{_apiConnector.Connector.Endpoint}'");
        try
        {
            _apiConnector.Connect();
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
            var response = await APICommandFactory.ExecutePingCommandAsync(_apiConnector);
            Pass(response);
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action("Getting version");
        try
        {
            var response = await APICommandFactory.ExecuteVersionCommandAsync(_apiConnector);
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
            var response = await APICommandFactory.ExecuteLoginCommandAsync(_apiConnector, _credentials);
            Pass(response);
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
            var response = await APICommandFactory.ExecuteLoginCommandAsync(_connector, _credentials);
            Pass(response);
        }
        catch (Exception ex)
        {
            Fail(ex, true);
        }

        Action("Getting server time");
        try
        {
            var response = await APICommandFactory.ExecuteServerTimeCommandAsync(_apiConnector);
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
            var response = await APICommandFactory.ExecuteCurrentUserDataCommandAsync(_apiConnector);
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
            var response = await APICommandFactory.ExecuteMarginLevelCommandAsync(_apiConnector);
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
            var response = await APICommandFactory.ExecuteAllSymbolsCommandAsync(_apiConnector);
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
            var response = await APICommandFactory.ExecuteSymbolCommandAsync(_apiConnector, "US500");
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
            var response = await APICommandFactory.ExecuteTradingHoursCommandAsync(_apiConnector, ["US500"]);
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
            var response = await APICommandFactory.ExecuteTickPricesCommandAsync(_apiConnector, ["US500"],
                TimeProvider.System.GetUtcNow().ToUnixTimeMilliseconds());
            Pass(response);
            Detail(response?.Ticks.FirstOrDefault()?.High?.ToString(CultureInfo.InvariantCulture) ?? "-");
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
            var response = await APICommandFactory.ExecuteChartLastCommandAsync(_apiConnector, "US500", PERIOD_CODE.PERIOD_H1,
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
            var response = await APICommandFactory.ExecuteChartRangeCommandAsync(_apiConnector, "US500", PERIOD_CODE.PERIOD_H1,
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
            var response = await APICommandFactory.ExecuteCommissionDefCommandAsync(_apiConnector, "US500", 1);
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
            var response = await APICommandFactory.ExecuteMarginTradeCommandAsync(_apiConnector, "US500", 1);
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
            var response = await APICommandFactory.ExecuteProfitCalculationCommandAsync(_apiConnector, "US500", 1, TRADE_OPERATION_CODE.BUY, 5000, 5100);
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
            var response = await APICommandFactory.ExecuteTradesCommandAsync(_apiConnector, false);
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
            var response = await APICommandFactory.ExecuteTradesCommandAsync(_apiConnector, true);
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
            var response = await APICommandFactory.ExecuteTradeRecordsCommandAsync(_apiConnector, []);
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
            var response = await APICommandFactory.ExecuteTradesHistoryCommandAsync(_apiConnector,
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
            var response = await APICommandFactory.ExecuteNewsCommandAsync(_apiConnector,
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
            var response = await APICommandFactory.ExecuteCalendarCommandAsync(_apiConnector);
            Pass(response);
            Detail(response?.CalendarRecords?.Count.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }
}
