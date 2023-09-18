using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class ContributionPresenter : SchedulerJobPresenterBase
{
    protected override async Task MainAsync()
    {
        await AppServices.RecordContributionAsync();
    }
}