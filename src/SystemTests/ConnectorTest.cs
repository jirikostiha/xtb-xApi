using System;

namespace Xtb.XApi.Client.SystemTests;

public sealed class ConnectorTest : TestBase
{
    public ConnectorTest(Connector connector, string user, string password)
        : base(user, password)
    {
        Client = connector;
    }

    public Connector Client { get; set; }

    public void Run()
    {
        ConnectionStage();
    }

    public void ConnectionStage()
    {
        Stage("Connection");

        Action($"Establishing connection");
        try
        {
            Client.Connect();
            Pass();
            Detail($"endpoint:{Client.Endpoint}");
        }
        catch (Exception ex)
        {
            Fail(ex, true);
        }

        Action("Ping");
        try
        {
            var response = Client.SendMessageWaitResponse(_pingRequest);
            Pass(response);
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Dropping connection");
        try
        {
            Client.Disconnect();
            Pass();
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action($"Reestablishing connection");
        try
        {
            Client.Connect();
            Pass();
            Detail($"endpoint:{Client.Endpoint}");
        }
        catch (Exception ex)
        {
            Fail(ex, true);
        }

        Action("Ping");
        try
        {
            var response = Client.SendMessageWaitResponse(_pingRequest);
            Pass(response);
        }
        catch (Exception ex)
        {
            Fail(ex);
        }

        Action("Getting version");
        try
        {
            var response = Client.SendMessageWaitResponse(_versionRequest);
            Pass(response);
        }
        catch (Exception ex)
        {
            Fail(ex);
        }
    }

    private const string _pingRequest =
        $$"""
        {
            "command": "ping",
            "arguments": {},
            "customTag": "1"
        }
        """;

    private const string _versionRequest =
        $$"""
        {
            "command": "getVersion",
            "arguments": {},
            "customTag": "2"
        }
        """;
}