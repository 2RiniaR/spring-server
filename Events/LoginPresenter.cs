using Discord;
using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class LoginPresenter : DiscordMessagePresenterBase
{
    protected override async Task MainAsync()
    {
        var login = await UserServices.As(Message.Author.Id).LoginAsync();
        if (login == null) return;

        var text = $"{UserNameText(Message.Author)}、今日も生きててえらい！ {MarvelousScoreDiffText(login.MarvelousScore)}";
        await Message.ReplyAsync(text);
    }
}