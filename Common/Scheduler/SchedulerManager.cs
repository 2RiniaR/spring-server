using System.Timers;
using Microsoft.EntityFrameworkCore;
using Timer = System.Timers.Timer;

namespace RineaR.Spring.Common;

public static class SchedulerManager
{
    private static readonly Timer Timer = new(TimeSpan.FromSeconds(1));
    private static readonly List<SchedulerJobRunner> Runners = new();
    private static readonly Dictionary<string, DateTime> CachedLastTimestamps = new();
    private static readonly Timer CacheFetchTimer = new(TimeSpan.FromMinutes(1));

    public static async Task InitializeAsync()
    {
        Timer.Elapsed += OnEverySecond;
        Timer.Start();
        CacheFetchTimer.Elapsed += (_, _) => FetchLastRunTimeAsync().Forget();
        CacheFetchTimer.Start();

        await FetchLastRunTimeAsync();
    }

    private static void OnEverySecond(object? sender, ElapsedEventArgs e)
    {
        var now = TimeManager.GetNow();
        foreach (var runner in Runners)
        {
            if (GetLastRunTime(runner) + runner.Interval + runner.ConfigureTime <= now)
            {
                runner.Run();
                SetLastRunTime(runner, now);
            }
        }
    }

    public static void RegisterInterval<T>(TimeSpan interval, TimeSpan delta) where T : SchedulerJobPresenterBase, new()
    {
        Runners.Add(new SchedulerJobRunner<T>
        {
            Interval = interval,
            ConfigureTime = delta,
        });
    }

    public static void RegisterDaily<T>(TimeSpan time) where T : SchedulerJobPresenterBase, new()
    {
        RegisterInterval<T>(TimeSpan.FromDays(1), time);
    }

    public static void RegisterWeekly<T>(DayOfWeek dayOfWeek, TimeSpan time) where T : SchedulerJobPresenterBase, new()
    {
        RegisterInterval<T>(TimeSpan.FromDays(7), TimeSpan.FromDays((int)dayOfWeek) + time);
    }

    public static DateTime GetLastRunTime(SchedulerJobRunner runner)
    {
        // 毎秒DBを読むことになってしまうため、キャッシュする
        if (CachedLastTimestamps.TryGetValue(runner.Id, out var lastRunTime)) return lastRunTime;
        return DateTime.MinValue;
    }

    public static void SetLastRunTime(SchedulerJobRunner runner, DateTime runTime)
    {
        CachedLastTimestamps[runner.Id] = runTime;
        SaveLastRunTimeAsync(runner, runTime).Forget();
    }

    public static async Task FetchLastRunTimeAsync()
    {
        await using var context = new SpringDbContext();
        var states = await context.Set<SchedulerJobState>().ToListAsync();
        foreach (var state in states)
        {
            CachedLastTimestamps[state.Id] = state.LastRunTime;
        }
    }

    public static async Task<SchedulerJobState> SaveLastRunTimeAsync(SchedulerJobRunner runner, DateTime runTime)
    {
        await using var context = new SpringDbContext();

        var state = await context.FindAsync<SchedulerJobState>(runner.Id);
        if (state == null)
        {
            state = new SchedulerJobState { Id = runner.Id, LastRunTime = runTime };
            context.Add(state);
        }
        else
            state.LastRunTime = runTime;

        await context.SaveChangesAsync();
        return state;
    }
}