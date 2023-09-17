using Discord;
using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class LoginPresenter : DiscordMessagePresenterBase
{
    public override async Task RunAsync()
    {
        var login = await UserServices.As(Message.Author.Id).LoginAsync();
        if (login == null) return;

        var text = $"{UserNameText(Message.Author)}、今日も生きててえらい！ {ScoreDiffText(login.Score)}";
        await Message.ReplyAsync(text);
    }
}