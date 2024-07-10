using System.Text.Json.Nodes;

namespace xAPI.Commands
{
    public class TradeTransactionCommand : BaseCommand
    {
        public const string Name = "tradeTransaction";

        public static readonly string[] RequiredArgs = ["tradeTransInfo"];

        public TradeTransactionCommand(JsonObject arguments, bool prettyPrint)
            : base(arguments, prettyPrint)
        {
        }

        public override string CommandName => Name;

        public override string[] RequiredArguments => RequiredArgs;
    }
}