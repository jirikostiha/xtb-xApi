namespace Xtb.XApi.Client.Model;

internal abstract class SubscribeCommandBase : ICommand
{
    protected SubscribeCommandBase(string streamSessionId) => StreamSessionId = streamSessionId;

    public abstract string CommandName { get; }

    public string StreamSessionId { get; set; }
}