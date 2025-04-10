namespace Xtb.XApi.Client.Model;

public sealed class AllSymbolsCommand : BaseCommand
{
    public const string Name = "getAllSymbols";

    public AllSymbolsCommand(bool prettyPrint = false)
        : base(prettyPrint)
    {
    }

    public override string CommandName => Name;

    public override string[] RequiredArguments => [];
}