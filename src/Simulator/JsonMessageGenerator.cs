using Bogus;
using System;
using System.Linq;
using Xtb.XApi.Streaming;

namespace Xtb.XApi.Simulation;

public class JsonMessageGenerator
{
    private readonly Faker _faker;

    public JsonMessageGenerator(Faker? faker = null)
    {
        _faker = faker ?? new Faker();
    }

	public AccountGeneratingOptions AccountOptions { get; set; } = new();

    public MarketGeneratingOptions MarketOptions { get; set; } = new();

    public CalendarGeneratingOptions CalendarOptions { get; set; } = new();

    public Func<DateTimeOffset> TimeProvider { get; init; } = new Func<DateTimeOffset>(() => DateTimeOffset.UtcNow);

    public long NewSessionId() => _faker.Random.Long(1000000000000000000, 8999999999999999999);

    #region Streaming
    //  public string GetStreamingPingResponse(bool pass = true)
    //  {
    //      return $$"""
    //{
    //	"command": "{{PingCommand.Name}}",
    //	"streamSessionId": "{{(pass ? SessionId : null)}}"
    //}
    //""";
    //  }

    public string GetStreamingKeepAliveResponse()
    {
        return $$"""
		{
			"command": "{{StreamingCommandName.KeepAlive}}",
			"data": {{GetStreamingKeepAliveRecord()}}
		}
		""";
    }

    public string GetStreamingKeepAliveRecord()
    {
        return $$"""
		{
			"timestamp": {{TimeProvider().ToUnixTimeMilliseconds()}}
		}
		""";
    }

    public string GetStreamingBalanceResponse()
    {
        return $$"""
		{
			"command": "{{StreamingCommandName.Balance}}",
			"data": {{GetStreamingBalanceRecord()}}
		}
		""";
    }

    public string GetStreamingBalanceRecord()
    {
        return $$"""
		{
			"balance": 995800269.43,
			"credit": 1000.00,
			"equity": 995985397.56,
			"margin": 572634.43,
			"marginFree": 995227635.00,
			"marginLevel": 173930.41
		}
		""";
    }

    public string GetProfitsResponse()
    {
        return $$"""
		{
			"command": "{{StreamingCommandName.Profit}}",
			"data": {{GetStreamingProfitsRecord()}}
		}
		""";
    }

    public string GetStreamingProfitsRecord()
    {
        return $$"""
		{
			"order": 7497776,
			"order2": 7497777,
			"position": 7497776,
			"profit": 7076.52
		}
		""";
    }

    public string GetStreamingCandlesResponse(string symbol)
    {
        return $$"""
		{
			"command": "{{StreamingCommandName.Candle}}",
			"data": {{GetStreamingCandlesRecord(symbol)}}
		}
		""";
    }

    public string GetStreamingCandlesRecord(string symbol)
    {
        return $$"""
		{
			"close": 4.1849,
			"ctm": {{TimeProvider().ToUnixTimeMilliseconds()}},
			"ctmString": "{{TimeProvider().ToString()}}",
			"high": 4.1854,
			"low": 4.1848,
			"open": 4.1848,
			"quoteId": 2,
			"symbol": "{{symbol}}",
			"vol": 0.0
		}
		""";
    }

    public string GetStreamingTradesResponse(long order)
    {
        return $$"""
		{
			"command": "{{StreamingCommandName.Trade}}",
			"data": {{GetStreamingTradesRecord(order)}}
		}
		""";
    }

    public string GetStreamingTradesRecord(long order)
    {
        return $$"""
		{
			"close_price": 1.3256,
			"close_time": null,
			"closed": false,
			"cmd": 0,
			"comment": "Web Trader",
			"commission": 0.0,
			"customComment": "Some text",
			"digits": 4,
			"expiration": null,
			"margin_rate": 3.9149000,
			"offset": 0,
			"open_price": 1.4,
			"open_time": 1272380927000,
			"order": {{order}},
			"order2": 1234567,
			"position": 1234567,
			"profit": 68.392,
			"sl": 0.0,
			"state": "Modified",
			"storage": -4.46,
			"symbol": "EURUSD",
			"tp": 0.0,
			"type": 0,
			"volume": 0.10
		}
		""";
    }

