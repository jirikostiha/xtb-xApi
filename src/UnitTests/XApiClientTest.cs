using NSubstitute;
using System.Diagnostics;

namespace Xtb.XApi.UnitTests;

public class XApiClientTest
{
    private IClient _requestingConnector;
    private IClient _streamingConnector;
    private IXApiClient _xclient;

    public XApiClientTest()
    {
        _requestingConnector = Substitute.For<IClient>();
        _streamingConnector = Substitute.For<IClient>();
        _xclient = new XApiClient(new ApiConnector(_requestingConnector, new StreamingApiConnector(_streamingConnector)));
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
        _requestingConnector.SendMessageWaitResponse(Arg.Any<string>()).Returns("{}");

        var stopwatch = Stopwatch.StartNew();

        _xclient.GetSymbol("US500");
        _xclient.GetSymbol("US500");

        stopwatch.Stop();
        Assert.True(stopwatch.Elapsed.TotalMilliseconds > 400);

    }
}