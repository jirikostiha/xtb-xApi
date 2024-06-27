namespace xAPI.Commands
{
    public class AllSymbolsCommand : BaseCommand
    {
        public const string Name = "getAllSymbols";

        public AllSymbolsCommand(bool prettyPrint)
            : base(prettyPrint)
        {
        }

        public override string CommandName => Name;

        public override string[] RequiredArguments => [];
    }
}