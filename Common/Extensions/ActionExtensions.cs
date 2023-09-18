namespace RineaR.Spring.Common;

public static class ActionExtensions
{
    public static Func<T, Task> ToAsync<T>(this Action<T>? action)
    {
        return x =>
        {
            action?.Invoke(x);
            return Task.CompletedTask;
        };
    }
}