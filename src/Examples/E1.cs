using System.Threading.Tasks;

namespace Xtb.XApi.Examples;

public class E1 : ExampleBase
{
    public override async Task Run()
    {
        var client = XApiClient.Create("81.2.190.163", 5124, 5125);
        await client.ConnectAsync();
        var loginResponse = await client.LoginAsync("16697884", "xoh11724");
    }
}