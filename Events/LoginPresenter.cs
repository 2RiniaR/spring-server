using Discord;
using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class LoginPresenter : DiscordMessagePresenterBase
{
    protected override async Task MainAsync()
    {
        var login = await UserServices.As(Message.Author.Id).LoginAsync();
        if (login == null) return;

        var text = $"{Format.UserName(Message.Author)}、今日も生きててえらい！ {Format.MarvelousScoreDiff(login.MarvelousScore)}";
        await Message.ReplyAsync(text);
    }
}