﻿using Discord;
using Discord.WebSocket;

namespace RineaR.Spring.Common;

public abstract class DiscordReactionPresenterBase : DiscordPresenterBase
{
    public SocketReaction Reaction { get; set; } = null!;
    public IUserMessage Message { get; set; } = null!;
    public abstract Task RunAsync();
}