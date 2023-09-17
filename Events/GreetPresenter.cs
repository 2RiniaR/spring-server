using Discord;
using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class GreetPresenter : DiscordMessagePresenterBase
{
    public override async Task RunAsync()
    {
        var wakeUp = await UserServices.As(Message.Author.Id).WakeUpAsync();
        if (wakeUp != null) await Message.ReplyAsync($"{Message.Author.Username}、おはよう！ ✨{wakeUp.MarvelousScore}");
    }
}