using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace RineaR.Spring.Common;

public static class DiscordManager
{
    public static readonly DiscordSocketClient Client = new();
    private static readonly CommandService Commands = new();

    public static async Task InitializeAsync()
    {
        Client.Log += OnLog;
        Client.MessageReceived += OnMessageReceived;
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

    private static async Task OnMessageReceived(SocketMessage message)
    {
        // botは弾く
        if (message is not SocketUserMessage userMessage || userMessage.Author.IsBot) return;

        var argPos = 0;
        if (!userMessage.HasStringPrefix(MasterManager.DiscordCommandPrefix, ref argPos)) return;

        var context = new SocketCommandContext(Client, userMessage);
        await Commands.ExecuteAsync(context, argPos, null);
    }

    public static void ExecuteMatchedCommand(SocketUserMessage message, string prefix)
    {
        var argPos = 0;
        if (!message.HasStringPrefix(prefix, ref argPos)) return;

        var context = new SocketCommandContext(Client, message);
        Commands.ExecuteAsync(context, argPos, null);
    }

    public static void Execute<T>(SocketUserMessage message, Action<T>? onInitialize = null)
        where T : DiscordMessagePresenterBase, new()
    {
        var presenter = new T { Message = message };
        onInitialize?.Invoke(presenter);
        presenter.RunAsync();
    }

    public static void Execute<T>(SocketReaction reaction, Action<T>? onInitialize = null)
        where T : DiscordReactionPresenterBase, new()
    {
        var presenter = new T { Reaction = reaction };
        onInitialize?.Invoke(presenter);
        presenter.RunAsync();
    }
}