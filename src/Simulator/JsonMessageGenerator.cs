using Bogus;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xtb.XApi.Streaming;

namespace Xtb.XApi.Simulation;

public class JsonMessageGenerator
{
    private readonly Faker _faker;

	private long _prevOrder = 1000000;

    public JsonMessageGenerator(Faker? faker = null)
    {
        _faker = faker ?? new Faker();
    }

	public AccountGeneratingOptions AccountOptions { get; set; } = new();

    public MarketGeneratingOptions MarketOptions { get; set; } = new();

    public CalendarGeneratingOptions CalendarOptions { get; set; } = new();

    public Func<DateTimeOffset> TimeProvider { get; init; } = new Func<DateTimeOffset>(() => DateTimeOffset.UtcNow);

    public long NewSessionId() => _faker.Random.Long(1000000000000000000, 8999999999999999999);

	public long NewOrderId() => _prevOrder++;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
    public string GetPingResponse(bool pass = true)
    {
        return $$"""
		{
			"status": {{pass.ToString().ToLowerInvariant()}}
		}
		""";
    }

	public string GetStreamingKeepAliveResponse(DateTimeOffset? time = null)
    {
        return $$"""
		{
			"command": "{{StreamingCommandName.KeepAlive}}",
			"data": {{GetStreamingKeepAliveRecord(time)}}
		}
		""";
    }

    public string GetStreamingKeepAliveRecord(DateTimeOffset? time = null)
    {
        var timeValue = time ?? TimeProvider();

		return $$"""
		{
			"timestamp": {{timeValue.ToUnixTimeMilliseconds()}}
		}
		""";
    }

    public string GetStreamingBalanceResponse(double? balance = null, double? equity = null)
    {
        return $$"""
		{
			"command": "{{StreamingCommandName.Balance}}",
			"data": {{GetStreamingBalanceRecord(balance, equity)}}
		}
		""";
    }

    public string GetStreamingBalanceRecord(double? balance = null, double? equity = null)
    {
		var balanceValue = balance ?? Math.Round(_faker.Random.Double(AccountOptions.BalanceMin, AccountOptions.BalanceMax), 2);
		var equityValue = equity ?? Math.Round(_faker.Random.Double(AccountOptions.BalanceMin, AccountOptions.BalanceMax), 2);

        return $$"""
		{
			"balance": {{balanceValue}},
			"credit": 1000.00,
			"equity": {{equityValue}},
			"margin": 572634.43,
			"marginFree": 995227635.00,
			"marginLevel": 173930.41
		}
		""";
    }

    public string GetProfitCalculationResponse(double? profit = null)
    {
        var profitValue = profit ?? Math.Round(_faker.Random.Double(AccountOptions.ProfitMin, AccountOptions.ProfitMax), 2);

        return $$"""
		{
			"status": true,
			"returnData": {
				"profit": {{profitValue}}
			}
		}
		""";
    }

    public string GetProfitsResponse(long? order1 = null, long? order2 = null, double? profit = null)
    {
        return $$"""
		{
			"command": "{{StreamingCommandName.Profit}}",
			"data": {{GetStreamingProfitsRecord(order1, order2, profit)}}
		}
		""";
    }

    public string GetStreamingProfitsRecord(long? order1 = null, long? order2 = null, double? profit = null)
    {
		var order1Value = order1 ?? NewOrderId();
		var order2Value = order2 ?? NewOrderId();
		var profitValue = profit ?? Math.Round(_faker.Random.Double(MarketOptions.ProfitMin, MarketOptions.ProfitMax), 2);

        return $$"""
		{
			"order": {{order1Value}},
			"order2": {{order2Value}},
			"position": {{order1Value}},
			"profit": {{profitValue}}
		}
		""";
    }

    public string GetStreamingCandlesResponse(string symbol, double? volume = null, DateTimeOffset? time = null)
    {
        return $$"""
		{
			"command": "{{StreamingCommandName.Candle}}",
			"data": {{GetStreamingCandlesRecord(symbol, volume, time)}}
		}
		""";
    }

    public string GetStreamingCandlesRecord(string symbol, double? volume = null, DateTimeOffset? time = null)
    {
		var timeValue = time ?? TimeProvider();
		var volumeValue = volume ?? Math.Round(_faker.Random.Double(MarketOptions.VolumeMin, MarketOptions.VolumeMax), 3);

        return $$"""
		{
			"close": 4.1849,
			"ctm": {{timeValue.ToUnixTimeMilliseconds()}},
			"ctmString": "{{timeValue.ToString("MMM dd, yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)}}",
			"high": 4.1854,
			"low": 4.1848,
			"open": 4.1848,
			"quoteId": 2,
			"symbol": "{{symbol}}",
			"vol": {{volumeValue}}
		}
		""";
    }

    public string GetStreamingTradesResponse(string symbol, long? order1 = null, long? order2 = null, double? volume = null)
    {
        return $$"""
		{
			"command": "{{StreamingCommandName.Trade}}",
			"data": {{GetStreamingTradesRecord(symbol, order1, order2, volume)}}
		}
		""";
    }

