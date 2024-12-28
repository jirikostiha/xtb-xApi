namespace Xtb.XApiClient.Model;

public sealed class CalendarCommand : BaseCommand
{
    public const string Name = "getCalendar";

    public CalendarCommand(bool prettyPrint = false)
        : base([], prettyPrint)
    {
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => [];
}