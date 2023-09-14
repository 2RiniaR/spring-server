using Discord.WebSocket;

namespace RineaR.Spring.Common;

public abstract class DiscordMessagePresenterBase
{
    public SocketUserMessage Message { get; set; } = null!;

    public abstract Task RunAsync();
}