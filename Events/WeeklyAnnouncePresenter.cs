using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class WeeklyAnnouncePresenter : SchedulerJobPresenterBase
{
    protected override async Task MainAsync()
    {
        var ranking = await AppServices.RankingAsync();
    }
}