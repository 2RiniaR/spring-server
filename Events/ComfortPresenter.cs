using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class ComfortPresenter : DiscordReactionPresenterBase
{
    protected override async Task MainAsync()
    {
        var comfort = await UserServices.As(Reaction.UserId)
            .ComfortAsync(AuthorUser.Id, Reaction.MessageId, Reaction.Emote.GetHashCode());

        // ToDo: 一定数慰められたら何かしたい
    }
}