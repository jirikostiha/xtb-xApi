using System.Text.Json.Nodes;

namespace xAPI.Commands
{

    public class TradeTransactionCommand : BaseCommand
    {
        public TradeTransactionCommand(JsonObject arguments, bool prettyPrint)
            : base(arguments, prettyPrint)
        {
        }

        public override string CommandName
        {
            get
            {
                return "tradeTransaction";
            }
        }

        public override string[] RequiredArguments
        {
            get
            {
                return new string[] { "tradeTransInfo" };
            }
        }
    }

}