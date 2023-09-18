using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class CancelPraisePresenter : DiscordReactionPresenterBase
{
    protected override async Task MainAsync()
    {
        await UserServices.As(Reaction.UserId)
            .CancelPraiseAsync(AuthorUser.Id, Reaction.MessageId, Reaction.Emote.GetHashCode());
    }
}