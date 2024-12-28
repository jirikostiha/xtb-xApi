using System.Text.Json.Nodes;

namespace Xtb.XApiClient.Model;

public sealed class TradeRecordsCommand : BaseCommand
{
    public const string Name = "getTradeRecords";

    public static readonly string[] RequiredArgs = ["orders"];

    public TradeRecordsCommand(JsonObject arguments, bool prettyPrint = false)
        : base(arguments, prettyPrint)
    {
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => RequiredArgs;
}