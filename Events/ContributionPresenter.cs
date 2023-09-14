using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class ContributionPresenter : SchedulerJobPresenterBase
{
    public override async Task RunAsync()
    {
        await AppServices.RecordContributionAsync();
    }
}