using System;
using System.Net;
using System.Threading.Tasks;

namespace Xtb.XApi.Examples;

public abstract class ExampleBase
{
    public const int DemoRequestingPort = 5124;
    public const int DemoStreamingPort = 5125;
    public const string Address = "81.2.190.163";
    //public static IPAddress Address => IPAddress.Parse("81.2.190.163");
    public const string UserId = "16697884";
    public const string Password = "xoh11724";
    public Credentials Credentials => new(UserId, Password);



    public abstract Task Run();
}