    public string GetStreamingTradeStatusResponse()
    {
        return $$"""
		{
			"command": "{{StreamingCommandName.TradeStatus}}",
			"data": {{GetStreamingTradeStatusRecord()}}
		}
		""";
    }

    public string GetStreamingTradeStatusRecord()
    {
        return $$"""
		{
			"customComment": "Some text",
			"message": null,
			"order": 43,
			"price": 1.392,
			"requestStatus": 3
		}
		""";
    }
    #endregion

    #region Requested

    public string GetPingResponse(bool pass = true)
    {
        return $$"""
		{
			"status": {{pass.ToString().ToLowerInvariant()}}
		}
		""";
    }

    public string GetServerTimeResponse()
    {
        return $$"""
		{
			"status": true,
			"returnData": {
				"time": {{TimeProvider().ToUnixTimeMilliseconds()}},
				"timeString": "Feb 12, 2014 2:22:59 PM"
			}
		}
		""";
    }

    public string GetLoginResponse(bool ok = true)
    {
        return $$"""
		{
			"status": {{ok.ToString().ToLowerInvariant()}},
			"streamSessionId": "8469308861804289383"
		}
		""";
    }

    public string GetLogoutResponse(bool ok = true)
    {
        return $$"""
		{
			"status": {{ok.ToString().ToLowerInvariant()}}
		}
		""";
    }

    public string GetVersionResponse(string version = "2.5.0")
    {
        return $$"""
		{
			"status": true,
			"returnData": {
				"version": "{{version}}"
			}
		}
		""";
    }

    public string GetCurrentUserDataResponse(string? currency = null)
    {
        return $$"""
		{
			"status": true,
			"returnData": {
				"currency": "{{currency ?? _faker.PickRandom(AccountOptions.Currencies)}}",
				"leverage": 1,
			}
		}
		""";
    }

    public string GetMarginLevelResponse()
	{
        return "";
    }

    public string GetMarginTradeResponse()
    {
		return "";
    }

    public string GetCalendarResponse()
    {
        return $$"""
		{
			"status": true,
			"returnData": [{{GetCalendarRecord()}}, {{GetCalendarRecord()}}]
		}
		""";
    }

    public string GetCalendarRecord(string? country = null, DateTimeOffset? time = null)
    {
        return $$"""
		{
			"country": "{{country ?? _faker.PickRandom(CalendarOptions.Countries)}}",
			"current": "",
			"forecast": "",
			"impact": "3",
			"period": "(FEB)",
			"previous": "58.3",
			"time": {{time?.ToUnixTimeMilliseconds()}},
			"title": "Ivey Purchasing Managers Index"
		}
		""";
    }

    public string GetNewsResponse()
	{
		return "";
	}

    public string GetSymbolResponse(string symbol)
    {
        return $$"""
		{
			"status": true,
			"returnData": {{GetSymbolRecord(symbol)}}
		}
		""";
    }

    public string GetAllSymbolsResponse()
    {
        return $$"""
		{
		    "status": true,
		    "returnData": [{{GetSymbolRecord("")}}, {{GetSymbolRecord("")}}]
		}
		""";
    }

