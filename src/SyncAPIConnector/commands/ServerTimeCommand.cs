namespace Xtb.XApi.Commands;

public sealed class ServerTimeCommand : BaseCommand
{
    public const string Name = "getServerTime";

    public ServerTimeCommand()
        : base([], false)
    {
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => [];
}