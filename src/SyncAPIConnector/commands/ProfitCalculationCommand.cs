using System.Text.Json.Nodes;

namespace xAPI.Commands
{

    public class ProfitCalculationCommand : BaseCommand
    {

        public ProfitCalculationCommand(JsonObject arguments, bool prettyPrint) : base(arguments, prettyPrint)
        {
        }

        public override string CommandName
        {
            get
            {
                return "getProfitCalculation";
            }
        }

        public override string[] RequiredArguments
        {
            get
            {
                return new string[] { "cmd", "symbol", "volume", "openPrice", "closePrice" };
            }
        }
    }
}