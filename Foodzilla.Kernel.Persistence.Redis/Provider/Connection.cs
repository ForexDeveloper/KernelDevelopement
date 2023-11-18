using Foodzilla.Kernel.Commons.Credentials;

namespace Foodzilla.Kernel.Persistence.Redis.Provider;

internal static class Connection
{
    internal static string GetConfiguration(RedisCredential credential)
    {
        if (string.IsNullOrEmpty(credential.ServerPort))
        {
            return $"{credential.ServerAddress}:{credential.ServerPort},password={credential.Password}";
        }
        else
        {
            return $"{credential.ServerAddress}:6379,password={credential.Password}";

        }
    }
}