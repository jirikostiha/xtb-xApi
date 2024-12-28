namespace Xtb.XApi.Model;

public sealed class MarginLevelCommand : BaseCommand
{
    public const string Name = "getMarginLevel";

    public MarginLevelCommand(bool? prettyPrint = false)
        : base([], prettyPrint)
    {
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => [];
}