using Microsoft.Extensions.Configuration;

namespace RineaR.Spring.Common;

public class EnvironmentManager
{
    private static string Get(string name)
    {
        return Configuration[name] ??
               throw new Exception($"Environment variable {name} is not set.");
    }

    public static void Reload()
    {
        Configuration.Reload();
    }

    private static IConfigurationRoot Configuration { get; } = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", true)
        .AddEnvironmentVariables()
        .AddUserSecrets<EnvironmentManager>(true)
        .Build();

    public static string DiscordToken => Get("DISCORD_SECRET");
    public static string MysqlConnectionString => Get("MYSQL_CONNECTION_STRING");
}