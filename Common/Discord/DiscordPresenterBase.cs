using Discord;
using Discord.WebSocket;

namespace RineaR.Spring.Common;

public abstract class DiscordPresenterBase
{
    protected string UserNameText(IUser user)
    {
        return (user as SocketGuildUser)?.Nickname ?? user.Username;
    }

    protected string MarvelousScoreDiffText(int diff)
    {
        return $"`✨{diff:+#;-#;0}`";
    }

    protected string PainfulScoreDiffText(int diff)
    {
        return $"`💊{diff:+#;-#;0}`";
    }

    protected string TimeText(TimeSpan time)
    {
        return $"{time.Hours:D2}:{time.Minutes:D2}";
    }

    protected string MarvelousScoreIcon => "✨";
    protected string MarvelousScoreName => "えらいポイント";
    protected string PainfulScoreIcon => "💊";
    protected string PainfulScoreName => "よしよしポイント";
}