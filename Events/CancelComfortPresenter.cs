using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class CancelComfortPresenter : DiscordReactionPresenterBase
{
    protected override async Task MainAsync()
    {
        await UserServices.As(Reaction.UserId)
            .CancelComfortAsync(AuthorUser.Id, Reaction.MessageId, Reaction.Emote.GetHashCode());
    }
}