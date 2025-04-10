using System.Text.Json.Nodes;

namespace Xtb.XApi.Client.Model;

public sealed class ProfitCalculationCommand : BaseCommand
{
    public const string Name = "getProfitCalculation";

    public static readonly string[] RequiredArgs = ["cmd", "symbol", "volume", "openPrice", "closePrice"];

    public ProfitCalculationCommand(JsonObject arguments, bool prettyPrint = false)
        : base(arguments, prettyPrint)
    {
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => RequiredArgs;
}