using System;
using System.Threading.Tasks;

namespace Xtb.XApi.Examples;

public class E1 : ExampleBase
{
    public override async Task Run()
    {
        var client = XApiClient.Create(Address, DemoRequestingPort, DemoStreamingPort);
        await client.ConnectAsync();
        var loginResponse = await client.LoginAsync(Credentials);
    }
}