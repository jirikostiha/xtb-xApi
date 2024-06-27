using System.Text.Json.Nodes;

namespace xAPI.Commands
{
    public class ChartRangeCommand : BaseCommand
    {
        public const string Name = "getChartRangeRequest";

        public static readonly string[] RequiredArgs = ["info"];

        public ChartRangeCommand(JsonObject arguments, bool prettyPrint)
            : base(arguments, prettyPrint)
        {
        }

        public override string CommandName => Name;

        public override string[] RequiredArguments => RequiredArgs;
    }
}