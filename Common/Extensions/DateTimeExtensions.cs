namespace RineaR.Spring.Common;

public static class DateTimeExtensions
{
    /// <summary>
    /// 特定の周期において、どの日に含まれるかを取得する
    /// 基準点は 西暦0001年1月1日(月) 0:00:00
    /// </summary>
    /// <param name="errorOfZero">DateTime(0)が、基準にしたい時点とどのくらいズレているか</param>
    public static DateTime AsInterval(this DateTime value, TimeSpan offset, TimeSpan interval, TimeSpan errorOfZero)
    {
        var x = value - offset + errorOfZero;
        return new DateTime(x.Ticks / interval.Ticks * interval.Ticks) - errorOfZero;
    }

    /// <summary>
    /// 属する週を返す
    /// 例） 毎週月曜5時にリセット（offset=1d5h）として、valueがどの週の分になるのかを計算し、その週の日曜日を返す
    /// </summary>
    public static DateTime AsWeek(this DateTime value, TimeSpan offset)
    {
        return value.AsInterval(offset, TimeSpan.FromDays(7), TimeSpan.FromDays((int)new DateTime(0).DayOfWeek));
    }

    /// <summary>
    /// 属する日を返す
    /// 例） 毎日5時にリセット（offset=5h）として、valueがどの日の分になるのかを計算し、その日の0時を返す
    /// </summary>
    public static DateTime AsDate(this DateTime value, TimeSpan offset)
    {
        return value.AsInterval(offset, TimeSpan.FromDays(1), TimeSpan.Zero);
    }
}