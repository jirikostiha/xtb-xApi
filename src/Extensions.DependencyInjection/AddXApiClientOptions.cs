using Microsoft.Extensions.Options;

namespace Xtb.XApi.Extensions.DependencyInjection;

public record AddXApiClientOptions //: IOptions<AddXApiClientOptions> NO
{
    public string Address { get; set; }

    public int MainPort { get; set; }

    public int StreamingPort { get; set; }

    public IStreamingListener StreamingListener { get; set; }

    /// <inheritdoc/>
    //public AddXApiClientOptions Value => this;
}