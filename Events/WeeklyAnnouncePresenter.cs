using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class WeeklyAnnouncePresenter : SchedulerJobPresenterBase
{
    public override async Task RunAsync()
    {
        var ranking = await AppServices.RankingAsync();
    }
}