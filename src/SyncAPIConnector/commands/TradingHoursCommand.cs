using System.Text.Json.Nodes;

namespace Xtb.XApi.Commands;

public class TradingHoursCommand : BaseCommand
{
    public const string Name = "getTradingHours";

    public static readonly string[] RequiredArgs = ["symbols"];

    public TradingHoursCommand(JsonObject arguments, bool prettyPrint)
        : base(arguments, prettyPrint)
    {
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => RequiredArgs;
}