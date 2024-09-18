using System.Text.Json.Nodes;

namespace Xtb.XApi.Commands;

public class TickPricesCommand : BaseCommand
{
    public const string Name = "getTickPrices";

    public static readonly string[] RequiredArgs = ["level", "symbols", "timestamp"];

    public TickPricesCommand(JsonObject arguments, bool prettyPrint)
        : base(arguments, prettyPrint)
    {
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => RequiredArgs;
}