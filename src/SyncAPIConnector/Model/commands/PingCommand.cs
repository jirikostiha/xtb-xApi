namespace Xtb.XApi.Model;

public sealed class PingCommand : BaseCommand
{
    public const string Name = "ping";

    public PingCommand()
        : base()
    {
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => [];
}