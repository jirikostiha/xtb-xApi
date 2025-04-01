using System.Text.Json.Nodes;

namespace Xtb.XApi.Client.Model;

public sealed class TradeTransactionCommand : BaseCommand
{
    public const string Name = "tradeTransaction";

    public static readonly string[] RequiredArgs = ["tradeTransInfo"];

    public TradeTransactionCommand(JsonObject arguments, bool prettyPrint = false)
        : base(arguments, prettyPrint)
    {
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => RequiredArgs;
}