namespace xAPI.Commands
{
    public class CurrentUserDataCommand : BaseCommand
    {
        public const string Name = "getCurrentUserData";

        public CurrentUserDataCommand(bool prettyPrint = false)
            : base([], prettyPrint)
        {
        }

        public override string CommandName => Name;

        public override string[] RequiredArguments => [];
    }
}