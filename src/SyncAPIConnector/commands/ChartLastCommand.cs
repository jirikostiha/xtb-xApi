using System.Text.Json.Nodes;

namespace xAPI.Commands
{
    public class ChartLastCommand : BaseCommand
    {
        public const string Name = "getChartLastRequest";

        public static readonly string[] RequiredArgs = ["info"];

        public ChartLastCommand(JsonObject arguments, bool prettyPrint)
            : base(arguments, prettyPrint)
        {
        }

        public override string CommandName => Name;

        public override string[] RequiredArguments => RequiredArgs;
    }
}