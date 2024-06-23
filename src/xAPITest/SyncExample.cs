using System;
using System.Linq;
using xAPI.Sync;
using xAPI.Commands;
using xAPI.Codes;
using System.Globalization;

namespace xAPITest;

public sealed class SyncExample : ExampleBase, IDisposable
{
    private readonly Server _server;
    private readonly Credentials _credentials;
    private SyncAPIConnector _connector;

    public SyncExample(Server server, string user, string password)
    {
        _server = server;
        _credentials = new Credentials(user, password);
    }

    public void Run()
    {
        ConnectionStage();
        AuthenticationStage();
        AccountInfoStage();
        MarketDataStage();
        TradingStage();
        TradingHistoryStage();
        GlobalDataStage();
    }

    public void ConnectionStage()
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
            var response = APICommandFactory.ExecutePingCommand(_connector);
            Pass(response);
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action("Getting version");
        try
        {
            var response = APICommandFactory.ExecuteVersionCommand(_connector);
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

        Action($"Logging in as '{_credentials.Login}'");
        try
        {
            var response = APICommandFactory.ExecuteLoginCommand(_connector, _credentials);
            Pass(response);
        }
        catch (Exception ex)
        {
            Fail(ex, true);
        }

        Action("Getting server time");
        try
        {
            var response = APICommandFactory.ExecuteServerTimeCommand(_connector);
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
            var response = APICommandFactory.ExecuteCurrentUserDataCommand(_connector);
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
            var response = APICommandFactory.ExecuteMarginLevelCommand(_connector);
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
            var response = APICommandFactory.ExecuteAllSymbolsCommand(_connector);
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
            var response = APICommandFactory.ExecuteSymbolCommand(_connector, "US500");
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
            var response = APICommandFactory.ExecuteTradingHoursCommand(_connector, ["US500"]);
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
            var response = APICommandFactory.ExecuteTickPricesCommand(_connector, ["US500"],
                TimeProvider.System.GetUtcNow().ToUnixTimeMilliseconds());
            Pass(response);
            Detail(response?.Ticks.FirstOrDefault()?.High?.ToString(CultureInfo.InvariantCulture) ?? "-");
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
            var response = APICommandFactory.ExecuteChartLastCommand(_connector, "US500", PERIOD_CODE.PERIOD_H1,
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
            var response = APICommandFactory.ExecuteChartRangeCommand(_connector, "US500", PERIOD_CODE.PERIOD_H1,
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
            var response = APICommandFactory.ExecuteCommissionDefCommand(_connector, "US500", 1);
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
            var response = APICommandFactory.ExecuteMarginTradeCommand(_connector, "US500", 1);
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
            var response = APICommandFactory.ExecuteProfitCalculationCommand(_connector, "US500", 1, TRADE_OPERATION_CODE.BUY, 5000, 5100);
            Pass(response);
            Detail(response?.Profit?.ToString(CultureInfo.InvariantCulture) ?? "-");
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
            var response = APICommandFactory.ExecuteTradesCommand(_connector, false);
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
            var response = APICommandFactory.ExecuteTradesCommand(_connector, true);
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
            var response = APICommandFactory.ExecuteTradeRecordsCommand(_connector, []);
            Pass(response);
            Detail(response?.TradeRecords?.Count.ToString(CultureInfo.InvariantCulture) ?? "-");
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }

    public void TradingHistoryStage()
    {
        Stage("Trading history");

        Action($"Getting passed trades");
        try
        {
            var response = APICommandFactory.ExecuteTradesHistoryCommand(_connector,
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

    public void GlobalDataStage()
    {
        Stage("Global data");

        Action($"Getting news");
        try
        {
            var response = APICommandFactory.ExecuteNewsCommand(_connector,
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
            var response = APICommandFactory.ExecuteCalendarCommand(_connector);
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
