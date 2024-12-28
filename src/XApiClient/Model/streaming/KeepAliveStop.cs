using System.Text.Json.Nodes;

namespace Xtb.XApiClient.Model;

internal sealed class KeepAliveStop : ICommand
{
    public const string Name = "stopKeepAlive";

    public string CommandName => Name;

    public override string ToString()
    {
        JsonObject result = new()
        {
            { "command", CommandName }
        };

        return result.ToJsonString();
    }
}