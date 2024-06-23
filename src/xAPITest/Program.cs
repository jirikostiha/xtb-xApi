using System;
using xAPI.Sync;

namespace xAPITest
{
    sealed class Program
    {
        private static Server server = Servers.DEMO;
        private static string userId = "10000";
        private static string password = "password";

        static void Main(string[] args)
        {
            using (var syncExample = new SyncExample(server, userId, password))
            {
                Console.WriteLine("----Sync test---");
                syncExample.Run();
            }

            using (var asyncExample = new AsyncExample(server, userId, password))
            {
                Console.WriteLine();
                Console.WriteLine("----Async test---");
                asyncExample.Run().GetAwaiter().GetResult();
            }

            Console.WriteLine("Done.");
            Console.Read();
        }
    }
}
