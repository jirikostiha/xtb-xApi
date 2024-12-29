namespace Xtb.XApi.Extensions.DependencyInjection;

/// <summary>
/// Options for configuring the <see cref="XApiClient" />.
/// </summary>
public record XApiClientServiceOptions
{
    /// <summary>
    /// The address of the X API service.
    /// </summary>
    public string Address { get; set; }

    /// <summary>
    /// The port number for requests.
    /// </summary>
    public int MainPort { get; set; }

    /// <summary>
    /// The port number used for streaming connections.
    /// </summary>
    public int StreamingPort { get; set; }

    /// <summary>
    /// The streaming listener instance that will handle streaming data.
    /// </summary>
    public IStreamingListener? StreamingListener { get; set; }
}
