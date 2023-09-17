using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class PraisePresenter : DiscordReactionPresenterBase
{
    public override async Task RunAsync()
    {
        var praise = await UserServices.As(Reaction.UserId)
            .PraiseAsync(AuthorUser.Id, Reaction.MessageId, Reaction.Emote.GetHashCode());

        // ToDo: 一定数褒められたら何かしたい
    }
}