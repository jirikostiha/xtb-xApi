namespace xAPI.Commands;

public class VersionCommand : BaseCommand
{
    public const string Name = "getVersion";

    public VersionCommand()
        : base()
    {
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => [];
}