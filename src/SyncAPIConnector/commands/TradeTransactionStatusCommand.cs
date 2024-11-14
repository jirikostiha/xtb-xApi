using System.Text.Json.Nodes;

namespace Xtb.XApi.Commands;

public sealed class TradeTransactionStatusCommand : BaseCommand
{
    public const string Name = "tradeTransactionStatus";

    public static readonly string[] RequiredArgs = ["orders"];

    public TradeTransactionStatusCommand(JsonObject arguments, bool prettyPrint)
        : base(arguments, prettyPrint)
    {
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => RequiredArgs;
}