    public string GetStreamingTradesRecord(string symbol, long? order1 = null, long? order2 = null, double? volume = null)
    {
		var order1Value = order1 ?? NewOrderId();
		var order2Value = order2 ?? NewOrderId();
        var volumeValue = volume ?? Math.Round(_faker.Random.Double(MarketOptions.VolumeMin, MarketOptions.VolumeMax), 3);

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
			"order": {{order1Value}},
			"order2": {{order2Value}},
			"position": {{order1Value}},
			"profit": 68.392,
			"sl": 0.0,
			"state": "Modified",
			"storage": -4.46,
			"symbol": "{{symbol}}",
			"tp": 0.0,
			"type": 0,
			"volume": {{volumeValue}}
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

    public string GetStreamingTradeStatusRecord(long? order = null, double? price = null)
    {
		var orderValue = order ?? NewOrderId();
		var priceValue = price ?? Math.Round(_faker.Random.Double(1, 100), 2);

        return $$"""
		{
			"customComment": "Some text",
			"message": null,
			"order": {{orderValue}},
			"price": {{priceValue}},
			"requestStatus": 3
		}
		""";
    }

    public string GetServerTimeResponse(DateTimeOffset? time = null)
    {
		var timeValue = time ?? TimeProvider();

        return $$"""
		{
			"status": true,
			"returnData": {
				"time": {{timeValue.ToUnixTimeMilliseconds()}},
				"timeString": {{timeValue.ToString("MMM dd, yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)}}
			}
		}
		""";
    }

    public string GetLoginResponse(bool ok = true)
    {
        return $$"""
		{
			"status": {{ok.ToString().ToLowerInvariant()}},
			"streamSessionId": {{NewSessionId()}}
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
		var currencyValue = currency ?? _faker.PickRandom(AccountOptions.Currencies);

        return $$"""
		{
			"status": true,
			"returnData": {
				"currency": "{{currencyValue}}",
				"leverage": 1,
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

    public string GetMarginTradeResponse(double? margin = null)
    {
		var marginValue = margin ??_faker.Random.Double(0, 100); //todo

        return $$"""
		{
			"status": true,
			"returnData": {
				"margin": {{marginValue}}
			}
		}
		""";
    }

    public string GetCalendarResponse(string? country = null, DateTimeOffset? time = null)
    {
        return $$"""
		{
			"status": true,
			"returnData": [{{GetCalendarRecord(country, time)}}, {{GetCalendarRecord(country, time)}}]
		}
		""";
    }

    public string GetCalendarRecord(string? country = null, DateTimeOffset? time = null)
    {
		var timeValue = time ?? TimeProvider();
		var countryValue = country ?? _faker.PickRandom(CalendarOptions.Countries);

        return $$"""
		{
			"country": "{{countryValue}}",
			"current": "",
			"forecast": "",
			"impact": "3",
			"period": "(FEB)",
			"previous": "58.3",
			"time": {{timeValue.ToUnixTimeMilliseconds()}},
			"title": ""
		}
		""";
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

    public string GetAllSymbolsResponse(string[] symbols = null)
    {
        return $$"""
		{
		    "status": true,
		    "returnData": [{{GetSymbolRecord(symbols[0])}}, {{GetSymbolRecord("")}}]
		}
		""";
    }

    public string GetSymbolRecord(string? symbol = null, DateTimeOffset? time = null)
    {
		var symbolValue = symbol ?? _faker.Random.String2(4);
        var timeValue = time ?? TimeProvider();

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
			"symbol": "{{symbolValue}}",
			"tickSize": 1.0,
			"tickValue": 1.0,
			"time": {{timeValue.ToUnixTimeMilliseconds()}},
			"timeString": {{timeValue.ToString("MMM dd, yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)}}
			"trailingEnabled": true,
			"type": 21
		}
		""";
    }

    public string GetTradingHoursResponse(string[] symbols, long? since = null, long? until = null)
    {
        var data = string.Join(", ", symbols.Select(s => GetTradingHoursRecord(s, since, until)));

        return $$"""
		{
		    "status": true,
		    "returnData": [{{data}}]
		}
		""";
    }

    public string GetTradingHoursRecord(string symbol, long? since = null, long? until = null)
    {
        return $$"""
		{
		    "quotes": [{{GetQuotesRecord(1, since, until)}}, {{GetQuotesRecord(2, since, until)}}],
			"symbol": "{{symbol}}",
			"trading": [{{GetTradingRecord(1, since, until)}}, {{GetTradingRecord(2, since, until)}}]
		}
		""";
    }

    public string GetQuotesRecord(int day, long? since = null, long? until = null)
    {
		var sinceValue = since ?? _faker.PickRandom(MarketOptions.StartTradingTime);
		var untilValue = until ?? _faker.PickRandom(MarketOptions.EndTradingTime);

        return $$"""
		{
			"day": {{day}},
			"fromT": {{sinceValue}},
			"toT": {{untilValue}}
		}
		""";
    }

    public string GetTradingRecord(int day, long? since = null, long? until = null)
    {
        var sinceValue = since ?? _faker.PickRandom(MarketOptions.StartTradingTime);
        var untilValue = until ?? _faker.PickRandom(MarketOptions.EndTradingTime);

        return $$"""
		{
			"day": {{day}},
			"fromT": {{sinceValue}},
			"toT": {{untilValue}}
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

    public string GetRateInfoRecord(DateTimeOffset? time = null)
    {
		var timeValue = time ?? TimeProvider();

        return $$"""
		{
			"close": 1.0,
			"ctm": {{timeValue.ToUnixTimeMilliseconds()}},
			"ctmString": "{{timeValue.ToString("MMM dd, yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)}}",
			"high": 6.0,
			"low": 0.0,
			"open": 41848.0,
			"vol": 0.0
		}
		""";
    }

    public string GetTickPricesResponse(string[] symbols, double? ask = null, double? bid = null)
    {
        string quotations = string.Join(", ", symbols.Select(s => GetTickRecord(s, ask, bid)));
		//todo time

        return $$"""
		{
			"status": true,
			"returnData": {
				"quotations": [{{quotations}}]
			}
		}
		""";
    }

    public string GetTickRecord(string symbol, double? ask = null, double? bid = null, DateTimeOffset? time = null)
    {
        var timeValue = time ?? TimeProvider();
		var askValue = ask ?? 0; //todo
		var bidValue = bid ?? 1; //todo

        return $$"""
		{
			"ask": {{askValue}},
			"askVolume": 15000,
			"bid": {{bidValue}},
			"bidVolume": 16000,
			"high": 4000.0,
			"level": 0,
			"low": 3500.0,
			"symbol": "{{symbol}}",
			"timestamp": {{timeValue.ToUnixTimeMilliseconds()}}
		}
		""";
    }

    public string GetTradeRecordsResponse(long[] orders)
    {
        var data = string.Join(", ", orders.Select(s => GetTradeRecord(null, NewOrderId(), NewOrderId())));

        return $$"""
		{
			"status": true,
			"returnData": [{{data}}]
		}
		""";
    }

    public string GetTradeRecord(string? symbol = null, long? order1 = null, long? order2 = null, double? volume = null)
    {
        var symbolValue = symbol ?? _faker.Random.String2(4);
        var order1Value = order1 ?? NewOrderId();
        var order2Value = order2 ?? NewOrderId();
		var timeValue = TimeProvider();
        var volumeValue = volume ?? Math.Round(_faker.Random.Double(MarketOptions.VolumeMin, MarketOptions.VolumeMax), 3);

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
			"order": {{order1Value}},
			"order2": {{order2Value}},
			"position": {{order1Value}},
			"profit": -2196.44,
			"sl": 0.0,
			"storage": -4.46,
			"symbol": "{{symbolValue}}",
			"timestamp": {{timeValue.ToUnixTimeMilliseconds()}},
			"tp": 0.0,
			"volume": {{volumeValue}}
		}
		""";
    }

    public string GetTradesResponse(string[] symbols)
    {
        var data = string.Join(", ", symbols.Select(s => GetTradeRecord(s, NewOrderId(), NewOrderId())));

        return $$"""
		{
			"status": true,
			"returnData": [{{data}}]
		}
		""";
    }

    public string GetTradesHistoryResponse(string[] symbols)
    {
        var data = string.Join(", ", symbols.Select(s => GetTradeRecord(s, NewOrderId(), NewOrderId())));

        return $$"""
		{
			"status": true,
			"returnData": [{{data}}]
		}
		""";
    }

    public string GetTradeTransactionResponse(long? order = null)
    {
		var orderValue = order ?? NewOrderId();

        return $$"""
		{
			"status": true,
			"returnData": {
				"order": {{orderValue}}
			}
		}
		""";
    }

    public string GetTradeTransactionStatusResponse(long? order = null)
    {
        var orderValue = order ?? NewOrderId();

        return $$"""
		{
			"status": true,
			"returnData": {
				"ask": 1.392,
				"bid": 1.392,
				"customComment": "Some text",
				"message": null,
				"order": {{orderValue}},
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

    public string GetNewsResponse(DateTimeOffset? since = null, DateTimeOffset? until = null)
    {
        var sinceValue = since ?? TimeProvider();
        var untilValue = until ?? TimeProvider();
		//todo

        return $$"""
		{
			"status": true,
			"returnData": [{{GetNewsTopicRecord(sinceValue)}}, {{GetNewsTopicRecord(untilValue)}}]
		}
		""";
    }

    public string GetNewsTopicRecord(DateTimeOffset? time = null)
    {
        var timeValue = time ?? TimeProvider();

        return $$"""
		{
			"body": "dd",
			"bodylen": 110,
			"key": "1f6da766abd29927aa854823f0105c23",
			"time": {{timeValue.ToUnixTimeMilliseconds()}},
			"timeString": "{{timeValue.ToString("MMM dd, yyyy hh:mm:ss tt", CultureInfo.InvariantCulture)}}",
			"title": "Breaking trend"
		}
		""";
    }
}
