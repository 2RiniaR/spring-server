using Discord;
using RineaR.Spring.Common;

namespace RineaR.Spring.Events;

public class LoginPresenter : DiscordMessagePresenterBase
{
    public override async Task RunAsync()
    {
        var login = await UserServices.As(Message.Author.Id).LoginAsync();
        if (login != null) await Message.ReplyAsync($"{Message.Author.Username}、今日も生きててえらい！ ✨{login.Score}");
    }
}