namespace RineaR.Spring.Common;

public static class DateTimeExtensions
{
    /// <summary>
    /// 特定の周期において、どの日に含まれるかを取得する
    /// 例） 毎日5時にアプリがリセットされる（interval: 1日, offset: 5時間）として、valueがどの日の分になるのかを取得する
    /// </summary>
    public static DateTime AsInterval(this DateTime value, TimeSpan interval, TimeSpan offset)
    {
        return value.AddTicks(-(value.Ticks - offset.Ticks) % interval.Ticks);
    }
}