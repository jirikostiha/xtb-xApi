using System;
using xAPI.Sync;

namespace xAPITest
{
    sealed class Program
    {
        private static Server serverData = Servers.DEMO;
        private static string userId = "10000";
        private static string password = "password";

        static void Main(string[] args)
        {
            SyncExample.Run(serverData, userId, password);
            //AsyncExample.Run(serverData, userId, password).Wait();

            Console.WriteLine("Done.");
            Console.Read();
        }
    }
}
