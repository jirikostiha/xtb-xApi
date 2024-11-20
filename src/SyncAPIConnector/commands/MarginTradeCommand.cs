using System.Text.Json.Nodes;

namespace Xtb.XApi.Commands;

public sealed class MarginTradeCommand : BaseCommand
{
    public const string Name = "getMarginTrade";

    public static readonly string[] RequiredArgs = ["symbol", "volume"];

    public MarginTradeCommand(JsonObject arguments, bool prettyPrint = false)
        : base(arguments, prettyPrint)
    {
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => RequiredArgs;
}