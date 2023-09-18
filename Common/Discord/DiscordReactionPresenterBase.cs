using Discord;
using Discord.WebSocket;

namespace RineaR.Spring.Common;

public abstract class DiscordReactionPresenterBase : PresenterBase
{
    public SocketReaction Reaction { get; set; } = null!;
    public IUser AuthorUser { get; set; } = null!;
}