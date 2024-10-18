using System.Globalization;
using System.IO;
using System;

namespace Xtb.XApi.SystemTests;

public abstract class XApiClientTestBase : TestBase
{
    protected XApiClientTestBase(XApiClient client, string user, string password, string? messageFolder = null)
        : base(user, password)
    {
        Client = client;
        MessageFolder = messageFolder;

        if (messageFolder != null)
        {
            Client.MessageReceived += Connector_MessageReceived;
            Client.MessageSent += Connector_MessageSent;
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

    protected string? MessageFolder { get; set; }

    protected XApiClient Client { get; set; }

    public bool ShallOpenTrades { get; set; }
}