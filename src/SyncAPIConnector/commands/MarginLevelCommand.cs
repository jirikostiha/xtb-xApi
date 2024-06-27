namespace xAPI.Commands
{
    public class MarginLevelCommand : BaseCommand
    {
        public const string Name = "getMarginLevel";

        public MarginLevelCommand(bool? prettyPrint)
            : base([], prettyPrint)
        {
        }

        public override string CommandName => Name;

        public override string[] RequiredArguments => [];
    }
}