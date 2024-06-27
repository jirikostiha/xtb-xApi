namespace xAPI.Commands
{
    public class CalendarCommand : BaseCommand
    {
        public const string Name = "getCalendar";

        public CalendarCommand(bool prettyPrint)
            : base([], prettyPrint)
        {
        }

        public override string CommandName => Name;

        public override string[] RequiredArguments => [];
    }
}