namespace RineaR.Spring.Common;

public static class MasterManager
{
    public static string DiscordCommandPrefix => "!erai ";
    public static DayOfWeek WeeklyReset => DayOfWeek.Monday;
    public static TimeSpan DailyReset => new(0, 5, 00, 00);
    public static TimeSpan BedInStart => new(0, 18, 00, 00);
    public static TimeSpan BedInEnd => new(1, 1, 00, 00);
    public static string BedInPhrase => "おやすみ";
    public static TimeSpan WakeUpStart => new(1, 4, 00, 00);
    public static TimeSpan WakeUpEnd => new(1, 11, 00, 00);
    public static string[] PraiseEmotes => new[] { "👏", "👍", "🎊", "🎉", "🙌", "✨", "💪", "⭐" };
    public static string[] ComfortEmotes => new[] { "😥", "😱", "🍵", "🛡" };

    public static int LoginMarvelousScore => 1;
    public static int WakeUpMarvelousScore => 1;
    public static int WakeUpFailedPainfulScore => 1;
    public static int SendPraiseMarvelousScore => 1;
    public static int ReceivePraiseMarvelousScore => 1;
    public static int SendComfortMarvelousScore => 1;
    public static int ReceiveComfortPainfulScore => 1;
    public static int DailyContributionMarvelousScore => 1;
}