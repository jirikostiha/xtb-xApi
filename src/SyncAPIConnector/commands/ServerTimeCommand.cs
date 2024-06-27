using System.Text.Json.Nodes;

namespace xAPI.Commands
{
    public class ServerTimeCommand : BaseCommand
    {
        public const string Name = "getServerTime";

        public ServerTimeCommand(bool? prettyPrint)
            : base([], prettyPrint)
        {
        }

        public override string CommandName => Name;

        public override string[] RequiredArguments => [];
    }
}