using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using Xtb.XApi.Extensions.DependencyInjection;

namespace Xtb.XApi.Examples;

public class E2 : ExampleBase
{
    public override async Task Run()
    {
        var hostBuilder = Host.CreateDefaultBuilder()
           .ConfigureServices((context, services) =>
           {
               services.AddXApiClient(options =>
               {
                   options.Address = Address;
                   options.MainPort = DemoRequestingPort;
                   options.StreamingPort = DemoStreamingPort;
               });
           });

        var host = hostBuilder.Build();

        var client = host.Services.GetRequiredService<XApiClient>();

        await client.ConnectAsync();
        var loginResponse = await client.LoginAsync(Credentials);
    }
}