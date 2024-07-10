namespace xAPI.Sync;

public record ConnectionPolicy
{
    public bool ShallReconnectOnError { get; set; }
    public bool ShallReconnectOnTimeout { get; set; }

    public int[] ReconnectDelays { get; set; }

}