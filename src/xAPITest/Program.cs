using System;
using System.Linq;
using xAPI.Sync;
using xAPI.Responses;
using xAPI.Commands;
using xAPI.Records;
using xAPI.Codes;
using System.Threading;

namespace xAPITest
{
    class Program
    {
        private static Server serverData = Servers.DEMO;
        private static string userId = "10000";
        private static string password = "password";

        static void Main(string[] args)
        {
            Console.WriteLine("Server address: " + serverData.Address + " port: " + serverData.MainPort + " streaming port: " + serverData.StreamingPort);

            // Connect to server
            SyncAPIConnector connector = null;
            try
            {
                connector = new SyncAPIConnector(serverData);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Environment.Exit(1);
            }

            Console.WriteLine("Connected to the server");

            // Login to server
            Credentials credentials = new Credentials(userId, password, "", "YOUR APP NAME");

            LoginResponse loginResponse = APICommandFactory.ExecuteLoginCommand(connector, credentials, true);
            Console.WriteLine("Logged in as: " + userId);

            var pingResponse = APICommandFactory.ExecutePingCommand(connector, true);
            Console.WriteLine("Ping status: " + pingResponse.Status);

            // Execute GetServerTime command
            ServerTimeResponse serverTimeResponse = APICommandFactory.ExecuteServerTimeCommand(connector, true);
            Console.WriteLine("Server time: " + serverTimeResponse.TimeString);

            // Execute GetAllSymbols command
            AllSymbolsResponse allSymbolsResponse = APICommandFactory.ExecuteAllSymbolsCommand(connector, true);
            Console.WriteLine("All symbols count: " + allSymbolsResponse.SymbolRecords.Count);

            // Print first 5 symbols
            Console.WriteLine("First five symbols:");
            foreach (SymbolRecord symbolRecord in allSymbolsResponse.SymbolRecords.Take(5))
            {
                Console.WriteLine(" > " + symbolRecord.Symbol + " ask: " + symbolRecord.Ask + " bid: " + symbolRecord.Bid);
            }

            // get US500 info
            Console.WriteLine("Getting US500 symbol.");
            var us500Symbol = APICommandFactory.ExecuteSymbolCommand(connector, "US500");
            Console.WriteLine(us500Symbol.Symbol.Symbol + " ask: " + us500Symbol.Symbol.Ask + " bid: " + us500Symbol.Symbol.Bid);

            var us500TradeTransInfo = new TradeTransInfoRecord(
                TRADE_OPERATION_CODE.BUY,
                TRADE_TRANSACTION_TYPE.ORDER_OPEN,
                us500Symbol.Symbol.Ask,
                null,
                null,
                us500Symbol.Symbol.Symbol,
                0.1,
                null,
                null,
                null);

            // Warning: Opening trade. Make sure you have set up demo account!
            TradeTransactionResponse us500TradeTransaction = APICommandFactory.ExecuteTradeTransactionCommand(connector, us500TradeTransInfo, true);
            Console.WriteLine("Opened trade: " + us500TradeTransaction.Order);

            // get all open trades
            TradesResponse openTrades = APICommandFactory.ExecuteTradesCommand(connector, true, true);
            Console.WriteLine("Open trades: ");
            foreach (var tradeRecord in openTrades.TradeRecords)
            {
                Console.WriteLine(" > " + tradeRecord.Order + " " + tradeRecord.Symbol + " open price: " + tradeRecord.Open_price + " profit: " + tradeRecord.Profit);
            }

            TradeRecord us500trade = openTrades.TradeRecords.First(t => t.Symbol == "US500");

            Thread.Sleep(500);

            // update trade transaction
            us500TradeTransInfo.Tp = us500trade.Open_price + 200;
            TradeTransactionResponse updatedUs500TradeTransaction = APICommandFactory.ExecuteTradeTransactionCommand(connector, us500TradeTransInfo, true);
            Console.WriteLine("Modified trade tp to " + us500TradeTransInfo.Tp + ", order: " + updatedUs500TradeTransaction.Order);

            Thread.Sleep(1000);

            // close trade transaction
            us500TradeTransInfo.Type = TRADE_TRANSACTION_TYPE.ORDER_CLOSE;
            us500TradeTransInfo.Order = us500trade.Order;
            us500TradeTransInfo.Price = us500Symbol.Symbol.Bid;
            TradeTransactionResponse closedUs500TradeTransaction = APICommandFactory.ExecuteTradeTransactionCommand(connector, us500TradeTransInfo, true);
            Console.WriteLine("Closed trade " + closedUs500TradeTransaction.Order);

            Console.Read();
        }
    }
}
