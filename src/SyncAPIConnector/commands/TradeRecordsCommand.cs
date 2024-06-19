using System.Text.Json.Nodes;

namespace xAPI.Commands
{

    public class TradeRecordsCommand : BaseCommand
    {
        public TradeRecordsCommand(JsonObject arguments, bool prettyPrint)
            : base(arguments, prettyPrint)
        {
        }

        public override string CommandName
        {
            get
            {
                return "getTradeRecords";
            }
        }

        public override string[] RequiredArguments
        {
            get
            {
                return new string[] { "orders" };
            }
        }
    }

}