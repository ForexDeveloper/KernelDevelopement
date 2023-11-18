namespace Foodzilla.Kernel.Commons.Credentials;

public sealed class RedisCredential
{
    public int DbIndex { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public string? ServerPort { get; set; }

    public string ServerAddress { get; set; }

    public RedisCredential()
    {

    }

    private RedisCredential(string userName, string password, string serverAddress, string? serverPort = null, int? dbIndex = null)
    {
        DbIndex = dbIndex ?? 0;
        UserName = userName;
        Password = password;
        ServerPort = serverPort;
        ServerAddress = serverAddress;
    }

    public static RedisCredential Create(string userName, string password, string serverAddress, string? serverPort = null, int? dbIndex = null)
    {
        return new RedisCredential(userName, password, serverAddress, serverPort, dbIndex);
    }
}