    public string GetSymbolRecord(string? symbol = null)
    {
        return $$"""
		{
			"ask": 4000.0,
			"bid": 4000.0,
			"categoryName": "Forex",
			"contractSize": 100000,
			"currency": "XXX",
			"currencyPair": true,
			"currencyProfit": "SEK",
			"description": "USD/PLN",
			"expiration": null,
			"groupName": "Minor",
			"high": 4000.0,
			"initialMargin": 0,
			"instantMaxVolume": 0,
			"leverage": 1.5,
			"longOnly": false,
			"lotMax": 10.0,
			"lotMin": 0.1,
			"lotStep": 0.1,
			"low": 3500.0,
			"marginHedged": 0,
			"marginHedgedStrong": false,
			"marginMaintenance": null,
			"marginMode": 101,
			"percentage": 100.0,
			"precision": 2,
			"profitMode": 5,
			"quoteId": 1,
			"shortSelling": true,
			"spreadRaw": 0.000003,
			"spreadTable": 0.00042,
			"starting": null,
			"stepRuleId": 1,
			"stopsLevel": 0,
			"swap_rollover3days": 0,
			"swapEnable": true,
			"swapLong": -2.55929,
			"swapShort": 0.131,
			"swapType": 0,
			"symbol": "{{symbol ?? _faker.Random.String2(4)}}",
			"tickSize": 1.0,
			"tickValue": 1.0,
			"time": 1272446136891,
			"timeString": "Thu May 23 12:23:44 EDT 2013",
			"trailingEnabled": true,
			"type": 21
		}
		""";
    }

    public string GetTradingHoursResponse(string[] symbols)
    {
        var data = string.Join(", ", symbols.Select(GetTradingHoursRecord));
        return $$"""
		{
		    "status": true,
		    "returnData": [{{data}}]
		}
		""";
    }

    public string GetTradingHoursRecord(string symbol)
    {
        return $$"""
		{
		    "quotes": [{{GetQuotesRecord(1)}}, {{GetQuotesRecord(2)}}],
			"symbol": "{{symbol}}",
			"trading": [{{GetTradingRecord(1)}}, {{GetTradingRecord(2)}}]
		}
		""";
    }

    public string GetQuotesRecord(int day, long? since = null, long? until = null)
    {
        return $$"""
		{
			"day": {{day}},
			"fromT": {{since ?? _faker.PickRandom(MarketOptions.StartTradingTime)}},
			"toT": {{until ?? _faker.PickRandom(MarketOptions.StartTradingTime)}}
		}
		""";
    }

    public string GetTradingRecord(int day, long? since = null, long? until = null)
    {
        return $$"""
		{
			"day": {{day}},
			"fromT": {{since ?? _faker.PickRandom(MarketOptions.StartTradingTime)}},
			"toT": {{until ?? _faker.PickRandom(MarketOptions.StartTradingTime)}}
		}
		""";
    }

    public string GetChartLastResponse()
    {
        return $$"""
		{
			"status": true,
			"returnData": {
				"digits": 4,
				"rateInfos": [
					{{GetRateInfoRecord()}},
					{{GetRateInfoRecord()}}
				]
			}
		}
		""";
    }

    public string GetChartRangeResponse()
    {
        return $$"""
		{
			"status": true,
			"returnData": {
				"digits": 4,
				"rateInfos": [{{GetRateInfoRecord()}}, {{GetRateInfoRecord()}}]
			}
		}
		""";
    }

    public string GetRateInfoRecord()
    {
        return $$"""
		{
			"close": 1.0,
			"ctm": 1389362640000,
			"ctmString": "Jan 10, 2014 3:04:00 PM",
			"high": 6.0,
			"low": 0.0,
			"open": 41848.0,
			"vol": 0.0
		}
		""";
    }

    public string GetProfitCalculationResponse(double profit)
    {
        return $$"""
		{
			"status": true,
			"returnData": {
				"profit": {{profit}}
			}
		}
		""";
    }


    public string GetTickPricesResponse(string[] symbols)
    {
        var quotations = string.Join(", ", symbols.Select(GetTickRecord));
        return $$"""
		{
			"status": true,
			"returnData": {
				"quotations": [{{quotations}}]
			}
		}
		""";
    }

