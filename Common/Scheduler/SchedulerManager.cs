using System.Timers;
using Timer = System.Timers.Timer;

namespace RineaR.Spring.Common;

public static class SchedulerManager
{
    private static readonly Timer Timer = new(TimeSpan.FromSeconds(1));
    private static readonly List<JobInfo> JobInfos = new();

    public static void Initialize()
    {
        Timer.Elapsed += OnEverySecond;
        Timer.Start();
    }

    private static void OnEverySecond(object? sender, ElapsedEventArgs e)
    {
        var now = TimeManager.GetNow();
        foreach (var jobInfo in JobInfos)
        {
            var nextTime = jobInfo.GetNext(now);
            if (now < nextTime) continue;
            jobInfo.CreateNewJob().RunAsync();
        }
    }

    private static void RegisterInternal<T>(Func<DateTime, DateTime> getNext)
        where T : SchedulerJobPresenterBase, new()
    {
        JobInfos.Add(new JobInfo(getNext, () => new T()));
    }

    public static void RegisterDaily<T>(Func<TimeSpan> getTime) where T : SchedulerJobPresenterBase, new()
    {
        RegisterInternal<T>(now => now.NextTime(getTime()));
    }

    public static void RegisterWeekly<T>(Func<(DayOfWeek, TimeSpan)> getTime) where T : SchedulerJobPresenterBase, new()
    {
        RegisterInternal<T>(now =>
        {
            var (dayOfWeek, time) = getTime();
            return now.NextDayOfWeek(dayOfWeek).NextTime(time);
        });
    }

    private class JobInfo
    {
        public JobInfo(Func<DateTime, DateTime> getNext, Func<SchedulerJobPresenterBase> createNewJob)
        {
            GetNext = getNext;
            CreateNewJob = createNewJob;
        }

        public Func<DateTime, DateTime> GetNext { get; }
        public Func<SchedulerJobPresenterBase> CreateNewJob { get; }
    }
}