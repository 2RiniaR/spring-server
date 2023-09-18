using Discord.WebSocket;

namespace RineaR.Spring.Common;

public abstract class DiscordMessagePresenterBase : PresenterBase
{
    public SocketUserMessage Message { get; set; } = null!;
}