namespace RineaR.Spring.Common;

public static class MasterManager
{
    public static string DiscordCommandPrefix => "!erai ";
    public static DayOfWeek WeeklyReset => DayOfWeek.Monday;
    public static TimeSpan DailyReset => new(0, 5, 00, 00);
    public static TimeSpan BedInStart => new(0, 18, 00, 00);
    public static TimeSpan BedInEnd => new(1, 1, 00, 00);
    public static TimeSpan WakeUpStart => new(1, 4, 00, 00);
    public static TimeSpan WakeUpEnd => new(1, 11, 00, 00);

    public static int LoginScore => 1;
    public static int WakeUpScore => 1;
    public static int SendPraiseScore => 1;
    public static int ReceivePraiseScore => 1;
    public static int DailyContributionScore => 1;
}