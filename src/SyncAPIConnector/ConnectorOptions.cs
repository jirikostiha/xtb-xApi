using System;

namespace Xtb.XApi;

public record ConnectorOptions
{
    public static TimeSpan DefaultReceiveTimeout => TimeSpan.FromSeconds(5);

    public static TimeSpan DefaultSendTimeout => TimeSpan.FromSeconds(5);

    public static ConnectorOptions Default => new()
    {
        ReceiveTimeout = DefaultReceiveTimeout,
        SendTimeout = DefaultSendTimeout,
    };

    /// <summary>
    /// Maximum receive connection time.
    /// </summary>
    public TimeSpan ReceiveTimeout { get; set; }

    /// <summary>
    /// Maximum send connection time.
    /// </summary>
    public TimeSpan SendTimeout { get; set; }

    /// <summary>
    /// Determine if secure connection shall be used.
    /// </summary>
    public bool ShallUseSecureConnection { get; set; }
}