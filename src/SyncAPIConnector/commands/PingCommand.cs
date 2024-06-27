namespace xAPI.Commands
{
    public class PingCommand : BaseCommand
    {
        public const string Name = "ping";

        public PingCommand(bool? prettyPrint)
            : base([], prettyPrint)
        {
        }

        public override string CommandName => Name;

        public override string[] RequiredArguments => [];
    }
}