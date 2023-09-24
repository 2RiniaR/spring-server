using System.Text;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore;

namespace RineaR.Spring.Common;

public abstract class PresenterBase
{
    public static string UserNameText(IUser user)
    {
        return (user as SocketGuildUser)?.Nickname ?? user.Username;
    }

    public static string MarvelousScoreText(int diff)
    {
        return $"`✨{diff}`";
    }

    public static string MarvelousScoreDiffText(int diff)
    {
        return $"`✨{diff:+#;-#;0}`";
    }

    public static string PainfulScoreText(int diff)
    {
        return $"`💊{diff}`";
    }

    public static string PainfulScoreDiffText(int diff)
    {
        return $"`💊{diff:+#;-#;0}`";
    }

    public static string TimeText(TimeSpan time)
    {
        return $"{time.Hours:D2}:{time.Minutes:D2}";
    }

    public static string DateTimeText(DateTime dateTime)
    {
        return $"{dateTime:yyyy/MM/dd HH:mm}";
    }

    public static string MarvelousScoreIcon => "✨";
    public static string MarvelousScoreName => "えらいポイント";
    public static string PainfulScoreIcon => "💊";
    public static string PainfulScoreName => "よしよしポイント";

    public static string IntTableText(IEnumerable<(string name, int count)> records)
    {
        var maxDigit = records.Max(r => r.Item2).ToString().Length;
        var sb = new StringBuilder();
        foreach (var (name, count) in records)
        {
            sb.AppendLine($"| {count.ToString().PadLeft(maxDigit)} | {name}");
        }

        return sb.ToString();
    }

    public async Task RunAsync()
    {
        _db = new SpringDbContext();

        try
        {
            await MainAsync();
        }
        catch (Exception e)
        {
            await Console.Error.WriteLineAsync("========================================\n");
            Console.Error.Write(e);
        }

        await _db.DisposeAsync();
    }

    protected abstract Task MainAsync();

    private SpringDbContext _db = null!;

    public DbSet<T> ApplicationData<T>() where T : class
    {
        return _db.Set<T>();
    }
}