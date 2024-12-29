using System.Diagnostics;
using Xtb.XApi.Simulation;
using Xtb.XApiClient;

namespace Xtb.XApi.UnitTests;

public class XApiClientFakeTest
{
    private IClient _connector1;
    private IClient _connector2;
    private IXApiClient _xapiclient;

    public XApiClientFakeTest()
    {
        _connector1 = new FakeConnector();
        _connector2 = new FakeConnector();
        _xapiclient = new XClient(new ApiConnector(_connector1, new StreamingApiConnector(_connector2)));
    }
}