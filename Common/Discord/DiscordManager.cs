using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace RineaR.Spring.Common;

public static class DiscordManager
{
    public static readonly DiscordSocketClient Client = new(new DiscordSocketConfig
    {
        GatewayIntents = GatewayIntents.All,
    });

    private static readonly CommandService Commands = new();

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

    public static void RegisterCommands<T>() where T : ModuleBase<SocketCommandContext>
    {
        Commands.AddModuleAsync<T>(null);
    }

    public static async Task ExecuteMatchedCommandAsync(SocketUserMessage message, string prefix)
    {
        var argPos = 0;
        if (!message.HasStringPrefix(prefix, ref argPos)) return;

        var context = new SocketCommandContext(Client, message);
        await Commands.ExecuteAsync(context, argPos, null);
    }

    public static async Task ExecuteAsync<T>(SocketUserMessage message, Action<T>? onInitialize = null)
        where T : DiscordMessagePresenterBase, new()
    {
        var presenter = new T { Message = message };
        onInitialize?.Invoke(presenter);
        await presenter.RunAsync();
    }

    public static async Task ExecuteAsync<T>(SocketReaction reaction, IUser authorUser, Action<T>? onInitialize = null)
        where T : DiscordReactionPresenterBase, new()
    {
        var presenter = new T { Reaction = reaction, AuthorUser = authorUser };
        onInitialize?.Invoke(presenter);
        await presenter.RunAsync();
    }
}