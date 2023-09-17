namespace RineaR.Spring.Common;

public abstract class SchedulerJobRunner
{
    public abstract string Id { get; }
    public TimeSpan Interval { get; set; }
    public TimeSpan ConfigureTime { get; set; }
    public abstract void Run();
}

public class SchedulerJobRunner<T> : SchedulerJobRunner where T : SchedulerJobPresenterBase, new()
{
    public override string Id => nameof(T);

    public override void Run()
    {
        new T().RunAsync().Forget();
    }
}