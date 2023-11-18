namespace Foodzilla.Kernel.Commons.Credentials;

public sealed class SqlCredential
{
    public string DbName { get; set; }

    public string UserName { get; set; }

    public string Password { get; set; }

    public string? ServerPort { get; set; }

    public string ServerAddress { get; set; }

    public SqlCredential()
    {

    }

    private SqlCredential(string dbName, string userName, string password, string serverAddress, string? serverPort = null)
    {
        DbName = dbName;
        UserName = userName;
        Password = password;
        ServerPort = serverPort;
        ServerAddress = serverAddress;
    }

    public static SqlCredential Create(string dbName, string userName, string password, string serverAddress, string? serverPort = null)
    {
        return new SqlCredential(dbName, userName, password, serverAddress, serverPort);
    }
}