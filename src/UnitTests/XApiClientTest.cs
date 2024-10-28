using NSubstitute;
using System.Diagnostics;

namespace Xtb.XApi.UnitTests;

public class XApiClientTest
{
    private IClient _connector1;
    private IClient _connector2;
    private IXApiClient _xapiclient;

    public XApiClientTest()
    {
        _connector1 = Substitute.For<IClient>();
        _connector2 = Substitute.For<IClient>();
        _xapiclient = new XApiClient(new ApiConnector(_connector1, new StreamingApiConnector(_connector2)));
    }

    [Fact]
    public void Create()
    {
        var xclient = XApiClient.Create("81.2.190.163", 5112, 5113);

        Assert.NotNull(xclient.ApiConnector);
        Assert.NotNull(xclient.ApiConnector.Endpoint);
        Assert.Equal("81.2.190.163", xclient.ApiConnector.Endpoint.Address.ToString());
        Assert.Null(xclient.AccountId);
    }

    [Fact]
    public void SendCommandsWithDelay()
    {
        var stopwatch = Stopwatch.StartNew();

        _xapiclient.GetSymbol("US500");
        _xapiclient.GetSymbol("US500");

        stopwatch.Stop();
        Assert.True(stopwatch.Elapsed.TotalMilliseconds > 400);

    }
}