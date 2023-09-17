namespace RineaR.Spring.Common;

public static class TimeManager
{
    /// <summary>
    /// アプリ内の現在時刻を取得する
    /// デバッグ機能により操作されることがある
    /// </summary>
    public static DateTime GetNow()
    {
        return DateTime.Now;
    }

    /// <summary>
    /// アプリ内の日付を取得する
    /// </summary>
    public static DateTime GetApplicationDate()
    {
        return GetNow().AsInterval(TimeSpan.FromDays(1), MasterManager.DailyReset);
    }
}