using Discord;
using Discord.WebSocket;

namespace RineaR.Spring.Common;

public static class DiscordManager
{
    public static readonly DiscordSocketClient Client = new(new DiscordSocketConfig
    {
        GatewayIntents = GatewayIntents.All,
    });

    public static async Task InitializeAsync()
    {
        Client.Log += OnLog;
        await Client.LoginAsync(TokenType.Bot, EnvironmentManager.DiscordToken);
        await Client.StartAsync();
    }

    private static Task OnLog(LogMessage content)
    {
        Console.WriteLine(content.ToString());
        return Task.CompletedTask;
    }

    public static async Task ExecuteAsync<T>(SocketUserMessage message, Func<T, Task>? onInitializeAsync = null)
        where T : DiscordMessagePresenterBase, new()
    {
        var presenter = new T { Message = message };
        if (onInitializeAsync != null) await onInitializeAsync.Invoke(presenter);
        await presenter.RunAsync();
    }

    public static async Task ExecuteAsync<T>(SocketReaction reaction, IUser authorUser,
        Func<T, Task>? onInitializeAsync = null)
        where T : DiscordReactionPresenterBase, new()
    {
        var presenter = new T { Reaction = reaction, AuthorUser = authorUser };
        if (onInitializeAsync != null) await onInitializeAsync.Invoke(presenter);
        await presenter.RunAsync();
    }
}