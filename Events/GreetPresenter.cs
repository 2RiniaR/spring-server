using Discord;
using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class GreetPresenter : DiscordMessagePresenterBase
{
    protected override async Task MainAsync()
    {
        var wakeUp = await UserServices.As(Message.Author.Id).WakeUpAsync();
        if (wakeUp == null) return;

        var name = Format.Sanitize(Message.Author.Username);
        var score = Format.MarvelousScoreDiff(wakeUp.MarvelousScore);
        await Message.ReplyAsync($"{name}、おはよう！ {score}");
    }
}