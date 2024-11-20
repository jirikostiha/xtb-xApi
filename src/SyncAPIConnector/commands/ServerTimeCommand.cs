namespace Xtb.XApi.Commands;

public sealed class ServerTimeCommand : BaseCommand
{
    public const string Name = "getServerTime";

    public ServerTimeCommand()
        : base()
    {
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => [];
}