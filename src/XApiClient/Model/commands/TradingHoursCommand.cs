using System.Text.Json.Nodes;

namespace Xtb.XApiClient.Model;

public sealed class TradingHoursCommand : BaseCommand
{
    public const string Name = "getTradingHours";

    public static readonly string[] RequiredArgs = ["symbols"];

    public TradingHoursCommand(JsonObject arguments, bool prettyPrint = false)
        : base(arguments, prettyPrint)
    {
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => RequiredArgs;
}