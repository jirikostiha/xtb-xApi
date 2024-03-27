using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xAPI.Sync;
using xAPI.Responses;
using xAPI.Commands;
using xAPI.Records;
using xAPI.Codes;

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
            SyncAPIConnector connector = new SyncAPIConnector(serverData);
            Console.WriteLine("Connected to the server");

            // Login to server
            Credentials credentials = new Credentials(userId, password, "", "YOUR APP NAME");

            LoginResponse loginResponse = APICommandFactory.ExecuteLoginCommand(connector, credentials, true);
            Console.WriteLine("Logged in as: " + userId);

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

            Console.Read();
        }
    }
}
