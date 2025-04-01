namespace Xtb.XApi.Client.Model;

public sealed class VersionCommand : BaseCommand
{
    public const string Name = "getVersion";

    public VersionCommand()
        : base()
    {
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => [];
}