using System.Text.Json.Nodes;

namespace xAPI.Commands
{

    public class TradingHoursCommand : BaseCommand
    {
        public TradingHoursCommand(JsonObject arguments, bool prettyPrint) : base(arguments, prettyPrint)
        {
        }

        public override string CommandName
        {
            get
            {
                return "getTradingHours";
            }
        }

        public override string[] RequiredArguments
        {
            get
            {
                return new string[] { "symbols" };
            }
        }
    }
}