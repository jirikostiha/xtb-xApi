using System;
using System.Net;
using xAPI.Sync;

namespace xAPITest
{
    static class Program
    {
        private static Server _server = Servers.DEMO;
        private static string _userId = "10000";
        private static string _password = "password";

        static void Main(string[] args)
        {
            using (var tcpConn = new TcpConnector(new IPEndPoint(1,1)))
            {
                Console.WriteLine("----Sync test---");
                var syncExample = new SyncExample(new ApiConnector(tcpConn, null), userId, password));
                syncExample.Run();
            }

            using (var tcpConn = new TcpConnector(new IPEndPoint(1, 1)))
            {
                Console.WriteLine();
                Console.WriteLine("----Async test---");
                var asyncExample = new AsyncExample(new ApiConnector(tcpConn, null), userId, password));
                asyncExample.Run().GetAwaiter().GetResult();
            }

            Console.WriteLine("Done.");
            Console.Read();
        }
    }
}
