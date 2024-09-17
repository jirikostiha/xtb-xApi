namespace xAPI;

public record Credentials
{
    public Credentials(string login, string password)
    {
        Login = login;
        Password = password;
    }

    public Credentials(string login, string password, string appId, string appName)
        : this(login, password)
    {
        AppId = appId;
        AppName = appName;
    }

    public string Login { get; }

    public string Password { get; }

    public string? AppId { get; set; }

    public string? AppName { get; set; }
}