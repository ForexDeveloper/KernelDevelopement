using Microsoft.Data.SqlClient;
using SqlCredential = Foodzilla.Kernel.Commons.Credentials.SqlCredential;

namespace Foodzilla.Kernel.Persistence.EF.Provider;

public static class Connection
{
    public static string GetConfiguration(SqlCredential credential)
    {
        return Configuration(credential);
    }

    internal static SqlConnection GetSqlConfiguration(SqlCredential credential)
    {
        return SqlConfiguration(credential);
    }

    private static string Configuration(SqlCredential credential)
    {
        if (string.IsNullOrEmpty(credential.ServerPort))
        {
            return
                $"Server={credential.ServerAddress};Database={credential.DbName};Integrated Security=False;Persist Security Info=False;User ID={credential.UserName};Password={credential.Password};Encrypt=False;";
        }
        else
        {
            return
                $"Server={credential.ServerAddress},{credential.ServerPort};Database={credential.DbName};Integrated Security=False;Persist Security Info=False;User ID={credential.UserName};Password={credential.Password};Encrypt=False;";
        }
    }

    private static SqlConnection SqlConfiguration(SqlCredential credential)
    {
        if (string.IsNullOrEmpty(credential.ServerPort))
        {
            return
                new SqlConnection($"Server={credential.ServerAddress};Database={credential.DbName};Integrated Security=False;Persist Security Info=False;User ID={credential.UserName};Password={credential.Password};Encrypt=False;");
        }
        else
        {
            return
                new SqlConnection($"Server={credential.ServerAddress},{credential.ServerPort};Database={credential.DbName};Integrated Security=False;Persist Security Info=False;User ID={credential.UserName};Password={credential.Password};Encrypt=False;");
        }
    }

    private static bool ValidateCredential(SqlCredential credential)
    {
        return true;
    }
}