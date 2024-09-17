using System.Text.Json.Nodes;
using xAPI.Commands;

namespace XApi.Streaming;

internal sealed class ProfitsStop : ICommand
{
    public const string Name = "stopProfits";

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