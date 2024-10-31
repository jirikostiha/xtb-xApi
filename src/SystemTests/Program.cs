using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Xtb.XApi.SystemTests;

internal static class Program
{
    public const string HostIPAddressString = "81.2.190.163";
    public const int DemoRequestingPort = 5124;
    public const int DemoStreamingPort = 5125;
    public const int RealRequestingPort = 5112;
    public const int RealStreamingPort = 5113;

    public static IPAddress HostIPAddress => IPAddress.Parse(HostIPAddressString);
    public static IPEndPoint DemoRequestingEndpoint => new(HostIPAddress, DemoRequestingPort);
    public static IPEndPoint DemoStreamingEndpoint => new(HostIPAddress, DemoStreamingPort);
    public static IPEndPoint RealRequestingEndpoint => new(HostIPAddress, RealRequestingPort);
    public static IPEndPoint RealStreamingEndpoint => new(HostIPAddress, RealStreamingPort);

    private static string _userId = "16697884";
    private static string _password = "xoh11724";

    private static void Main(string[] args)
    {
        using (var connector = new Connector(DemoRequestingEndpoint))
        {
            RunConnectorTest(connector);
        }

        Console.WriteLine();

        using (var xApiClient = XApiClient.Create(DemoRequestingEndpoint, DemoStreamingEndpoint))
        {
            RunSyncTest(xApiClient);
        }

        Console.WriteLine();

        using (var xApiClient = XApiClient.Create(DemoRequestingEndpoint, DemoStreamingEndpoint))
        {
            RunAsyncTest(xApiClient);
        }

        Console.WriteLine("**** Done ****");
        Console.Read();
    }

    private static void RunConnectorTest(Connector connector)
    {
        Console.WriteLine("----Connector test---");
        var connectorTest = new ConnectorTest(connector, _userId, _password);
        connectorTest.Run();
    }

    private static void RunSyncTest(XApiClient xApiClient)
    {
        Console.WriteLine();
        Console.WriteLine("----Sync test---");
        var syncTest = new SyncTest(xApiClient, _userId, _password, @"\messages\");
        syncTest.Run();
    }

    private static void RunAsyncTest(XApiClient xApiClient)
    {
        Console.WriteLine("----Async test---");
        Console.WriteLine("(esc) abort");
        var asyncTest = new AsyncTest(xApiClient, _userId, _password);
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
            asyncTest.RunAsync(tokenSource.Token).GetAwaiter().GetResult();
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