using System;
using System.Globalization;
using System.IO;

namespace Xtb.XApi.SystemTests;

public abstract class XApiClientTestBase : TestBase
{
    private string? _messageFolder;

    protected XApiClientTestBase(XApiClient client, string user, string password)
        : base(user, password)
    {
        Client = client;
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

    public string? MessageFolder
    {
        get => _messageFolder;
        set
        {
            _messageFolder = value;
            if (value == null)
            {
                Client.ApiConnector.Connector.MessageReceived -= Connector_MessageReceived;
                Client.ApiConnector.Connector.MessageSent -= Connector_MessageSent;
            }
            else
            {
                Client.ApiConnector.Connector.MessageReceived += Connector_MessageReceived;
                Client.ApiConnector.Connector.MessageSent += Connector_MessageSent;
            }
        }
    }

    protected XApiClient Client { get; set; }

    public bool ShallOpenTrades { get; set; }
}