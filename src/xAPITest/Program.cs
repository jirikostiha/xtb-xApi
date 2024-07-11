using System;
using System.Threading;
using System.Threading.Tasks;
using xAPI.Sync;

namespace xAPITest
{
    internal static class Program
    {
        private static Server _server = Servers.DEMO;
        private static string _userId = "16401086";
        private static string _password = "8Ddddddd";

        private static void Main(string[] args)
        {
            using (var apiConnector = new ApiConnector(_server))
            {
                Console.WriteLine("----Sync test---");
                var syncExample = new SyncExample(apiConnector, _userId, _password);
                syncExample.Run();
            }

            using (var apiConnector = new ApiConnector(_server))
            {
                Console.WriteLine();
                Console.WriteLine("----Async test---");
                Console.WriteLine("(esc) abort");
                var asyncExample = new AsyncExample(apiConnector, _userId, _password);
                using var tokenSource = new CancellationTokenSource();

                Task.Run(() =>
                {
                    while (!tokenSource.Token.IsCancellationRequested)
                    {
                        if (Console.KeyAvailable)
                        {
                            var key = Console.ReadKey(intercept: true);
                            if (key.Key == ConsoleKey.Escape)
                            {
                                tokenSource.Cancel();
                                Console.WriteLine("Operation aborted.");
                                break;
                            }
                        }
                        Thread.Sleep(100);
                    }
                });

                try
                {
                    asyncExample.RunAsync(tokenSource.Token).GetAwaiter().GetResult();
                }
                catch (OperationCanceledException)
                {
                    Console.WriteLine("Operation was canceled.");
                }
            }

            Console.WriteLine("Done.");
            Console.Read();
        }
    }
}