﻿using System;
using xAPI.Sync;

namespace xAPITest
{
    sealed class Program
    {
        private static Server _server = Servers.DEMO;
        private static string _userId = "10000";
        private static string _password = "password";

        static void Main(string[] args)
        {
            using (var syncConnector = new SyncAPIConnector(_server))
            {
                Console.WriteLine("----Sync test---");
                var syncExample = new SyncExample(syncConnector, _userId, _password);
                syncExample.Run();
            }

            using (var syncConnector = new SyncAPIConnector(_server))
            {
                Console.WriteLine();
                Console.WriteLine("----Async test---");
                var asyncExample = new AsyncExample(syncConnector, _userId, _password);
                asyncExample.Run().GetAwaiter().GetResult();
            }

            Console.WriteLine("Done.");
            Console.Read();
        }
    }
}
