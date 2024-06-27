using System.Text.Json.Nodes;

namespace xAPI.Commands
{
    public class VersionCommand : BaseCommand
    {
        public const string Name = "getVersion";

        public VersionCommand(JsonObject arguments, bool prettyPrint)
            : base(arguments, prettyPrint)
        {
        }

        public override string CommandName => Name;

        public override string[] RequiredArguments => [];
    }
}