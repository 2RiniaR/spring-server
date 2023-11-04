namespace RineaR.Spring.Common;

public static class TimeManager
{
    /// <summary>
    /// アプリ内の現在時刻を取得する
    /// デバッグ機能により操作されることがある
    /// </summary>
    public static DateTime GetNow()
    {
        return DateTime.Now.ToLocalTime();
    }

    /// <summary>
    /// アプリ内の日付を取得する
    /// </summary>
    public static DateTime GetCurrentApplicationDate()
    {
        return GetNow().AsDate(MasterManager.DailyReset);
    }

    public static DateTime GetCurrentApplicationDateStart()
    {
        return GetCurrentApplicationDate() + MasterManager.DailyReset;
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
        return GetNow().AsWeek(MasterManager.WeeklyReset);
    }

    public static DateTime GetCurrentApplicationWeekStart()
    {
        return GetCurrentApplicationWeek() + MasterManager.WeeklyReset;
    }

    public static DateTime GetCurrentApplicationWeekEnd()
    {
        return GetCurrentApplicationWeekStart() + TimeSpan.FromDays(7);
    }
}