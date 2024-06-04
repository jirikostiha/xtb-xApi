using System.Text.Json.Nodes;

namespace xAPI.Commands
{

    public class ChartLastCommand : BaseCommand
    {
        public ChartLastCommand(JsonObject arguments, bool prettyPrint) : base(arguments, prettyPrint)
        {
        }

        public override string CommandName
        {
            get
            {
                return "getChartLastRequest";
            }
        }

        public override string[] RequiredArguments
        {
            get
            {
                return new string[] { "info" };
            }
        }
    }
}