using System;
using System.Threading.Tasks;
using Xtb.XApi.Examples;

namespace Xtb.XApi.SystemTests;

internal static class Program
{
    private static async Task Main(string[] args)
    {
        //await new E1().Run();
        await new E2().Run();

        Console.WriteLine("Done.");
        Console.Read();
    }
}