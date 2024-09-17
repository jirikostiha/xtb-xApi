using System.Text.Json.Nodes;

namespace XApi.Commands;

public class MarginTradeCommand : BaseCommand
{
    public const string Name = "getMarginTrade";

    public static readonly string[] RequiredArgs = ["symbol", "volume"];

    public MarginTradeCommand(JsonObject arguments, bool prettyPrint)
        : base(arguments, prettyPrint)
    {
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => RequiredArgs;
}