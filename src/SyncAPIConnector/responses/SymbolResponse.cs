using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System;
using xAPI.Codes;
using xAPI.Records;

namespace xAPI.Responses
{
    public class SymbolResponse : BaseResponse
    {
        public SymbolResponse()
            : base()
        { }

        public SymbolResponse(string body)
            : base(body)
        {
            if (ReturnData is null)
                return;

            var ob = ReturnData.AsObject();
            Symbol = new SymbolRecord()
            {
                //   Ask = (double?)value["ask"];
                //Bid = (double?)value["bid"];
                //CategoryName = (string)value["categoryName"];
                //Currency = (string)value["currency"];
                //CurrencyPair = (bool?)value["currencyPair"];
                //CurrencyProfit = (string)value["currencyProfit"];
                //Description = (string)value["description"];
                //Expiration = (long?)value["expiration"];
                //GroupName = (string)value["groupName"];
                //High = (double?)value["high"];
                //InstantMaxVolume = (long?)value["instantMaxVolume"];
                //Leverage = (double)value["leverage"];
                //LongOnly = (bool?)value["longOnly"];
                //LotMax = (double?)value["lotMax"];
                //LotMin = (double?)value["lotMin"];
                //LotStep = (double?)value["lotStep"];
                //Low = (double?)value["low"];
                //Precision = (long?)value["precision"];
                //Starting = (long?)value["starting"];
                //StopsLevel = (long?)value["stopsLevel"];
                //Symbol = (string)value["symbol"];
                //Time = (long?)value["time"];
                //TimeString = (string)value["timeString"];
                //Type = (long?)value["type"];
                //ContractSize = (long?)value["contractSize"];
                //InitialMargin = (long?)value["initialMargin"];
                //MarginHedged = (long?)value["marginHedged"];
                //MarginHedgedStrong = (bool?)value["marginHedgedStrong"];
                //MarginMaintenance = (long?)value["marginMaintenance"];
                //MarginMode = new MARGIN_MODE((long)value["marginMode"]);
                //Percentage = (double?)value["percentage"];
                //ProfitMode = new PROFIT_MODE((long)value["profitMode"]);
                //QuoteId = (long?)value["quoteId"];
                //SpreadRaw = (double?)value["spreadRaw"];
                //SpreadTable = (double?)value["spreadTable"];
                //StepRuleId = (long?)value["stepRuleId"];
                //SwapEnable = (bool?)value["swapEnable"];
                //SwapLong = (double?)value["swapLong"];
                //SwapShort = (double?)value["swapShort"];
                //SwapType = new SWAP_TYPE((long)value["swapType"]);
                //SwapRollover = new SWAP_ROLLOVER_TYPE((long)value["swap_rollover3days"]);
                //TickSize = (double?)value["tickSize"];
                //TickValue = (double?)value["tickValue"];
            };
            //Symbol.FieldsFromJsonObject(ob);
        }

        public SymbolRecord? Symbol { get; init; }
    }
}