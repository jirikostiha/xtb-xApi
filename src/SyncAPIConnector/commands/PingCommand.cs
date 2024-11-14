namespace Xtb.XApi.Commands;

public sealed class PingCommand : BaseCommand
{
    public const string Name = "ping";

    public PingCommand()
        : base([], false)
    {
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => [];
}