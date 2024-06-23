using System;
using System.Linq;
using xAPI.Sync;
using xAPI.Commands;
using xAPI.Codes;
using System.Threading.Tasks;
using System.Globalization;

namespace xAPITest;

public sealed class AsyncExample : ExampleBase, IDisposable
{
    private readonly Server _server;
    private readonly Credentials _credentials;
    private SyncAPIConnector _connector;

    public AsyncExample(Server server, string user, string password)
    {
        _server = server;
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
    }

    public async Task ConnectionStage()
    {
        Stage("Connection");

        Action($"Establishing connection to '{_server.Address}:{_server.MainPort}'");
        try
        {
            _connector = new SyncAPIConnector(_server);
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
        }
        catch (Exception ex)
        {
            Fail(ex);
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
            Detail(response?.Margin_level?.ToString(CultureInfo.InvariantCulture) ?? "-");
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

    public void Dispose()
    {
        _connector?.Dispose();
        GC.SuppressFinalize(this);
    }
}