    public string GetTickRecord(string symbol, double? ask, double? bid, DateTimeOffset? time = null)
    {
        return $$"""
		{
			"ask": 4000.0,
			"askVolume": 15000,
			"bid": 4000.0,
			"bidVolume": 16000,
			"high": 4000.0,
			"level": 0,
			"low": 3500.0,
			"symbol": "{{symbol}}",
			"timestamp": {{time?.ToUnixTimeMilliseconds() ?? TimeProvider().ToUnixTimeMilliseconds()}}
		}
		""";
    }

    public string GetTradeRecordsResponse(long[] orders)
    {
        var data = string.Join(", ", orders.Select(GetTradeRecord));
        return $$"""
		{
			"status": true,
			"returnData": [{{data}}]
		}
		""";
    }

    public string GetTradeRecord(long order)
    {
        return $$"""
		{
			"close_price": 1.3256,
			"close_time": null,
			"close_timeString": null,
			"closed": false,
			"cmd": 0,
			"comment": "Web Trader",
			"commission": 0.0,
			"customComment": "Some text",
			"digits": 4,
			"expiration": null,
			"expirationString": null,
			"margin_rate": 0.0,
			"offset": 0,
			"open_price": 1.4,
			"open_time": 1272380927000,
			"open_timeString": "Fri Jan 11 10:03:36 CET 2013",
			"order": {{order}},
			"order2": 1234567,
			"position": 1234567,
			"profit": -2196.44,
			"sl": 0.0,
			"storage": -4.46,
			"symbol": "XXX",
			"timestamp": 1272540251000,
			"tp": 0.0,
			"volume": 0.10
		}
		""";
    }

    public string GetTradesResponse()
    {
        return $$"""
		{
			"status": true,
			"returnData": [{{GetTradeRecord(11)}}, {{GetTradeRecord(22)}}]
		}
		""";
    }

    public string GetTradesHistoryResponse()
    {
        return $$"""
		{
			"status": true,
			"returnData": [{{GetTradeRecord(11)}}, {{GetTradeRecord(22)}}]
		}
		""";
    }

    public string GetTradeTransactionResponse(long order)
    {
        return $$"""
		{
			"status": true,
			"returnData": {
				"order": {{order}}
			}
		}
		""";
    }

    public string GetTradeTransactionStatusResponse(long order)
    {
        return $$"""
		{
			"status": true,
			"returnData": {
				"ask": 1.392,
				"bid": 1.392,
				"customComment": "Some text",
				"message": null,
				"order": {{order}},
				"requestStatus": 3
			}
		}
		""";
    }

    public string GetCommissionDefResponse()
    {
        return $$"""
		{
			"status": true,
			"returnData": {
				"commission": 0.51,
				"rateOfExchange": 0.1609
			}
		}
		""";
    }


    public string GetMarginLevelResponse(string currency)
    {
        return $$"""
		{
			"status": true,
			"returnData": {
				"balance": 995800269.43,
				"credit": 1000.00,
				"currency": "{{currency}}",
				"equity": 995985397.56,
				"margin": 572634.43,
				"margin_free": 995227635.00,
				"margin_level": 173930.41
			}
		}
		""";
    }

    public string GetMarginTradeResponse(double margin)
    {
        return $$"""
		{
			"status": true,
			"returnData": {
				"margin": {{margin}}
			}
		}
		""";
    }

    public string GetNewsResponse(DateTimeOffset since, DateTimeOffset until)
    {
        return $$"""
		{
			"status": true,
			"returnData": [{{GetNewsTopicRecord()}}, {{GetNewsTopicRecord()}}]
		}
		""";
    }

    public string GetNewsTopicRecord(DateTimeOffset? time = null)
    {
        return $$"""
		{
			"body": "dd",
			"bodylen": 110,
			"key": "1f6da766abd29927aa854823f0105c23",
			"time": {{time?.ToUnixTimeMilliseconds() ?? TimeProvider().ToUnixTimeMilliseconds()}},
			"timeString": "May 17, 2013 4:30:00 PM",
			"title": "Breaking trend"
		}
		""";
    }
    #endregion
}
