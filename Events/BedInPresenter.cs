using Discord;
using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class BedInPresenter : DiscordMessagePresenterBase
{
    public override async Task RunAsync()
    {
        var bedIn = await UserServices.As(Message.Author.Id).BedInAsync();
        if (bedIn == null) return;
        await Message.ReplyAsync("おやすみなさい。");
    }
}