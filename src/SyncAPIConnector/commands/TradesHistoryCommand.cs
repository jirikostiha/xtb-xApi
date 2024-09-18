using System.Text.Json.Nodes;

namespace Xtb.XApi.Commands;

public class TradesHistoryCommand : BaseCommand
{
    public const string Name = "getTradesHistory";

    public static readonly string[] RequiredArgs = ["start", "end"];

    public TradesHistoryCommand(JsonObject arguments, bool prettyPrint)
        : base(arguments, prettyPrint)
    {
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => RequiredArgs;
}