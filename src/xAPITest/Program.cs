using System;
using System.Threading;
using System.Threading.Tasks;
using xAPI;
using xAPI.Sync;

namespace xAPITest;

internal static class Program
{
    private static Server _server = Servers.DEMO;
    private static string _userId = "16401086";
    private static string _password = "8Ddddddd";

    private static void Main(string[] args)
    {
        RunSyncExample();
        RunAsyncExample();

        Console.WriteLine("Done.");
        Console.Read();
    }

    private static void RunSyncExample()
    {
        using (var client = new XApiClient())
        {
            Console.WriteLine("----Sync test---");
            var syncExample = new SyncExample(client, _server, _userId, _password, @"\messages\");
            syncExample.Run();
        }
    }

    private static void RunAsyncExample()
    {
        using (var apiConnector = new XApiClient())
        {
            Console.WriteLine();
            Console.WriteLine("----Async test---");
            Console.WriteLine("(esc) abort");
            var asyncExample = new AsyncExample(apiConnector, _server, _userId, _password);
            using var tokenSource = new CancellationTokenSource();

            var keyWaitTask = Task.Run(() =>
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
            finally
            {
                tokenSource.Cancel();
                keyWaitTask.Wait();
            }
        }
    }
}