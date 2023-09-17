using RineaR.Spring.Common;
using RineaR.Spring.Events;

namespace RineaR.Spring;

public static class SchedulerEntry
{
    public static void RegisterEvents()
    {
        SchedulerManager.RegisterDaily<ContributionPresenter>(MasterManager.DailyReset);
        SchedulerManager.RegisterWeekly<WeeklyAnnouncePresenter>(MasterManager.WeeklyReset, MasterManager.DailyReset);
    }
}