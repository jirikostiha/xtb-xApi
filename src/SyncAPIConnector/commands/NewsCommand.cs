using System.Text.Json.Nodes;

namespace XApi.Commands;

public class NewsCommand : BaseCommand
{
    public const string Name = "getNews";

    public static readonly string[] RequiredArgs = ["start", "end"];

    public NewsCommand(JsonObject body, bool prettyPrint)
        : base(body, prettyPrint)
    {
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => RequiredArgs;
}