using System.Text.Json.Nodes;

namespace xAPI.Commands
{

    public class ChartRangeCommand : BaseCommand
    {
        public ChartRangeCommand(JsonObject arguments, bool prettyPrint) : base(arguments, prettyPrint)
        {
        }

        public override string CommandName
        {
            get
            {
                return "getChartRangeRequest";
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