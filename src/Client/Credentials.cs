namespace Xtb.XApi.Client;

public record Credentials
{
    public Credentials(string login, string password, string? appId = null, string? appName = null)
    {
        Login = login;
        Password = password;
        AppId = appId;
        AppName = appName;
    }

    public string Login { get; }

    public string Password { get; }

    public string? AppId { get; set; }

    public string? AppName { get; set; }
}