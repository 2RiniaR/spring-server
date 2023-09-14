namespace RineaR.Spring.Common;

/// <summary>
/// 登録された型のインスタンスを毎回生成してイテレーションを回す
/// </summary>
public class TypeGenerator<T>
{
    private readonly List<Func<T>> _generators = new();

    public void ForEach(Action<T> action)
    {
        foreach (var generator in _generators)
        {
            action(generator());
        }
    }

    public async Task ForEachAsync(Func<T, Task> action)
    {
        foreach (var generator in _generators)
        {
            await action(generator());
        }
    }

    public void Register<TDerived>() where TDerived : T, new()
    {
        _generators.Add(() => new TDerived());
    }
}