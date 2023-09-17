using Discord;

namespace RineaR.Spring.Common;

public static class DiscordExtensions
{
    public static bool IsMentioned(this IMessage message, IUser user)
    {
        return message.MentionedUserIds.Contains(user.Id);
    }
}