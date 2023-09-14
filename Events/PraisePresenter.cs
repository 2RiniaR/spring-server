using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class PraisePresenter : DiscordReactionPresenterBase
{
    public override async Task RunAsync()
    {
        await UserServices.As(Reaction.UserId)
            .PraiseAsync(Reaction.User.Value.Id, Message.Id, Reaction.Emote.GetHashCode());
    }
}