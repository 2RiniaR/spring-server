namespace RineaR.Spring.Common;

public static class TimeManager
{
    /// <summary>
    /// アプリ内の現在時刻を取得する
    /// デバッグ機能により操作されることがある
    /// </summary>
    public static DateTime GetNow()
    {
        return DateTime.Today + TimeSpan.FromHours(8);
        // return DateTime.Now;
    }

    /// <summary>
    /// アプリ内の日付を取得する
    /// </summary>
    public static DateTime GetCurrentApplicationDate()
    {
        return GetNow().AsInterval(TimeSpan.FromDays(1), MasterManager.DailyReset);
    }

    public static DateTime GetCurrentApplicationDateStart()
    {
        return GetNow().AsInterval(TimeSpan.FromDays(1), MasterManager.DailyReset, true);
    }

    public static DateTime GetCurrentApplicationDateEnd()
    {
        return GetCurrentApplicationDateStart() + TimeSpan.FromDays(1);
    }

    /// <summary>
    /// アプリ内の週を取得する
    /// </summary>
    public static DateTime GetCurrentApplicationWeek()
    {
        return GetNow().AsInterval(TimeSpan.FromDays(7), MasterManager.WeeklyReset);
    }

    public static DateTime GetCurrentApplicationWeekStart()
    {
        return GetNow().AsInterval(TimeSpan.FromDays(7), MasterManager.WeeklyReset, true);
    }

    public static DateTime GetCurrentApplicationWeekEnd()
    {
        return GetCurrentApplicationWeekStart() + TimeSpan.FromDays(7);
    }
}