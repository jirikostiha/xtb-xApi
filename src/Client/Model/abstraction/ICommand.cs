namespace Xtb.XApi.Client.Model;

/// <summary>
/// Represents a command with a name.
/// </summary>
public interface ICommand
{
    /// <summary>
    /// The name of the command.
    /// </summary>
    string CommandName { get; }
}
