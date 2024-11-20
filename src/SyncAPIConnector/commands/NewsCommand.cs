using System.Text.Json.Nodes;

namespace Xtb.XApi.Commands;

public sealed class NewsCommand : BaseCommand
{
    public const string Name = "getNews";

    public static readonly string[] RequiredArgs = ["start", "end"];

    public NewsCommand(JsonObject body, bool prettyPrint = false)
        : base(body, prettyPrint)
    {
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => RequiredArgs;
}