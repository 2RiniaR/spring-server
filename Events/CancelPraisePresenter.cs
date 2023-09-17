using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class CancelPraisePresenter : DiscordReactionPresenterBase
{
    public override async Task RunAsync()
    {
        await UserServices.As(Reaction.UserId)
            .CancelPraiseAsync(AuthorUser.Id, Reaction.MessageId, Reaction.Emote.GetHashCode());
    }
}