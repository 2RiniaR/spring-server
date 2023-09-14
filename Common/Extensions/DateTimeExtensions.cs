namespace RineaR.Spring.Common;

public static class DateTimeExtensions
{
    public static bool IsInRange(this DateTime value, DateTime start, DateTime end)
    {
        return start <= value && value < end;
    }

    public static DateTime NextDayOfWeek(this DateTime source, DayOfWeek dayOfWeek)
    {
        source = source.Date;
        var thisWeekTarget = source.AddDays(DayOfWeek.Sunday - source.DayOfWeek + (int)dayOfWeek);
        return source < thisWeekTarget ? thisWeekTarget : thisWeekTarget.AddDays(7);
    }

    public static DateTime NextTime(this DateTime source, TimeSpan target)
    {
        var todayTarget = source.OnTime(target);
        return source < todayTarget ? todayTarget : todayTarget.AddDays(1);
    }

    public static DateTime PreviousTime(this DateTime source, TimeSpan target)
    {
        var todayTarget = source.OnTime(target);
        return todayTarget < source ? todayTarget : todayTarget.AddDays(-1);
    }

    public static DateTime OnTime(this DateTime source, TimeSpan target)
    {
        return source.Date + new TimeSpan(target.Hours, target.Minutes, target.Seconds);
    }
}