using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using XApi;
using XApi.Responses;

namespace XApiTest;

public abstract class ExampleBase
{
    protected ExampleBase(XApiClient client, string user, string password, string? messageFolder = null)
    {
        Client = client;
        Credentials = new Credentials(user, password);
        MessageFolder = messageFolder;

        if (messageFolder != null)
        {
            Client.MessageReceived += Connector_MessageReceived;
            client.MessageSent += Connector_MessageSent;
        }
    }

    private void Connector_MessageSent(object? sender, MessageEventArgs e)
    {
        if (MessageFolder != null)
        {
            Directory.CreateDirectory(MessageFolder);
            var fileName = $"sent_{TimeProvider.System.GetUtcNow().ToString("yyyy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture)}.json";
            File.WriteAllText(Path.Combine(MessageFolder, fileName), e.Message);
        }
    }

    private void Connector_MessageReceived(object? sender, MessageEventArgs e)
    {
        if (MessageFolder != null)
        {
            Directory.CreateDirectory(MessageFolder);
            var fileName = $"sent_{TimeProvider.System.GetUtcNow().ToString("yyyy-MM-dd_HH-mm-ss", CultureInfo.InvariantCulture)}.json";
            File.WriteAllText(Path.Combine(MessageFolder, fileName), e.Message);
        }
    }

    protected Credentials Credentials { get; set; }

    protected XApiClient Client { get; set; }

    protected string? MessageFolder { get; set; }

    public bool ShallOpenTrades { get; set; }

    protected static void Stage(string name)
    {
        var oc = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine($"Stage: {name}");

        Console.ForegroundColor = oc;
    }

    protected static void Action(string name)
    {
        Task.Delay(200);
        Console.Write($"  {name}...");
    }

    protected static void Pass(BaseResponse? response = null)
    {
        var oc = Console.ForegroundColor;

        if (response is null || response.Status == true)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine("OK");
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Magenta;

            Console.WriteLine($"Error: {response.ErrCode}, {response.ErrorDescr}");
        }

        Console.ForegroundColor = oc;
    }

    protected static void Fail(Exception ex, bool interrupt = false)
    {
        var oc = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.Red;

        Console.WriteLine($"Fail: {ex.Message}");

        Console.ForegroundColor = oc;

        if (interrupt)
            Environment.Exit(1);
    }

    protected static void Detail(string? text)
    {
        var oc = Console.ForegroundColor;
        Console.ForegroundColor = ConsoleColor.DarkGray;

        Console.WriteLine($"    {text}");

        Console.ForegroundColor = oc;
    }
}