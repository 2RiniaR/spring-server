using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class CancelComfortPresenter : DiscordReactionPresenterBase
{
    public override async Task RunAsync()
    {
        await UserServices.As(Reaction.UserId)
            .CancelComfortAsync(AuthorUser.Id, Reaction.MessageId, Reaction.Emote.GetHashCode());
    }
}