using Discord;
using Discord.WebSocket;

namespace RineaR.Spring.Common;

public abstract class DiscordPresenterBase
{
    protected string UserNameText(IUser user)
    {
        return (user as SocketGuildUser)?.Nickname ?? user.Username;
    }

    protected string ScoreDiffText(int diff)
    {
        return $"✨{diff:+#;-#;0}";
    }
}