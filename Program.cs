using RineaR.Spring.Common;

namespace RineaR.Spring;

public class Program
{
    private static void Main(string[] args)
    {
        BuildAsync(args).GetAwaiter().GetResult();
    }

    private static async Task BuildAsync(string[] args)
    {
        SchedulerManager.Initialize();
        await DiscordManager.InitializeAsync();

        DiscordEntry.RegisterEvents();
        SchedulerEntry.RegisterEvents();

        // 永久に待つ
        await Task.Delay(-1);
    }
}